using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.Parameters.Exception;

public class ParameterException(string code, string message) : UserLevelException(code, message)
{
    public ParameterException(ExceptionMessage exceptionMessage) : this(exceptionMessage.Code, exceptionMessage.Message) { }
}
