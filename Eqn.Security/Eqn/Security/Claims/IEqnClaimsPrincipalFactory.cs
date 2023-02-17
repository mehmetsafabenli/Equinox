using System.Security.Claims;

namespace Eqn.Security.Eqn.Security.Claims;

public interface IEqnClaimsPrincipalFactory
{
    Task<ClaimsPrincipal> CreateAsync(ClaimsPrincipal existsClaimsPrincipal = null);
}
