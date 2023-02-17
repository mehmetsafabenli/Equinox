using Eqn.Core;
using Eqn.Core.DependencyInjection;
using Eqn.Localization.Abstraction.Extensions.Localization;
using Eqn.Localization.Localization;
using Eqn.MultiTenancy.MultiTenancy;
using Eqn.Security.Eqn.Users;
using Eqn.Timing.Timing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Eqn.AspNetCore.SignalR.SignalR;

public abstract class EqnHub<T> : Hub<T>
    where T : class
{
    public IEqnLazyServiceProvider LazyServiceProvider { get; set; }

    public IServiceProvider ServiceProvider { get; set; }

    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetService<ILoggerFactory>();

    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance);

    protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetService<ICurrentUser>();

    protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetService<ICurrentTenant>();

    protected IAuthorizationService AuthorizationService => LazyServiceProvider.LazyGetService<IAuthorizationService>();

    protected IClock Clock => LazyServiceProvider.LazyGetService<IClock>();

    protected IStringLocalizerFactory StringLocalizerFactory => LazyServiceProvider.LazyGetService<IStringLocalizerFactory>();

    protected IStringLocalizer L {
        get {
            if (_localizer == null)
            {
                _localizer = CreateLocalizer();
            }

            return _localizer;
        }
    }
    private IStringLocalizer _localizer;

    protected Type LocalizationResource {
        get => _localizationResource;
        set {
            _localizationResource = value;
            _localizer = null;
        }
    }
    private Type _localizationResource = typeof(DefaultResource);

    protected virtual IStringLocalizer CreateLocalizer()
    {
        if (LocalizationResource != null)
        {
            return StringLocalizerFactory.Create(LocalizationResource);
        }

        var localizer = StringLocalizerFactory.CreateDefaultOrNull();
        if (localizer == null)
        {
            throw new EqnException($"Set {nameof(LocalizationResource)} or define the default localization resource type (by configuring the {nameof(EqnLocalizationOptions)}.{nameof(EqnLocalizationOptions.DefaultResourceType)}) to be able to use the {nameof(L)} object!");
        }

        return localizer;
    }
}
