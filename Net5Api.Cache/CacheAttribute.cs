using Net5Api.Cache.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Cache
{
    public class CacheAttribute : Attribute, ICacheAttribute
    {
        public int Time { get; set; }

        public CacheAttribute(int time = 3600)
        {
            Time = time;
        }

        public object OnBefore(MethodInfo targetMethod, object[] args, ICacheRepository cacheRepository)
        {
            return cacheRepository.GetCache(getKeyString(targetMethod, args));
        }

        public void OnAfter(MethodInfo targetMethod, object[] args, object value, ICacheRepository cacheRepository)
        {
            cacheRepository.SetCache(getKeyString(targetMethod, args), value, DateTime.Now.AddSeconds(Time));
        }

        string getKeyString(MethodInfo targetMethod, object[] args)
        {
            return $"{targetMethod.Name}_{string.Join("-", targetMethod.GetParameters().Select(c => $"{c.Name}:{args[c.Position]}"))}";
        }
    }

    public interface ICacheAttribute
    {
        object OnBefore(MethodInfo targetMethod, object[] args, ICacheRepository cacheRepository);
        void OnAfter(MethodInfo targetMethod, object[] args, object value, ICacheRepository cacheRepository);
    }
}
