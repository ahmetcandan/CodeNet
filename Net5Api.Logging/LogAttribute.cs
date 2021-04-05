using Net5Api.Abstraction;
using Net5Api.Abstraction.Enum;
using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

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
        public void OnException(MethodInfo targetMethod, object[] args, ILogRepository logRepository, ClaimsPrincipal user, Exception ex)
        {
            string userName = user != null ? user.Identity.Name : "anonymous";
            var model = new LogModel()
            {
                MethodParameters = getMethodParameters(targetMethod.GetParameters(), args),
                MethodName = targetMethod.Name,
                Namespace = targetMethod.DeclaringType.Namespace,
                ClassName = targetMethod.DeclaringType.Name,
                UserName = userName,
                Message = $"{{ Message: {ex.Message}, StackTrace: {ex.StackTrace}, InnerExceptionMessage: {(ex.InnerException != null ? ex.InnerException.Message : "")} }}",
                LogTime = LogTime.Exception,
                LogType = LogType.Error
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
        void OnException(MethodInfo targetMethod, object[] args, ILogRepository logRepository, ClaimsPrincipal user, Exception ex);
        void OnAfter(MethodInfo targetMethod, object[] args, object value, ILogRepository logRepository, ClaimsPrincipal user);
        LogTime GetLogTime();
    }
}
