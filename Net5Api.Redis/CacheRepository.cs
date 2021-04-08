using Net5Api.Abstraction;
using Net5Api.Abstraction.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Net5Api.Redis
{
    public class CacheRepository : ICacheRepository
    {
        IRedisClient redisClient;

        public CacheRepository(IOptions<RedisEndpoint> config)
        {
            redisClient = new RedisClient(config.Value);
        }

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

        public void SetCache(string key, object value, int time)
        {
            redisClient.Set(key, new CacheModel(key, value, time));
        }
    }
}
