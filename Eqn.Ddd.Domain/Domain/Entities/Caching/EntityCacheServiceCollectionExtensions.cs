using Eqn.Caching.Caching;
using Eqn.Json.SystemTextJson.Json.SystemTextJson;
using Eqn.Json.SystemTextJson.Json.SystemTextJson.Modifiers;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Eqn.Ddd.Domain.Domain.Entities.Caching;

public static class EntityCacheServiceCollectionExtensions
{
    public static IServiceCollection AddEntityCache<TEntity, TKey>(
        this IServiceCollection services,
        [CanBeNull] DistributedCacheEntryOptions cacheOptions = null)
        where TEntity : Entity<TKey>
    {
        services
            .TryAddTransient<
                IEntityCache<TEntity, TKey>,
                EntityCacheWithoutCacheItem<TEntity, TKey>
            >();
        services
            .TryAddTransient<EntityCacheWithoutCacheItem<TEntity, TKey>>();

        services.Configure<EqnDistributedCacheOptions>(options =>
        {
            options.ConfigureCache<TEntity>(cacheOptions ?? GetDefaultCacheOptions());
        });

        services.Configure<EqnSystemTextJsonSerializerModifiersOptions>(options =>
        {
            options.Modifiers.Add(new EqnIncludeNonPublicPropertiesModifiers<TEntity, TKey>().CreateModifyAction(x => x.Id));
        });

        return services;
    }

    public static IServiceCollection AddEntityCache<TEntity, TEntityCacheItem, TKey>(
        this IServiceCollection services,
        [CanBeNull] DistributedCacheEntryOptions cacheOptions = null)
        where TEntity : Entity<TKey>
        where TEntityCacheItem : class
    {
        services
            .TryAddTransient<
                IEntityCache<TEntityCacheItem, TKey>,
                EntityCacheWithObjectMapper<TEntity, TEntityCacheItem, TKey>
            >();
        services
            .TryAddTransient<EntityCacheWithObjectMapper<TEntity, TEntityCacheItem, TKey>>();

        services.Configure<EqnDistributedCacheOptions>(options =>
        {
            options.ConfigureCache<TEntityCacheItem>(cacheOptions ?? GetDefaultCacheOptions());
        });

        return services;
    }

    public static IServiceCollection AddEntityCache<TObjectMapperContext, TEntity, TEntityCacheItem, TKey>(
        this IServiceCollection services,
        [CanBeNull] DistributedCacheEntryOptions cacheOptions = null)
        where TEntity : Entity<TKey>
        where TEntityCacheItem : class
    {
        services
            .TryAddTransient<
                IEntityCache<TEntityCacheItem, TKey>,
                EntityCacheWithObjectMapperContext<TObjectMapperContext, TEntity, TEntityCacheItem, TKey>
            >();
        services.TryAddTransient<EntityCacheWithObjectMapperContext<TObjectMapperContext, TEntity, TEntityCacheItem, TKey>>();

        services.Configure<EqnDistributedCacheOptions>(options =>
        {
            options.ConfigureCache<TEntityCacheItem>(cacheOptions ?? GetDefaultCacheOptions());
        });

        return services;
    }

    private static DistributedCacheEntryOptions GetDefaultCacheOptions()
    {
        return new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        };
    }
}
