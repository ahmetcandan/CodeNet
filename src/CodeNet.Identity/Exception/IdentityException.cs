using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.Identity.Exception;

public class IdentityException(string code, string message) : UserLevelException(code, message)
{
    public IdentityException(ExceptionMessage exceptionMessage) : this(exceptionMessage.Code, exceptionMessage.Message)
    {
    }
}
