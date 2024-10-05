using Microsoft.Extensions.Caching.Distributed;

namespace SocketServer.Services;

public class CachingService(IDistributedCache cache)
{
    private readonly DistributedCacheEntryOptions _options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
        SlidingExpiration = TimeSpan.FromSeconds(1200),
    };
    public async Task<string?> GetAsync(string key)
    {
        return await cache.GetStringAsync(key);
    }
    public async Task SetAsync(string key, string value)
    {
        await cache.SetStringAsync(key, value, _options);
    }
    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}