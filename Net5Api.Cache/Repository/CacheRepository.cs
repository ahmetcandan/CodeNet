using Net5Api.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Cache.Repository
{
    public class CacheRepository : BaseMongoRepository<CacheModel>, ICacheRepository
    {
        public CacheRepository() : base("Cache")
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
