using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace NetCore.Redis
{
    public class RedisCacheRepository : ICacheRepository
    {
        IRedisClient redisClient;

        public RedisCacheRepository(IOptions<RedisSettings> config)
        {
            redisClient = new RedisClient(new RedisEndpoint { Host = config.Value.Host, Port = config.Value.Port, Password = config.Value.Password, RetryTimeout = config.Value.RetryTimeout });
        }

        /// <summary>
        /// Get Cache value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            var result = redisClient.Get<CacheModel>(key);
            if (result == null)
                return null;
            if (result.ExpiryDate < DateTime.Now)
            {
                Task.Run(() => { redisClient.Delete(result); });
                return null;
            }
            return result.Value;
        }

        /// <summary>
        /// Set Cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time">seconds</param>
        public void SetCache(string key, object value, int time)
        {
            redisClient.Set(key, new CacheModel(key, value, time), DateTime.Now.AddSeconds(time));
        }
    }
}
