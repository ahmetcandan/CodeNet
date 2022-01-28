using NetCore.Abstraction;
using NetCore.Abstraction.Enum;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Principal;

namespace NetCore.Logging
{
    public class LogProxy<TDecorated> : DispatchProxy
    {
        private TDecorated decorated;
        private ILogRepository _logRepository;

        public LogProxy()
        {

        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var aspect = (ILogAttribute)targetMethod.GetCustomAttributes(typeof(ILogAttribute), true).FirstOrDefault();

            if (aspect == null)
                return targetMethod.Invoke(decorated, args);


            IPrincipal user = null;

            if (decorated.GetType().GetInterfaces().Any(c => c.Name == "IService"))
                user = (decorated as IService).GetUser();

            if (aspect.GetLogTime() == LogTime.Before || aspect.GetLogTime() == LogTime.BeforeAndAfter)
                ((ILogAttribute)aspect)?.OnBefore(targetMethod, args, _logRepository, user);

            object result = null;
            try
            {
                result = targetMethod.Invoke(decorated, args);
            }
            catch (Exception ex)
            {
                if (aspect.GetLogTime() == LogTime.Exception)
                    ((ILogAttribute)aspect)?.OnException(targetMethod, args, _logRepository, user, ex);
                throw;
            }

            if (aspect.GetLogTime() == LogTime.After || aspect.GetLogTime() == LogTime.BeforeAndAfter)
                (aspect as ILogAttribute)?.OnAfter(targetMethod, args, result, _logRepository, user);

            return result;
        }

        public static TDecorated Create(TDecorated decorated, ILogRepository logRepository)
        {
            object proxy = Create<TDecorated, LogProxy<TDecorated>>();
            ((LogProxy<TDecorated>)proxy).SetParameters(decorated, logRepository);
            return (TDecorated)proxy;
        }

        private void SetParameters(TDecorated decorated, ILogRepository logRepository)
        {
            this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
        }
    }
}
