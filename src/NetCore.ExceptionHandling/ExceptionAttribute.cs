using NetCore.Abstraction;
using NetCore.Abstraction.Enum;
using NetCore.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetCore.ExceptionHandling
{
    public class ExceptionAttribute : Attribute, IExceptionAttribute
    {
        private bool throwException;
        public ExceptionAttribute(bool throwException = true)
        {
            this.throwException = throwException;
        }

        public void OnException(MethodInfo targetMethod, object[] args, ILogRepository logRepository, Exception ex, string username)
        {
            var model = new LogModel()
            {
                MethodParameters = getMethodParameters(targetMethod.GetParameters(), args),
                MethodName = targetMethod.Name,
                Namespace = targetMethod.DeclaringType.Namespace,
                ClassName = targetMethod.DeclaringType.Name,
                UserName = username,
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

        public bool GetThrowException()
        {
            return throwException;
        }
    }

    public interface IExceptionAttribute
    {
        void OnException(MethodInfo targetMethod, object[] args, ILogRepository logRepository, Exception ex, string username);

        bool GetThrowException();
    }
}
