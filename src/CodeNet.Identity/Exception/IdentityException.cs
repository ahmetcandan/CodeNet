using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.Identity.Exception;

public class IdentityException : UserLevelException
{
    public IdentityException(ExceptionMessage exceptionMessage) : base(exceptionMessage.Code, exceptionMessage.Message)
    {
    }

    public IdentityException(string code, string message) : base(code, message)
    {
    }
}
