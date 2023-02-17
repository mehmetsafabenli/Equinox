using Eqn.Core.DependencyInjection;
using JetBrains.Annotations;

namespace Eqn.AspNetCore.Eqn;

public static class ServiceProviderAccessorExtensions
{
    [CanBeNull]
    public static HttpContext GetHttpContext(this IServiceProviderAccessor serviceProviderAccessor)
    {
        return serviceProviderAccessor.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
    }
}
