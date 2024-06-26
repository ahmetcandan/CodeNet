using CodeNet.ExceptionHandling;

namespace CodeNet.MakerChecker.Exception;

public class MakerCheckerException(string code, string message) : UserLevelException(code, message)
{
}
