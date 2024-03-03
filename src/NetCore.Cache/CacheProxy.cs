using NetCore.Abstraction;
using System;
using System.Linq;
using System.Reflection;

namespace NetCore.Cache
{
    public class CacheProxy<TDecorated> : DispatchProxy
    {
        private TDecorated _decorated;
        private ICacheRepository _cacheRepository;

        public CacheProxy()
        {

        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var aspect = (ICacheAttribute)targetMethod.GetCustomAttributes(typeof(ICacheAttribute), true).FirstOrDefault();

            if (aspect == null)
                return targetMethod.Invoke(_decorated, args);

            var cacheResponse = aspect?.OnBefore(targetMethod, args, _cacheRepository);

            object result;
            if (cacheResponse == null || cacheResponse.GetType() == targetMethod.ReturnType)
            {
                result = targetMethod.Invoke(_decorated, args);
                aspect?.OnAfter(targetMethod, args, result, _cacheRepository);
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
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            _cacheRepository = cacheRepository ?? throw new ArgumentNullException(nameof(cacheRepository));
        }
    }
}
