using Eqn.AspNetCore.Mvc.AspNetCore.Mvc.Validation;
using Eqn.Core;
using Eqn.Core.Aspects;
using Eqn.Core.DependencyInjection;
using Eqn.Core.System;
using Eqn.Features.Features;
using Eqn.Guids.Guids;
using Eqn.Localization.Abstraction.Extensions.Localization;
using Eqn.Localization.Localization;
using Eqn.MultiTenancy.MultiTenancy;
using Eqn.ObjectMapper.ObjectMapping;
using Eqn.Security.Eqn.Users;
using Eqn.Timing.Timing;
using Eqn.Uow.Uow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Eqn.AspNetCore.Mvc.AspNetCore.Mvc;

public abstract class EqnController : Controller, IAvoidDuplicateCrossCuttingConcerns
{
    public IEqnLazyServiceProvider LazyServiceProvider { get; set; }

    [Obsolete("Use LazyServiceProvider instead.")]
    public IServiceProvider ServiceProvider { get; set; }

    protected IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

    protected Type ObjectMapperContext { get; set; }
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
        ObjectMapperContext == null
            ? provider.GetRequiredService<IObjectMapper>()
            : (IObjectMapper)provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));

    protected IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance);

    protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();

    protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    protected IAuthorizationService AuthorizationService => LazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();

    protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

    protected IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

    protected IModelStateValidator ModelValidator => LazyServiceProvider.LazyGetRequiredService<IModelStateValidator>();

    protected IFeatureChecker FeatureChecker => LazyServiceProvider.LazyGetRequiredService<IFeatureChecker>();
    protected IStringLocalizerFactory StringLocalizerFactory => LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();

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

    public List<string> AppliedCrossCuttingConcerns { get; } = new List<string>();

    protected virtual void ValidateModel()
    {
        ModelValidator?.Validate(ModelState);
    }

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

    protected virtual RedirectResult RedirectSafely(string returnUrl, string returnUrlHash = null)
    {
        return Redirect(GetRedirectUrl(returnUrl, returnUrlHash));
    }

    protected virtual string GetRedirectUrl(string returnUrl, string returnUrlHash = null)
    {
        returnUrl = NormalizeReturnUrl(returnUrl);

        if (!returnUrlHash.IsNullOrWhiteSpace())
        {
            returnUrl = returnUrl + returnUrlHash;
        }

        return returnUrl;
    }

    protected virtual string NormalizeReturnUrl(string returnUrl)
    {
        if (returnUrl.IsNullOrEmpty())
        {
            return GetAppHomeUrl();
        }

        if (Url.IsLocalUrl(returnUrl))
        {
            return returnUrl;
        }

        return GetAppHomeUrl();
    }

    protected virtual string GetAppHomeUrl()
    {
        return Url.Content("~/");
    }
}
