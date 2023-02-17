﻿using System.Collections.Concurrent;
using Eqn.Core.System;
using Eqn.Core.System.Collections.Generic;
using Eqn.Core.Threading;
using Eqn.Localization.Abstraction.Extensions.Localization;
using Eqn.Localization.Localization.External;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Eqn.Localization.Localization;

public class EqnStringLocalizerFactory : IStringLocalizerFactory, IEqnStringLocalizerFactory
{
    protected internal EqnLocalizationOptions EqnLocalizationOptions { get; }
    protected ResourceManagerStringLocalizerFactory InnerFactory { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected IExternalLocalizationStore ExternalLocalizationStore { get; }
    protected ConcurrentDictionary<string, StringLocalizerCacheItem> LocalizerCache { get; }
    protected SemaphoreSlim LocalizerCacheSemaphore { get; } = new(1, 1);

    public EqnStringLocalizerFactory(
        ResourceManagerStringLocalizerFactory innerFactory,
        IOptions<EqnLocalizationOptions> eqnLocalizationOptions,
        IServiceProvider serviceProvider,
        IExternalLocalizationStore externalLocalizationStore)
    {
        InnerFactory = innerFactory;
        ServiceProvider = serviceProvider;
        ExternalLocalizationStore = externalLocalizationStore;
        EqnLocalizationOptions = eqnLocalizationOptions.Value;

        LocalizerCache = new ConcurrentDictionary<string, StringLocalizerCacheItem>();
    }

    public virtual IStringLocalizer Create(Type resourceType)
    {
        return Create(resourceType, lockCache: true);
    }
    
    private IStringLocalizer Create(Type resourceType, bool lockCache)
    {
        var resource = EqnLocalizationOptions.Resources.GetOrNull(resourceType);
        if (resource == null)
        {
            return InnerFactory.Create(resourceType);
        }

        return CreateInternal(resource.ResourceName, resource, lockCache);
    }
    
    public IStringLocalizer CreateByResourceNameOrNull(string resourceName)
    {
        return CreateByResourceNameOrNullInternal(resourceName, lockCache: true);
    }
    
    private IStringLocalizer CreateByResourceNameOrNullInternal(
        string resourceName,
        bool lockCache)
    {
        var resource = EqnLocalizationOptions.Resources.GetOrDefault(resourceName);
        if (resource == null)
        {
            resource = ExternalLocalizationStore.GetResourceOrNull(resourceName);
            if (resource == null)
            {
                return null;
            }
        }

        return CreateInternal(resourceName, resource, lockCache);
    }

    public Task<IStringLocalizer> CreateByResourceNameOrNullAsync(string resourceName)
    {
        return CreateByResourceNameOrNullInternalAsync(resourceName, lockCache: true);
    }
    
    private async Task<IStringLocalizer> CreateByResourceNameOrNullInternalAsync(
        string resourceName,
        bool lockCache)
    {
        var resource = EqnLocalizationOptions.Resources.GetOrDefault(resourceName);
        if (resource == null)
        {
            resource = await ExternalLocalizationStore.GetResourceOrNullAsync(resourceName);
            if (resource == null)
            {
                return null;
            }
        }

        return await CreateInternalAsync(resourceName, resource, lockCache);
    }

    private IStringLocalizer CreateInternal(
        string resourceName,
        LocalizationResourceBase resource,
        bool lockCache)
    {
        if (LocalizerCache.TryGetValue(resourceName, out var cacheItem))
        {
            return cacheItem.Localizer;
        }

        IStringLocalizer GetOrCreateLocalizer()
        {
            // Double check
            if (LocalizerCache.TryGetValue(resourceName, out var cacheItem2))
            {
                return cacheItem2.Localizer;
            }

            return LocalizerCache.GetOrAdd(
                resourceName,
                _ => CreateStringLocalizerCacheItem(resource)
            ).Localizer;
        }

        if (lockCache)
        {
            using (LocalizerCacheSemaphore.Lock())
            {
                return GetOrCreateLocalizer();
            }
        }
        else
        {
            return GetOrCreateLocalizer();
        }
    }
    
    private async Task<IStringLocalizer> CreateInternalAsync(
        string resourceName,
        LocalizationResourceBase resource,
        bool lockCache)
    {
        if (LocalizerCache.TryGetValue(resourceName, out var cacheItem))
        {
            return cacheItem.Localizer;
        }

        async Task<IStringLocalizer> GetOrCreateLocalizerAsync()
        {
            // Double check
            if (LocalizerCache.TryGetValue(resourceName, out var cacheItem2))
            {
                return cacheItem2.Localizer;
            }

            var newCacheItem = await CreateStringLocalizerCacheItemAsync(resource);
            LocalizerCache[resourceName] = newCacheItem;
            return newCacheItem.Localizer;
        }

        if (lockCache)
        {
            using (await LocalizerCacheSemaphore.LockAsync())
            {
                return await GetOrCreateLocalizerAsync();
            }
        }
        else
        {
            return await GetOrCreateLocalizerAsync();
        }
    }

    private StringLocalizerCacheItem CreateStringLocalizerCacheItem(LocalizationResourceBase resource)
    {
        foreach (var globalContributorType in EqnLocalizationOptions.GlobalContributors)
        {
            resource.Contributors.Add(
                Activator
                    .CreateInstance(globalContributorType)
                    .As<ILocalizationResourceContributor>()
            );
        }

        var context = new LocalizationResourceInitializationContext(resource, ServiceProvider);

        foreach (var contributor in resource.Contributors)
        {
            contributor.Initialize(context);
        }

        return new StringLocalizerCacheItem(
            new EqnDictionaryBasedStringLocalizer(
                resource,
                resource
                    .BaseResourceNames
                    .Select(x => CreateByResourceNameOrNullInternal(x, lockCache: false))
                    .Where(x => x != null)
                    .ToList(),
                EqnLocalizationOptions
            )
        );
    }
    
    private async Task<StringLocalizerCacheItem> CreateStringLocalizerCacheItemAsync(LocalizationResourceBase resource)
    {
        foreach (var globalContributorType in EqnLocalizationOptions.GlobalContributors)
        {
            resource.Contributors.Add(
                Activator
                    .CreateInstance(globalContributorType)
                    .As<ILocalizationResourceContributor>()
            );
        }

        var context = new LocalizationResourceInitializationContext(resource, ServiceProvider);

        foreach (var contributor in resource.Contributors)
        {
            contributor.Initialize(context);
        }
        
        var baseLocalizers = new List<IStringLocalizer>();
        
        foreach (var baseResourceName in resource.BaseResourceNames)
        {
            var baseLocalizer = await CreateByResourceNameOrNullInternalAsync(baseResourceName, lockCache: false);
            if (baseLocalizer != null)
            {
                baseLocalizers.Add(baseLocalizer);
            }
        }

        return new StringLocalizerCacheItem(
            new EqnDictionaryBasedStringLocalizer(
                resource,
                baseLocalizers,
                EqnLocalizationOptions
            )
        );
    }

    public virtual IStringLocalizer Create(string baseName, string location)
    {
        return InnerFactory.Create(baseName, location);
    }

    internal static void Replace(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Singleton<IStringLocalizerFactory, EqnStringLocalizerFactory>());
        services.AddSingleton<ResourceManagerStringLocalizerFactory>();
    }

    protected class StringLocalizerCacheItem
    {
        public EqnDictionaryBasedStringLocalizer Localizer { get; }

        public StringLocalizerCacheItem(EqnDictionaryBasedStringLocalizer localizer)
        {
            Localizer = localizer;
        }
    }

    public IStringLocalizer CreateDefaultOrNull()
    {
        if (EqnLocalizationOptions.DefaultResourceType == null)
        {
            return null;
        }

        return Create(EqnLocalizationOptions.DefaultResourceType);
    }
}