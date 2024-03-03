using NetCore.Abstraction;
using System;

namespace NetCore.Cache
{
    public class CacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public object GetCache(string key)
        {
            return _cacheRepository.GetCache(key);
        }

        public T GetCache<T>(string key)
        {
            return (T)_cacheRepository.GetCache(key);
        }

        public void SetCache(string key, object value, int time)
        {
            _cacheRepository.SetCache(key, value, time);
        }

        public void SetCache(string key, object value, TimeSpan timeSpan)
        {
            SetCache(key, value, (int)timeSpan.TotalSeconds);
        }
    }
}
