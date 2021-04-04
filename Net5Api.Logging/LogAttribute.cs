using Net5Api.Logging.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Logging
{
    public class LogAttribute : Attribute, ILogAttribute
    {
        public LogTime LogTime { get; set; }
        public LogAttribute(LogTime logType)
        {
            LogTime = logType;
        }

        public void OnBefore(MethodInfo targetMethod, object[] args, ILogRepository logRepository, ClaimsPrincipal user)
        {
            string userName = user != null ? user.Identity.Name : "anonymous";
            var model = new LogModel()
            {
                MethodParameters = getMethodParameters(targetMethod.GetParameters(), args),
                MethodName = targetMethod.Name,
                Namespace = targetMethod.DeclaringType.Namespace,
                ClassName = targetMethod.DeclaringType.Name,
                UserName = userName,
                LogTime = LogTime.Before,
                LogType = LogType.Info
            };
            
            logRepository.Insert(model);
        }

        public void OnAfter(MethodInfo targetMethod, object[] args, object value, ILogRepository logRepository, ClaimsPrincipal user)
        {
            string userName = user != null ? user.Identity.Name : "anonymous";
            var model = new LogModel()
            {
                MethodParameters = getMethodParameters(targetMethod.GetParameters(), args),
                MethodName = targetMethod.Name,
                Namespace = targetMethod.DeclaringType.Namespace,
                ClassName = targetMethod.DeclaringType.Name,
                UserName = userName,
                LogTime = LogTime.After,
                LogType = LogType.Info
            };

            logRepository.Insert(model);
        }

        IEnumerable<MethodParameter> getMethodParameters(ParameterInfo[] parameters, object[] args)
        {
            return (from p in parameters
                    select new MethodParameter
                    {
                        Name = p.Name,
                        Value = args[p.Position]
                    });
        }

        public LogTime GetLogTime()
        {
            return LogTime;
        }
    }

    public interface ILogAttribute
    {
        void OnBefore(MethodInfo targetMethod, object[] args, ILogRepository logRepository, ClaimsPrincipal user);
        void OnAfter(MethodInfo targetMethod, object[] args, object value, ILogRepository logRepository, ClaimsPrincipal user);
        LogTime GetLogTime();
    }
}
