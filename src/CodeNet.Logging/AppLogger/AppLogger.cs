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
        logger.Log(
            LogTimeToLevel(logTime),
            new EventId(codeNetContext.CorrelationId.GetHashCode(), $"{assemblyName}_{className}_{methodName}"),
            exception,
            LogModel.ToJson(isJsonData, data, elapsedDuration, codeNetContext.CorrelationId, assemblyName, className, methodName, logTime.ToString(), codeNetContext.UserName));
    }

    private static LogLevel LogTimeToLevel(LogTime logTime) => logTime switch
    {
        LogTime.Entry or LogTime.Exit => LogLevel.Information,
        LogTime.Trace => LogLevel.Trace,
        LogTime.Error => LogLevel.Error,
        _ => LogLevel.None,
    };
}
