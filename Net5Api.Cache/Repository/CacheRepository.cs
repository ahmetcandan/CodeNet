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
            if (ContainsKey(key))
            {
                var result = GetById(key);
                if (result == null)
                    return null;
                if (result.ExpiryDate < DateTime.Now)
                {
                    Delete(result);
                    return null;
                }
                return result.Value;
            }
            return null;
        }

        public void SetCache(string key, object value, int time)
        {
            var model = new CacheModel(key, value, time);
            if (ContainsKey(key))
                Update(key, model);
            else
                Create(model);
        }

        public bool ContainsKey(string key)
        {
            return ContainsId(key);
        }
    }
}
