using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.Identity.Exception;

public class IdentityException : CodeNetException
{
    public IdentityException(ExceptionMessage exceptionMessage) : base(exceptionMessage)
    {
    }

    public IdentityException(string code, string message) : base(code, message)
    {
    }
}
