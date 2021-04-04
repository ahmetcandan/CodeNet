using Net5Api.Cache.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Net5Api.Cache.Proxy
{
    public class CacheProxy<TDecorated> : DispatchProxy
    {
        private TDecorated decorated;
        private ICacheRepository _cacheRepository;

        public CacheProxy()
        {

        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var aspect = targetMethod.GetCustomAttributes(typeof(ICacheAttribute), true).FirstOrDefault();

            if (aspect == null)
                return targetMethod.Invoke(decorated, args);

            var cacheResponse = ((ICacheAttribute)aspect)?.OnBefore(targetMethod, args, _cacheRepository);

            object result;
            if (cacheResponse == null)
            {
                result = targetMethod.Invoke(decorated, args);
                (aspect as ICacheAttribute)?.OnAfter(targetMethod, args, result, _cacheRepository);
            }
            else
                result = cacheResponse;

            return result;
        }

        public static TDecorated Create(TDecorated decorated, ICacheRepository cacheRepository)
        {
            object proxy = Create<TDecorated, CacheProxy<TDecorated>>();
            ((CacheProxy<TDecorated>)proxy).SetParameters(decorated, cacheRepository);
            return (TDecorated)proxy;
        }

        private void SetParameters(TDecorated decorated, ICacheRepository cacheRepository)
        {
            this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            _cacheRepository = cacheRepository ?? throw new ArgumentNullException(nameof(cacheRepository));
        }
    }
}
