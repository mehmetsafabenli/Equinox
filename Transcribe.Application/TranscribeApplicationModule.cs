using Eqn.AspNetCore.SignalR.SignalR;
using Eqn.Core.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Transcribe.Application;

[DependsOn(typeof(EqnAspNetCoreSignalRModule))]
public class TranscribeApplicationModule : EqnModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient();
    }
}