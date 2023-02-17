using Eqn.Core;
using Eqn.Core.Microsoft.Extensions.DependencyInjection;
using Eqn.Core.Modularity;
using Eqn.Json.Json;
using Eqn.Threading.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Eqn.Kafka.Kafka;

[DependsOn(
    typeof(EqnJsonModule),
    typeof(EqnThreadingModule)
)]
public class EqnKafkaModule : EqnModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<EqnKafkaOptions>(configuration.GetSection("Kafka"));
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        context.ServiceProvider
            .GetRequiredService<IConsumerPool>()
            .Dispose();

        context.ServiceProvider
            .GetRequiredService<IProducerPool>()
            .Dispose();
    }
}
