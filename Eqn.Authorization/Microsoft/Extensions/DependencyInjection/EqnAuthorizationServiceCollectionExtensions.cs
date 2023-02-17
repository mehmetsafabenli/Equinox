using Eqn.Authorization.Abstractions.Authorization;
using Eqn.Authorization.Abstractions.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Eqn.Authorization.Microsoft.Extensions.DependencyInjection;

public static class EqnAuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddAlwaysAllowAuthorization(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Singleton<IAuthorizationService, AlwaysAllowAuthorizationService>());
        services.Replace(ServiceDescriptor.Singleton<IEqnAuthorizationService, AlwaysAllowAuthorizationService>());
        services.Replace(ServiceDescriptor.Singleton<IMethodInvocationAuthorizationService, AlwaysAllowMethodInvocationAuthorizationService>());
        return services.Replace(ServiceDescriptor.Singleton<IPermissionChecker, AlwaysAllowPermissionChecker>());
    }
}
