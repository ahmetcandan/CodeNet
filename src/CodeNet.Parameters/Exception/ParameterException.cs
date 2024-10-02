using CodeNet.ExceptionHandling;

namespace CodeNet.Parameters.Exception;

public class ParameterException : UserLevelException
{
    public ParameterException(string code, string message) : base(code, message)
    {
    }

    public ParameterException(string code, string message, int httpStatusCode) : base(code, message, httpStatusCode)
    {
        
    }
}
