using CodeNet.Core.Context;
using CodeNet.Core.Extensions;
using CodeNet.Logging.Enum;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Reflection;

namespace CodeNet.Logging;

public class AppLogger(ICodeNetContext codeNetContext, ILogger<AppLogger> logger) : IAppLogger
{
    private const string _message1 = "AssemblyName: {AssemblyName}, ClassName: {ClassName}, MethodName: {MethodName}, Data: {Data}, CorrelationId: {CorrelationId}, ElapsedDuration: {ElapsedDuration}, LogTime: {LogTime}, Username: {Username}";
    private const string _message2 = "AssemblyName: {AssemblyName}, ClassName: {ClassName}, MethodName: {MethodName}, Data: {@Data}, CorrelationId: {CorrelationId}, ElapsedDuration: {ElapsedDuration}, LogTime: {LogTime}, Username: {Username}";

    public void EntryLog(object? request, MethodBase? methodBase) => PostLogData(LogTime.Entry, methodBase, request);

    public void ExceptionLog(Exception exception, MethodBase? methodBase) => PostLogData(LogTime.Error, methodBase, exception, exception: exception);

    public void ExceptionLog(Exception exception, object? data, MethodBase? methodBase) => PostLogData(LogTime.Error, methodBase, new { Exception = exception, Data = data }, exception: exception);

    public void ExitLog(object? response, MethodBase? methodBase) => PostLogData(LogTime.Exit, methodBase, response);

    public void ExitLog(object? response, MethodBase? methodBase, long time) => PostLogData(LogTime.Exit, methodBase, response, elapsedDuration: time);

    public void TraceLog(object? data, MethodBase? methodBase) => PostLogData(LogTime.Trace, methodBase, data);

    protected virtual string GetObjectToString(object obj) => obj is string ? obj.ToString() ?? string.Empty : JsonConvert.SerializeObject(obj);

    private void PostLogData(LogTime logTime, MethodBase? methodBase, object? data, long? elapsedDuration = null, Exception? exception = null)
    {
        var _methodBase = methodBase?.GetMethodBase();
        if (IsSimpleType(data?.GetType()))
            PostLogData(logTime, _methodBase?.DeclaringType?.Assembly.GetName().Name, _methodBase?.DeclaringType?.Name ?? string.Empty, _methodBase?.Name ?? string.Empty, data?.ToString() ?? string.Empty, elapsedDuration: elapsedDuration, exception: exception);
        else
            PostLogData(logTime, _methodBase?.DeclaringType?.Assembly.GetName().Name, _methodBase?.DeclaringType?.Name ?? string.Empty, _methodBase?.Name ?? string.Empty, data, elapsedDuration: elapsedDuration, exception: exception);
    }

    private void PostLogData(LogTime logTime, string? assemblyName, string className, string methodName, string data, long? elapsedDuration = null, Exception? exception = null)
        => logger.Log(
            LogTimeToLevel(logTime),
            new EventId(codeNetContext.CorrelationId.GetHashCode(), $"{assemblyName}_{className}_{methodName}"),
            exception,
            _message1,
            assemblyName ?? string.Empty,
            className,
            methodName,
            data,
            codeNetContext.CorrelationId,
            elapsedDuration,
            logTime.ToString(),
            codeNetContext.UserName);

    private void PostLogData(LogTime logTime, string? assemblyName, string className, string methodName, object? data, long? elapsedDuration = null, Exception? exception = null)
        => logger.Log(
            LogTimeToLevel(logTime),
            new EventId(codeNetContext.CorrelationId.GetHashCode(), $"{assemblyName}_{className}_{methodName}"),
            exception,
            _message2,
            assemblyName ?? string.Empty,
            className,
            methodName,
            data,
            codeNetContext.CorrelationId,
            elapsedDuration,
            logTime.ToString(),
            codeNetContext.UserName);

    private static bool IsSimpleType(Type? type) => type is null || type.IsPrimitive || type.IsValueType || type == typeof(string);

    private static LogLevel LogTimeToLevel(LogTime logTime) => logTime switch
    {
        LogTime.Entry or LogTime.Exit => LogLevel.Information,
        LogTime.Trace => LogLevel.Trace,
        LogTime.Error => LogLevel.Error,
        _ => LogLevel.None,
    };
}
