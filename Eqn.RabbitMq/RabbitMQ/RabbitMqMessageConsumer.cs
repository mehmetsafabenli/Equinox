using System.Collections.Concurrent;
using Eqn.Core;
using Eqn.Core.DependencyInjection;
using Eqn.Core.ExceptionHandling;
using Eqn.Core.Microsoft.Extensions.Logging;
using Eqn.Core.Threading;
using Eqn.Threading.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Eqn.RabbitMq.RabbitMQ;

public class RabbitMqMessageConsumer : IRabbitMqMessageConsumer, ITransientDependency, IDisposable
{
    public ILogger<RabbitMqMessageConsumer> Logger { get; set; }

    protected IConnectionPool ConnectionPool { get; }

    protected IExceptionNotifier ExceptionNotifier { get; }

    protected  EqnAsyncTimer Timer { get; }

    protected ExchangeDeclareConfiguration Exchange { get; private set; }

    protected QueueDeclareConfiguration Queue { get; private set; }

    protected string ConnectionName { get; private set; }

    protected ConcurrentBag<Func<IModel, BasicDeliverEventArgs, Task>> Callbacks { get; }

    protected IModel Channel { get; private set; }

    protected ConcurrentQueue<QueueBindCommand> QueueBindCommands { get; }

    protected object ChannelSendSyncLock { get; } = new object();

    public RabbitMqMessageConsumer(
        IConnectionPool connectionPool,
         EqnAsyncTimer timer,
        IExceptionNotifier exceptionNotifier)
    {
        ConnectionPool = connectionPool;
        Timer = timer;
        ExceptionNotifier = exceptionNotifier;
        Logger = NullLogger<RabbitMqMessageConsumer>.Instance;

        QueueBindCommands = new ConcurrentQueue<QueueBindCommand>();
        Callbacks = new ConcurrentBag<Func<IModel, BasicDeliverEventArgs, Task>>();

        Timer.Period = 5000; //5 sec.
        Timer.Elapsed = Timer_Elapsed;
        Timer.RunOnStart = true;
    }

    public void Initialize(
        [NotNull] ExchangeDeclareConfiguration exchange,
        [NotNull] QueueDeclareConfiguration queue,
        string connectionName = null)
    {
        Exchange = Check.NotNull(exchange, nameof(exchange));
        Queue = Check.NotNull(queue, nameof(queue));
        ConnectionName = connectionName;
        Timer.Start();
    }

    public virtual async Task BindAsync(string routingKey)
    {
        QueueBindCommands.Enqueue(new QueueBindCommand(QueueBindType.Bind, routingKey));
        await TrySendQueueBindCommandsAsync();
    }

    public virtual async Task UnbindAsync(string routingKey)
    {
        QueueBindCommands.Enqueue(new QueueBindCommand(QueueBindType.Unbind, routingKey));
        await TrySendQueueBindCommandsAsync();
    }

    protected virtual async Task TrySendQueueBindCommandsAsync()
    {
        try
        {
            while (!QueueBindCommands.IsEmpty)
            {
                if (Channel == null || Channel.IsClosed)
                {
                    return;
                }

                lock (ChannelSendSyncLock)
                {
                    if (QueueBindCommands.TryPeek(out var command))
                    {
                        switch (command.Type)
                        {
                            case QueueBindType.Bind:
                                Channel.QueueBind(
                                    queue: Queue.QueueName,
                                    exchange: Exchange.ExchangeName,
                                    routingKey: command.RoutingKey
                                );
                                break;
                            case QueueBindType.Unbind:
                                Channel.QueueUnbind(
                                    queue: Queue.QueueName,
                                    exchange: Exchange.ExchangeName,
                                    routingKey: command.RoutingKey
                                );
                                break;
                            default:
                                throw new  EqnException($"Unknown {nameof(QueueBindType)}: {command.Type}");
                        }

                        QueueBindCommands.TryDequeue(out command);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex, LogLevel.Warning);
            await ExceptionNotifier.NotifyAsync(ex, logLevel: LogLevel.Warning);
        }
    }

    public virtual void OnMessageReceived(Func<IModel, BasicDeliverEventArgs, Task> callback)
    {
        Callbacks.Add(callback);
    }

    protected virtual async Task Timer_Elapsed( EqnAsyncTimer timer)
    {
        if (Channel == null || Channel.IsOpen == false)
        {
            await TryCreateChannelAsync();
            await TrySendQueueBindCommandsAsync();
        }
    }

    protected virtual async Task TryCreateChannelAsync()
    {
        await DisposeChannelAsync();

        try
        {
            Channel = ConnectionPool
                .Get(ConnectionName)
                .CreateModel();

            Channel.ExchangeDeclare(
                exchange: Exchange.ExchangeName,
                type: Exchange.Type,
                durable: Exchange.Durable,
                autoDelete: Exchange.AutoDelete,
                arguments: Exchange.Arguments
            );

            Channel.QueueDeclare(
                queue: Queue.QueueName,
                durable: Queue.Durable,
                exclusive: Queue.Exclusive,
                autoDelete: Queue.AutoDelete,
                arguments: Queue.Arguments
            );

            if (Queue.PrefetchCount.HasValue)
            {
                Channel.BasicQos(0, Queue.PrefetchCount.Value, false);
            }
            
        }
        catch (Exception ex)
        {
            Logger.LogException(ex, LogLevel.Warning);
            await ExceptionNotifier.NotifyAsync(ex, logLevel: LogLevel.Warning);
        }
    }

    protected virtual async Task HandleIncomingMessageAsync(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
    {
        try
        {
            foreach (var callback in Callbacks)
            {
                await callback(Channel, basicDeliverEventArgs);
            }

            Channel.BasicAck(basicDeliverEventArgs.DeliveryTag, multiple: false);
        }
        catch (Exception ex)
        {
            try
            {
                Channel.BasicNack(
                    basicDeliverEventArgs.DeliveryTag,
                    multiple: false,
                    requeue: true
                );
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }

            Logger.LogException(ex);
            await ExceptionNotifier.NotifyAsync(ex);
        }
    }

    protected virtual async Task DisposeChannelAsync()
    {
        if (Channel == null)
        {
            return;
        }

        try
        {
            Channel.Dispose();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex, LogLevel.Warning);
            await ExceptionNotifier.NotifyAsync(ex, logLevel: LogLevel.Warning);
        }
    }

    protected virtual void DisposeChannel()
    {
        if (Channel == null)
        {
            return;
        }

        try
        {
            Channel.Dispose();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex, LogLevel.Warning);
            AsyncHelper.RunSync(() => ExceptionNotifier.NotifyAsync(ex, logLevel: LogLevel.Warning));
        }
    }

    public virtual void Dispose()
    {
        Timer.Stop();
        DisposeChannel();
    }

    protected class QueueBindCommand
    {
        public QueueBindType Type { get; }

        public string RoutingKey { get; }

        public QueueBindCommand(QueueBindType type, string routingKey)
        {
            Type = type;
            RoutingKey = routingKey;
        }
    }

    protected enum QueueBindType
    {
        Bind,
        Unbind
    }
}
