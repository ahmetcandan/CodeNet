using Net5Api.Abstraction;
using Net5Api.Abstraction.Enum;
using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace Net5Api.ExceptionHandling
{
    public class ExceptionAttribute : Attribute, IExceptionAttribute
    {
        public ExceptionAttribute()
        {

        }

        public void OnException(MethodInfo targetMethod, object[] args, ILogRepository logRepository, Exception ex, ClaimsPrincipal user)
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
                LogType = LogType.Error,
                Message = $"{{ Message: {ex.Message}, StackTrace: {ex.StackTrace}, InnerExceptionMessage: {(ex.InnerException != null ? ex.InnerException.Message : "")} }}"
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
    }

    public interface IExceptionAttribute
    {
        void OnException(MethodInfo targetMethod, object[] args, ILogRepository logRepository, Exception ex, ClaimsPrincipal user);
    }
}
