using System.Security.Claims;
using Eqn.Authorization.Abstractions.Authorization;
using Eqn.Core.DependencyInjection;
using Eqn.Security.Eqn.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eqn.Authorization.Authorization;

[Dependency(ReplaceServices = true)]
public class EqnAuthorizationService : DefaultAuthorizationService, IEqnAuthorizationService, ITransientDependency
{
    public IServiceProvider ServiceProvider { get; }

    public ClaimsPrincipal CurrentPrincipal => _currentPrincipalAccessor.Principal;

    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public EqnAuthorizationService(
        IAuthorizationPolicyProvider policyProvider,
        IAuthorizationHandlerProvider handlers,
        ILogger<DefaultAuthorizationService> logger,
        IAuthorizationHandlerContextFactory contextFactory,
        IAuthorizationEvaluator evaluator,
        IOptions<AuthorizationOptions> options,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IServiceProvider serviceProvider)
        : base(
            policyProvider,
            handlers,
            logger,
            contextFactory,
            evaluator,
            options)
    {
        _currentPrincipalAccessor = currentPrincipalAccessor;
        ServiceProvider = serviceProvider;
    }
}
