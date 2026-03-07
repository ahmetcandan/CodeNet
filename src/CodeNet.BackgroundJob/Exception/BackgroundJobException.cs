using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.BackgroundJob.Exception;

public class BackgroundJobException(string code, string message) : UserLevelException(code, message)
{
    public BackgroundJobException(ExceptionMessage exceptionMessage) : this(exceptionMessage.Code, exceptionMessage.Message) { }
}
