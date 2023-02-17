using Eqn.Core;
using Eqn.Core.Modularity;
using Eqn.Core.Threading;
using Eqn.Threading.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eqn.BackGroundWorkers.BackgroundWorkers;

[DependsOn(
    typeof(EqnThreadingModule)
    )]
public class EqnBackgroundWorkersModule : EqnModule
{
    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<EqnBackgroundWorkerOptions>>().Value;
        if (options.IsEnabled)
        {
            await context.ServiceProvider
                .GetRequiredService<IBackgroundWorkerManager>()
                .StartAsync();
        }
    }

    public async override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<EqnBackgroundWorkerOptions>>().Value;
        if (options.IsEnabled)
        {
            await context.ServiceProvider
                .GetRequiredService<IBackgroundWorkerManager>()
                .StopAsync();
        }
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationShutdownAsync(context));
    }
}
