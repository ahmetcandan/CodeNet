using Net5Api.Abstraction;
using System;
using System.Linq;
using System.Reflection;

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
            cacheRepository.SetCache(getKeyString(targetMethod, args), value, Time);
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
