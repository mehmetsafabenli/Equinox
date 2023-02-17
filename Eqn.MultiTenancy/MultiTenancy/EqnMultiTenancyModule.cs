using Eqn.Core.Microsoft.Extensions.DependencyInjection;
using Eqn.Core.Modularity;
using Eqn.Data.Data;
using Eqn.EventBus.Abstraction.EventBus.Abstractions;
using Eqn.MultiTenancy.MultiTenancy.ConfigurationStore;
using Eqn.Security.Eqn.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Eqn.MultiTenancy.MultiTenancy;

//TODO: Create a Volo.Eqn.MultiTenancy.Abstractions package?

[DependsOn(
    typeof(EqnDataModule),
    typeof(EqnSecurityModule),
    typeof(EqnEventBusAbstractionsModule)
    )]
public class EqnMultiTenancyModule : EqnModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<ICurrentTenantAccessor>(AsyncLocalCurrentTenantAccessor.Instance);

        var configuration = context.Services.GetConfiguration();
        Configure<EqnDefaultTenantStoreOptions>(configuration);
    }
}
