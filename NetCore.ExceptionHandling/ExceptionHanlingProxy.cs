using NetCore.Abstraction;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Principal;

namespace NetCore.ExceptionHandling
{
    public class ExceptionHanlingProxy<TDecorated> : DispatchProxy
    {
        private TDecorated decorated;
        private ILogRepository _logRepository;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var aspect = (IExceptionAttribute)targetMethod.GetCustomAttributes(typeof(IExceptionAttribute), true).FirstOrDefault();

            if (aspect == null)
                return targetMethod.Invoke(decorated, args);


            IPrincipal user = null;
            if (decorated.GetType().GetInterfaces().Any(c => c == typeof(IService)))
                user = (decorated as IService).GetUser();

            object result = null;

            try
            {
                result = targetMethod.Invoke(decorated, args);
            }
            catch (UserLevelException ex)
            {
                aspect?.OnException(targetMethod, args, _logRepository, ex, user);
                if (aspect.GetThrowException())
                    throw;
            }
            catch (Exception ex)
            {
                aspect?.OnException(targetMethod, args, _logRepository, ex, user);
                if (aspect.GetThrowException())
                    throw new Exception(ex.Message);
            }

            return result;
        }

        public static TDecorated Create(TDecorated decorated, ILogRepository logRepository)
        {
            object proxy = Create<TDecorated, ExceptionHanlingProxy<TDecorated>>();
            ((ExceptionHanlingProxy<TDecorated>)proxy).SetParameters(decorated, logRepository);
            return (TDecorated)proxy;
        }

        private void SetParameters(TDecorated decorated, ILogRepository logRepository)
        {
            this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
        }
    }
}
