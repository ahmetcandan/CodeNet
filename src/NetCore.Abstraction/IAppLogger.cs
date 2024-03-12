using System;
using System.Reflection;

namespace NetCore.Abstraction;

public interface IAppLogger
{
    void EntryLog(object request, MethodBase methodBase);
    void ExitLog(object response, MethodBase methodBase);
    void ExitLog(object response, MethodBase methodBase, long time);
    void ExceptionLog(Exception exception, MethodBase methodBase);
    void ExceptionLog(Exception exception, object data, MethodBase methodBase);
    void TraceLog(object data, MethodBase methodBase);
}
