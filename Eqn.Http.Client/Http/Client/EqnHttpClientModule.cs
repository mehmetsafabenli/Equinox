using Eqn.Castle.Core;
using Eqn.Core.Modularity;
using Eqn.ExceptionHandling.ExceptionHandling;
using Eqn.Http.Client.Http.Client.DynamicProxying;
using Eqn.Http.Http;
using Eqn.MultiTenancy.MultiTenancy;
using Eqn.RemoteServices.RemoteServices;
using Eqn.Threading.Threading;
using Eqn.Validations.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Eqn.Http.Client.Http.Client;

[DependsOn(
    typeof(EqnHttpModule),
    typeof(EqnCastleCoreModule),
    typeof(EqnThreadingModule),
    typeof(EqnMultiTenancyModule),
    typeof(EqnValidationModule),
    typeof(EqnExceptionHandlingModule),
    typeof(EqnRemoteServicesModule)
    )]
public class EqnHttpClientModule : EqnModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient();
        context.Services.AddTransient(typeof(DynamicHttpProxyInterceptorClientProxy<>));
    }
}
