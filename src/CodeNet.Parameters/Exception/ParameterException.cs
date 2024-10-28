using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.Parameters.Exception;

public class ParameterException : UserLevelException
{
    public ParameterException(ExceptionMessage exceptionMessage) : base(exceptionMessage.Code, exceptionMessage.Message)
    {
    }

    public ParameterException(string code, string message) : base(code, message)
    {
    }
}
