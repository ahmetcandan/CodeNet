using CodeNet.Abstraction;
using CodeNet.Abstraction.Enum;
using CodeNet.Abstraction.Extension;
using Newtonsoft.Json;
using System.Reflection;

namespace CodeNet.Logging;

public class AppLogger(IIdentityContext IdentityContext) : IAppLogger
{
    public void EntryLog(object request, MethodBase methodBase) => PostLogData(LogTime.Entry, methodBase, request);

    public void ExceptionLog(Exception exception, MethodBase methodBase) => PostLogData(LogTime.Error, methodBase, exception);

    public void ExceptionLog(Exception exception, object data, MethodBase methodBase) => PostLogData(LogTime.Error, methodBase, new { Exception = exception, Data = data });

    public void ExitLog(object response, MethodBase methodBase) => PostLogData(LogTime.Exit, methodBase, response);

    public void ExitLog(object response, MethodBase methodBase, long time) => PostLogData(LogTime.Exit, methodBase, response, elapsedDuration: time);

    public void TraceLog(object data, MethodBase methodBase) => PostLogData(LogTime.Trace, methodBase, data);

    protected virtual string GetObjectToString(object obj) => JsonConvert.SerializeObject(obj);

    private void PostLogData(LogTime logTime, MethodBase methodBase, object data, long? elapsedDuration = null) => PostLogData(logTime, methodBase, GetObjectToString(data), elapsedDuration);

    private void PostLogData(LogTime logTime, MethodBase methodBase, string data, long? elapsedDuration = null)
    {
        var _methodBase = methodBase.GetMethodBase();
        PostLogData(logTime, _methodBase!.DeclaringType!.Assembly.GetName().Name, _methodBase.DeclaringType.Name, _methodBase.Name, data, elapsedDuration);
    }

    private void PostLogData(LogTime logTime, string? assemblyName, string className, string methodName, string data, long? elapsedDuration = null)
    {
        Console.WriteLine($"{{\"LogTime\": \"{logTime}\", \"AssemblyName\": \"{assemblyName}\", \"ClassName\":  \"{className}\", \"MethodName\": \"{methodName}\", \"Username\": \"{IdentityContext.UserName}\", \"RequestId\": \"{IdentityContext.RequestId}\", \"Data\": \"{data.Replace("\"", "'")}\", \"ElapsedDuration\": {(elapsedDuration.HasValue ? elapsedDuration.ToString() : "null")}}}");
        //QService.Post(Settings.LOG_CHANNEL_NAME, new LogModel
        //{
        //    AssemblyName = assemblyName ?? "unknow",
        //    ClassName = className,
        //    MethodName = methodName,
        //    LogTime = logTime.ToString(),
        //    Username = IdentityContext.UserName,
        //    Data = data,
        //    RequestId = IdentityContext.RequestId,
        //    ElapsedDuration = elapsedDuration
        //});
    }
}
