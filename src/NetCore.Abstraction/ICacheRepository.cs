using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Abstraction;

public interface ICacheRepository
{
    Task<TModel> GetCacheAsync<TModel>(string key);
    Task<TModel> GetCacheAsync<TModel>(string key, CancellationToken cancellationToken);

    Task SetCacheAsync(string key, object value, int time);
    Task SetCacheAsync(string key, object value, int time, CancellationToken cancellationToken);
    Task SetCacheAsync(string key, string jsonValue, int time);
    Task SetCacheAsync(string key, string jsonValue, int time, CancellationToken cancellationToken);
}
