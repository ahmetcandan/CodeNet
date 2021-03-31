using Net5Api.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Cache.Repository
{
    public interface ICacheRepository
    {
        public object GetCache(string key);

        public void SetCache(string key, object value, DateTime expiryDate);

        public bool ContainsKey(string key);
    }
}
