using NetCore.Abstraction;
using NetCore.Abstraction.Extension;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace NetCore.Logging
{
    public class AppLogger : IAppLogger
    {
        public void EntryLog(object request, MethodBase methodBase)
        {
            Console.WriteLine($"LogType: Entry, Data: {GetObjectToString(request)}, {MethodBaseToString(methodBase)}");
        }

        public void ExceptionLog(Exception exception, MethodBase methodBase)
        {
            Console.WriteLine($"LogType: Exception, Message: {exception.Message}, StackTrace: {exception.StackTrace}, {MethodBaseToString(methodBase)}");
        }

        public void ExceptionLog(Exception exception, object data, MethodBase methodBase)
        {
            Console.WriteLine($"LogType: Exception, Message: {exception.Message}, StackTrace: {exception.StackTrace}, Data: {GetObjectToString(data)}, {MethodBaseToString(methodBase)}");
        }

        public void ExitLog(object response, MethodBase methodBase)
        {
            Console.WriteLine($"LogType: Exit, Data: {GetObjectToString(response)}, {MethodBaseToString(methodBase)}");
        }

        public void ExitLog(object response, MethodBase methodBase, long time)
        {
            Console.WriteLine($"LogType: Exit, Data: {GetObjectToString(response)}, {MethodBaseToString(methodBase)}, Time: {time}");
        }

        public void TraceLog(object data, MethodBase methodBase)
        {
            Console.WriteLine($"LogType: Trace, Data: {GetObjectToString(data)}, {MethodBaseToString(methodBase)}");
        }

        protected virtual string MethodBaseToString(MethodBase methodBase)
        {
            var _methodBase = methodBase.GetMethodBase();
            return $"Assebly: {methodBase.DeclaringType.Assembly.GetName().Name}, Class: {_methodBase.DeclaringType.Name}, Method: {_methodBase.Name}";
        }

        protected virtual string GetObjectToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
