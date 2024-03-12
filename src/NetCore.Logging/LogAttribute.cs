using NetCore.Abstraction;
using NetCore.Abstraction.Enum;
using NetCore.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetCore.Logging;

public class LogAttribute(LogTime logType) : Attribute, ILogAttribute
{
    public LogTime LogTime { get; set; } = logType;

    public void OnBefore(MethodInfo targetMethod, object[] args, ILogRepository logRepository, string username)
    {
        var model = new LogModel()
        {
            MethodParameters = getMethodParameters(targetMethod.GetParameters(), args),
            MethodName = targetMethod.Name,
            Namespace = targetMethod.DeclaringType.Namespace,
            ClassName = targetMethod.DeclaringType.Name,
            UserName = username,
            LogTime = LogTime.Before,
            LogType = LogType.Info,
        };

        logRepository.Insert(model);
    }
    public void OnException(MethodInfo targetMethod, object[] args, ILogRepository logRepository, string username, Exception ex)
    {
        var model = new LogModel()
        {
            MethodParameters = getMethodParameters(targetMethod.GetParameters(), args),
            MethodName = targetMethod.Name,
            Namespace = targetMethod.DeclaringType.Namespace,
            ClassName = targetMethod.DeclaringType.Name,
            UserName = username,
            Message = $"{{ Message: {ex.Message}, StackTrace: {ex.StackTrace}, InnerExceptionMessage: {(ex.InnerException != null ? ex.InnerException.Message : "")} }}",
            LogTime = LogTime.Exception,
            LogType = LogType.Error
        };

        logRepository.Insert(model);
    }

    public void OnAfter(MethodInfo targetMethod, object[] args, object value, ILogRepository logRepository, string username)
    {
        var model = new LogModel()
        {
            MethodParameters = getMethodParameters(targetMethod.GetParameters(), args),
            MethodName = targetMethod.Name,
            Namespace = targetMethod.DeclaringType.Namespace,
            ClassName = targetMethod.DeclaringType.Name,
            UserName = username,
            LogTime = LogTime.After,
            LogType = LogType.Info
        };

        logRepository.Insert(model);
    }

    private static IEnumerable<MethodParameter> getMethodParameters(ParameterInfo[] parameters, object[] args)
    {
        return from p in parameters
               select new MethodParameter
               {
                   Name = p.Name,
                   Value = args[p.Position]
               };
    }

    public LogTime GetLogTime()
    {
        return LogTime;
    }
}

public interface ILogAttribute
{
    void OnBefore(MethodInfo targetMethod, object[] args, ILogRepository logRepository, string username);
    void OnException(MethodInfo targetMethod, object[] args, ILogRepository logRepository, string username, Exception ex);
    void OnAfter(MethodInfo targetMethod, object[] args, object value, ILogRepository logRepository, string username);
    LogTime GetLogTime();
}
