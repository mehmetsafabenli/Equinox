namespace Eqn.RabbitMq.RabbitMQ;

public class EqnRabbitMqOptions
{
    public RabbitMqConnections Connections { get; }

    public EqnRabbitMqOptions()
    {
        Connections = new RabbitMqConnections();
    }
}
