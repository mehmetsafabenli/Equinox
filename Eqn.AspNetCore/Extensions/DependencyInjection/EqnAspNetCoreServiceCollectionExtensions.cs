using Eqn.Core.Microsoft.Extensions.DependencyInjection;

namespace Eqn.AspNetCore.Extensions.DependencyInjection;

public static class EqnAspNetCoreServiceCollectionExtensions
{
    public static IWebHostEnvironment GetHostingEnvironment(this IServiceCollection services)
    {
        var hostingEnvironment = services.GetSingletonInstanceOrNull<IWebHostEnvironment>();

        if (hostingEnvironment == null)
        {
            return new EmptyHostingEnvironment()
            {
                EnvironmentName = Environments.Development
            };
        }

        return hostingEnvironment;
    }
}
