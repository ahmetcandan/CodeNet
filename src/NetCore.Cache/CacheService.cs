using NetCore.Abstraction;
using System;

namespace NetCore.Cache;

public class CacheService(ICacheRepository cacheRepository)
{
    public object GetCache(string key)
    {
        return cacheRepository.GetCache(key);
    }

    public T GetCache<T>(string key)
    {
        return (T)cacheRepository.GetCache(key);
    }

    public void SetCache(string key, object value, int time)
    {
        cacheRepository.SetCache(key, value, time);
    }

    public void SetCache(string key, object value, TimeSpan timeSpan)
    {
        SetCache(key, value, (int)timeSpan.TotalSeconds);
    }
}
