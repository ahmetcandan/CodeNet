using CodeNet.ExceptionHandling;

namespace CodeNet.Identity.Exception;

public class IdentityException(string code, string message) : CodeNetException(code, message)
{
}
