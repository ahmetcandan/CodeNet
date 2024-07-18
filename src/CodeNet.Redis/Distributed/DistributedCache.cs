using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CodeNet.Redis;

internal class DistributedCache<TModel>(IDistributedCache distributedCache) : IDistributedCache<TModel>
    where TModel : class
{
    /// <summary>
    /// Get Value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<TModel?> GetValueAsync(string key)
    {
        return GetValueAsync(key, CancellationToken.None);
    }

    /// <summary>
    /// Get Value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TModel?> GetValueAsync(string key, CancellationToken cancellationToken)
    {
        var json = await distributedCache.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrEmpty(json))
            return default;
        return JsonConvert.DeserializeObject<TModel>(json);
    }

    /// <summary>
    /// Get Value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TModel? GetValue(string key)
    {
        var json = distributedCache.GetString(key);
        if (string.IsNullOrEmpty(json))
            return default;
        return JsonConvert.DeserializeObject<TModel>(json);
    }

    /// <summary>
    /// Set Value
    /// </summary>
    /// <param name="model"></param>
    /// <param name="key"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public Task SetValueAsync(TModel model, string key, int time)
    {
        return SetValueAsync(model, key, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(time) }, CancellationToken.None);
    }

    /// <summary>
    /// Set Value
    /// </summary>
    /// <param name="model"></param>
    /// <param name="key"></param>
    /// <param name="time">Minutes</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task SetValueAsync(TModel model, string key, int time, CancellationToken cancellationToken)
    {
        return SetValueAsync(model, key, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(time) }, cancellationToken);
    }

    /// <summary>
    /// Set Value
    /// </summary>
    /// <param name="model"></param>
    /// <param name="key"></param>
    /// <param name="distributedCacheEntryOptions"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SetValueAsync(TModel model, string key, DistributedCacheEntryOptions distributedCacheEntryOptions)
    {
        await SetValueAsync(model, key, distributedCacheEntryOptions, CancellationToken.None);
    }

    /// <summary>
    /// Set Value
    /// </summary>
    /// <param name="model"></param>
    /// <param name="key"></param>
    /// <param name="distributedCacheEntryOptions"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SetValueAsync(TModel model, string key, DistributedCacheEntryOptions distributedCacheEntryOptions, CancellationToken cancellationToken)
    {
        await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(model), distributedCacheEntryOptions, cancellationToken);
    }

    /// <summary>
    /// Set Value
    /// </summary>
    /// <param name="model"></param>
    /// <param name="key"></param>
    /// <param name="time"></param>
    public void SetValue(TModel model, string key, int time)
    {
        SetValue(model, key, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(time) });
    }

    /// <summary>
    /// Set Value
    /// </summary>
    /// <param name="model"></param>
    /// <param name="key"></param>
    /// <param name="distributedCacheEntryOptions"></param>
    public void SetValue(TModel model, string key, DistributedCacheEntryOptions distributedCacheEntryOptions)
    {
        distributedCache.SetString(key, JsonConvert.SerializeObject(model), distributedCacheEntryOptions);
    }

    /// <summary>
    /// Remove by Key
    /// </summary>
    /// <param name="key"></param>
    public void Remove(string key)
    {
        distributedCache.Remove(key);
    }

    /// <summary>
    /// Remove by Key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task RemoveAsync(string key)
    {
        return RemoveAsync(key, CancellationToken.None);
    }

    /// <summary>
    /// Remove by Key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return distributedCache.RemoveAsync(key, cancellationToken);
    }
}
