using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.Email.Exception;

public class EmailException : UserLevelException
{
    public EmailException(ExceptionMessage exceptionMessage) : base(exceptionMessage.Code, exceptionMessage.Message)
    {
    }

    public EmailException(string code, string message) : base(code, message)
    {
    }
}
