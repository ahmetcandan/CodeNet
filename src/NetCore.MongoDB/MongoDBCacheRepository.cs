using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using System;
using System.Threading.Tasks;

namespace NetCore.MongoDB
{
    public class MongoDBCacheRepository : BaseMongoRepository<CacheModel>, ICacheRepository

    {
        public MongoDBCacheRepository(IOptions<MongoDbSettings> config) : base(config.Value.ConnectionString, config.Value.DatabaseName, "Cache")
        {

        }

        public object GetCache(string key)
        {
            var result = GetById(key);
            if (result == null)
                return null;
            if (result.ExpiryDate < DateTime.Now)
            {
                Task.Run(() => { Delete(result); });
                return null;
            }
            return result.Value;
        }

        public void SetCache(string key, object value, int time)
        {
            var model = new CacheModel(key, value, time);
            if (ContainsId(key))
                Task.Run(() => { Update(key, model); });
            else
                Task.Run(() => { Create(model); });
        }
    }
}
