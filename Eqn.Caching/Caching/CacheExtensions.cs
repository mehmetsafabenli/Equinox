using Microsoft.Extensions.Caching.Distributed;

namespace Eqn.Caching.Caching;

public static class CacheExtensions
{
    public static DistributedCacheEntryOptions GetDefaultCacheOptions =>
        new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(5)
        };
}