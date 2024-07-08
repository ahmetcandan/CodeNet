using Microsoft.Extensions.Caching.Distributed;

namespace CodeNet.Redis;

public interface IDistributedCache<TModel>
    where TModel : class
{
    Task<TModel?> GetValueAsync(string key);
    Task<TModel?> GetValueAsync(string key, CancellationToken cancellationToken);
    TModel? GetValue(string key);
    Task SetValueAsync(TModel model, string key, int time);
    Task SetValueAsync(TModel model, string key, int time, CancellationToken cancellationToken);
    Task SetValueAsync(TModel model, string key, DistributedCacheEntryOptions distributedCacheEntryOptions);
    Task SetValueAsync(TModel model, string key, DistributedCacheEntryOptions distributedCacheEntryOptions, CancellationToken cancellationToken);
}
