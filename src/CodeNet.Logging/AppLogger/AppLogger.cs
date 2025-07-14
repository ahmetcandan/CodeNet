using CodeNet.Core.Context;
using CodeNet.Core.Extensions;
using CodeNet.Logging.Enum;
using CodeNet.Logging.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Reflection;

namespace CodeNet.Logging;

public class AppLogger(ICodeNetContext codeNetContext, ILogger<AppLogger> logger) : IAppLogger
{
    public void EntryLog(object request, MethodBase? methodBase) => PostLogData(LogTime.Entry, methodBase, request);

    public void ExceptionLog(Exception exception, MethodBase? methodBase) => PostLogData(LogTime.Error, methodBase, exception, exception: exception);

    public void ExceptionLog(Exception exception, object data, MethodBase? methodBase) => PostLogData(LogTime.Error, methodBase, new { Exception = exception, Data = data }, exception: exception);

    public void ExitLog(object response, MethodBase? methodBase) => PostLogData(LogTime.Exit, methodBase, response);

    public void ExitLog(object response, MethodBase? methodBase, long time) => PostLogData(LogTime.Exit, methodBase, response, elapsedDuration: time);

    public void TraceLog(object data, MethodBase? methodBase) => PostLogData(LogTime.Trace, methodBase, data);

    protected virtual string GetObjectToString(object obj) => obj.GetType() == typeof(string) ? obj?.ToString() ?? string.Empty : JsonConvert.SerializeObject(obj);

    private void PostLogData(LogTime logTime, MethodBase? methodBase, object data, long? elapsedDuration = null, Exception? exception = null)
    {
        var isJsonData = data.GetType() != typeof(string);
        PostLogData(logTime, methodBase, isJsonData ? JsonConvert.SerializeObject(data) : data?.ToString() ?? string.Empty, isJsonData, elapsedDuration, exception: exception);
    }

    private void PostLogData(LogTime logTime, MethodBase? methodBase, string data, bool isJsonData, long? elapsedDuration = null, Exception? exception = null)
    {
        var _methodBase = methodBase?.GetMethodBase();
        PostLogData(logTime, _methodBase?.DeclaringType?.Assembly.GetName().Name, _methodBase?.DeclaringType?.Name ?? string.Empty, _methodBase?.Name ?? string.Empty, data, isJsonData, elapsedDuration: elapsedDuration, exception: exception);
    }

    private void PostLogData(LogTime logTime, string? assemblyName, string className, string methodName, string data, bool isJsonData, long? elapsedDuration = null, Exception? exception = null)
    {
        var eventId = new EventId(codeNetContext.CorrelationId.GetHashCode(), $"{assemblyName}_{className}_{methodName}");
        string message = new LogModel(isJsonData)
        {
            AssemblyName = assemblyName ?? "unknow",
            ClassName = className,
            MethodName = methodName,
            LogTime = logTime.ToString(),
            Username = codeNetContext.UserName,
            Data = data,
            CorrelationId = codeNetContext.CorrelationId,
            ElapsedDuration = elapsedDuration
        }.ToString();
        switch (logTime)
        {
            case LogTime.Entry:
                logger.Log(LogLevel.Information, eventId, message);
                break;
            case LogTime.Exit:
                logger.Log(LogLevel.Information, eventId, message);
                break;
            case LogTime.Trace:
                logger.Log(LogLevel.Trace, eventId, message);
                break;
            case LogTime.Error:
                logger.Log(LogLevel.Error, eventId, exception, message);
                break;
            default:
                logger.Log(LogLevel.None, eventId, message);
                break;
        }
    }
}
