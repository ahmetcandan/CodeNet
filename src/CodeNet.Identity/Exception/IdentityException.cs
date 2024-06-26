using CodeNet.ExceptionHandling;

namespace CodeNet.Identity.Exception;

public class IdentityException(string code, string message) : UserLevelException(code, message)
{
}
