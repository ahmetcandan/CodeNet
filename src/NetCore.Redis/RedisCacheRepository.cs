using Microsoft.Extensions.Caching.Distributed;
using NetCore.Abstraction;
using Newtonsoft.Json;

namespace NetCore.Redis;

public class RedisCacheRepository(IDistributedCache DistributedCache) : ICacheRepository
{
    /// <summary>
    /// Get Cache value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<TModel> GetCacheAsync<TModel>(string key, CancellationToken cancellationToken)
    {
        var json = await DistributedCache.GetStringAsync(key, cancellationToken);
        return JsonConvert.DeserializeObject<TModel>(json);
    }

    /// <summary>
    /// Get Cache value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<TModel> GetCacheAsync<TModel>(string key)
    {
        return await GetCacheAsync<TModel>(key, CancellationToken.None);
    }

    /// <summary>
    /// Set Cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="time">seconds</param>
    public async Task SetCacheAsync(string key, string jsonValue, int time, CancellationToken cancellationToken)
    {
        await DistributedCache.SetStringAsync(key, jsonValue, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(time) }, cancellationToken);
    }

    /// <summary>
    /// Set Cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="time">seconds</param>
    public async Task SetCacheAsync(string key, string jsonValue, int time)
    {
        await SetCacheAsync(key, jsonValue, time, CancellationToken.None);
    }

    /// <summary>
    /// Set Cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="time">seconds</param>
    public async Task SetCacheAsync(string key, object value, int time, CancellationToken cancellationToken)
    {
        await SetCacheAsync(key, JsonConvert.SerializeObject(value), time, cancellationToken);
    }

    /// <summary>
    /// Set Cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="time">seconds</param>
    public async Task SetCacheAsync(string key, object value, int time) 
    {
        await SetCacheAsync(key, value, time, CancellationToken.None);
    }
}
