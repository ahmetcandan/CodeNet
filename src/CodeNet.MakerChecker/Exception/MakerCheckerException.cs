using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.MakerChecker.Exception;

public class MakerCheckerException(string code, string message) : UserLevelException(code, message)
{
    public MakerCheckerException(ExceptionMessage exceptionMessage) : this(exceptionMessage.Code, exceptionMessage.Message)
    {
    }
}
