using NetCore.Abstraction;
using NetCore.Abstraction.Enum;
using NetCore.Abstraction.Extension;
using NetCore.Abstraction.Model;
using Newtonsoft.Json;
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
        PostLogData(LogTime.Error, methodBase, exception);
    }

    public void ExceptionLog(Exception exception, object data, MethodBase methodBase)
    {
        PostLogData(LogTime.Error, methodBase, new { Exception = exception, Data = data });
    }

    public void ExitLog(object response, MethodBase methodBase)
    {
        PostLogData(LogTime.Exit, methodBase, response);
    }

    public void ExitLog(object response, MethodBase methodBase, long time)
    {
        PostLogData(LogTime.Exit, methodBase, response, elapsedDuration: time);
    }

    public void TraceLog(object data, MethodBase methodBase)
    {
        PostLogData(LogTime.Trace, methodBase, data);
    }

    protected virtual string GetObjectToString(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    private void PostLogData(LogTime logTime, MethodBase methodBase, object data, long? elapsedDuration = null)
    {
        PostLogData(logTime, methodBase, GetObjectToString(data), elapsedDuration);
    }

    private void PostLogData(LogTime logTime, MethodBase methodBase, string data, long? elapsedDuration = null)
    {
        var _methodBase = methodBase.GetMethodBase();
        PostLogData(logTime, _methodBase!.DeclaringType!.Assembly.GetName().Name, _methodBase.DeclaringType.Name, _methodBase.Name, data, elapsedDuration);
    }

    private void PostLogData(LogTime logTime, string? assemblyName, string className, string methodName, string data, long? elapsedDuration = null)
    {
        QService.Post(CHANNEL_NAME, new LogModel
        {
            AssemblyName = assemblyName ?? "unknow",
            ClassName = className,
            MethodName = methodName,
            LogTime = nameof(logTime),
            Username = IdentityContext.UserName,
            Data = data,
            RequestId = IdentityContext.RequestId,
            ElapsedDuration = elapsedDuration
        });
    }
}
