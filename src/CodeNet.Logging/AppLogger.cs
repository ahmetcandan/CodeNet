using CodeNet.Core;
using CodeNet.Core.Extensions;
using CodeNet.Logging.Enum;
using CodeNet.Logging.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Reflection;

namespace CodeNet.Logging;

internal class AppLogger(IIdentityContext IdentityContext, ILogger Logger) : IAppLogger
{
    public void EntryLog(object request, MethodBase methodBase) => PostLogData(LogTime.Entry, methodBase, request);

    public void ExceptionLog(Exception exception, MethodBase methodBase) => PostLogData(LogTime.Error, methodBase, exception, exception: exception);

    public void ExceptionLog(Exception exception, object data, MethodBase methodBase) => PostLogData(LogTime.Error, methodBase, new { Exception = exception, Data = data }, exception: exception);

    public void ExitLog(object response, MethodBase methodBase) => PostLogData(LogTime.Exit, methodBase, response);

    public void ExitLog(object response, MethodBase methodBase, long time) => PostLogData(LogTime.Exit, methodBase, response, elapsedDuration: time);

    public void TraceLog(object data, MethodBase methodBase) => PostLogData(LogTime.Trace, methodBase, data);

    protected virtual string GetObjectToString(object obj) => JsonConvert.SerializeObject(obj);

    private void PostLogData(LogTime logTime, MethodBase methodBase, object data, long? elapsedDuration = null, Exception? exception = null) => PostLogData(logTime, methodBase, GetObjectToString(data), elapsedDuration, exception: exception);

    private void PostLogData(LogTime logTime, MethodBase methodBase, string data, long? elapsedDuration = null, Exception? exception = null)
    {
        var _methodBase = methodBase.GetMethodBase();
        PostLogData(logTime, _methodBase!.DeclaringType!.Assembly.GetName().Name, _methodBase.DeclaringType.Name, _methodBase.Name, data, elapsedDuration: elapsedDuration, exception: exception);
    }

    private void PostLogData(LogTime logTime, string? assemblyName, string className, string methodName, string data, long? elapsedDuration = null, Exception? exception = null)
    {
        var eventId = new EventId(IdentityContext.RequestId.GetHashCode(), $"{assemblyName}_{className}_{methodName}");
        string message = JsonConvert.SerializeObject(new LogModel
        {
            AssemblyName = assemblyName ?? "unknow",
            ClassName = className,
            MethodName = methodName,
            LogTime = logTime.ToString(),
            Username = IdentityContext.UserName,
            Data = data,
            RequestId = IdentityContext.RequestId,
            ElapsedDuration = elapsedDuration
        }) ?? "";
        switch (logTime)
        {
            case LogTime.Entry:
                Logger.Log(LogLevel.Information, message);
                break;
            case LogTime.Exit:
                Logger.Log(LogLevel.Information, message);
                break;
            case LogTime.Trace:
                Logger.Log(LogLevel.Trace, message);
                break;
            case LogTime.Error:
                Logger.Log(LogLevel.Error, exception: exception ,message);
                break;
            default:
                Logger.Log(LogLevel.None, message);
                break;
        }
    }
}
