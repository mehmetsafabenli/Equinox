using Eqn.Core.DependencyInjection;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Eqn.AspNetCore.Mvc.AspNetCore.Mvc.AntiForgery;

public class AspNetCoreEqnAntiForgeryManager : IEqnAntiForgeryManager, ITransientDependency
{
    protected EqnAntiForgeryOptions Options { get; }

    protected HttpContext HttpContext => _httpContextAccessor.HttpContext;

    private readonly IAntiforgery _antiforgery;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AspNetCoreEqnAntiForgeryManager(
        IAntiforgery antiforgery,
        IHttpContextAccessor httpContextAccessor,
        IOptions<EqnAntiForgeryOptions> options)
    {
        _antiforgery = antiforgery;
        _httpContextAccessor = httpContextAccessor;
        Options = options.Value;
    }

    public virtual void SetCookie()
    {
        HttpContext.Response.Cookies.Append(
            Options.TokenCookie.Name,
            GenerateToken(),
            Options.TokenCookie.Build(HttpContext)
        );
    }

    public virtual string GenerateToken()
    {
        return _antiforgery.GetAndStoreTokens(_httpContextAccessor.HttpContext).RequestToken;
    }
}
