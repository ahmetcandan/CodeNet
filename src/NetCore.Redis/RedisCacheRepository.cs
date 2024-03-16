using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using ServiceStack.Redis;
using System;

namespace NetCore.Redis;

public class RedisCacheRepository : ICacheRepository
{
    private readonly IRedisClient _redisClient;

    public RedisCacheRepository(IOptions<RedisSettings> config)
    {
        _redisClient = new RedisClient(new RedisEndpoint { Host = config.Value.Host, Port = config.Value.Port, Password = config.Value.Password, RetryTimeout = config.Value.RetryTimeout });
    }

    /// <summary>
    /// Get Cache value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TModel GetCache<TModel>(string key) => _redisClient.Get<TModel>(key);

    /// <summary>
    /// Set Cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="time">seconds</param>
    public void SetCache(string key, object value, int time) => _redisClient.Set(key, value, DateTime.Now.AddMinutes(time));
}
