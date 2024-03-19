using NetCore.Abstraction;
using NetCore.Abstraction.Enum;
using NetCore.Abstraction.Extension;
using NetCore.Abstraction.Model;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace NetCore.Logging;

public class AppLogger(IQService QService, IIdentityContext IdentityContext) : IAppLogger
{
    private const string CHANNEL_NAME = "log";

    public void EntryLog(object request, MethodBase methodBase)
    {
        PostLogData(LogTime.Entry, methodBase, request);
    }

    public void ExceptionLog(Exception exception, MethodBase methodBase)
    {
        PostLogData(LogTime.Exception, methodBase, exception);
    }

    public void ExceptionLog(Exception exception, object data, MethodBase methodBase)
    {
        PostLogData(LogTime.Exception, methodBase, new { Exception = exception, Data = data });
    }

    public void ExitLog(object response, MethodBase methodBase)
    {
        PostLogData(LogTime.Exit, methodBase, response);
    }

    public void ExitLog(object response, MethodBase methodBase, long time)
    {
        PostLogData(LogTime.Exit, methodBase, response, time: time);
    }

    public void TraceLog(object data, MethodBase methodBase)
    {
        PostLogData(LogTime.Trace, methodBase, data);
    }

    protected virtual string GetObjectToString(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    private void PostLogData(LogTime logTime, MethodBase methodBase, object data, long time = 0)
    {
        PostLogData(logTime, methodBase, GetObjectToString(data), time);
    }

    private void PostLogData(LogTime logTime, MethodBase methodBase, string data, long time = 0)
    {
        var _methodBase = methodBase.GetMethodBase();
        PostLogData(logTime, _methodBase.DeclaringType.Assembly.GetName().Name, _methodBase.DeclaringType.Name, _methodBase.Name, data, time);
    }

    private void PostLogData(LogTime logTime, string _namespace, string className, string methodName, string data, long time = 0)
    {
        QService.Post(CHANNEL_NAME, new LogModel
        {
            Namespace = _namespace,
            ClassName = className,
            MethodName = methodName,
            LogTime = logTime,
            Username = IdentityContext.UserName,
            Data = data,
            RequestId = IdentityContext.RequestId,
            Time = time
        });
    }
}
