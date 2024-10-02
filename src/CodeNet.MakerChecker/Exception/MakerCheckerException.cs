using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.MakerChecker.Exception;

public class MakerCheckerException : UserLevelException
{
    public MakerCheckerException(ExceptionMessage exceptionMessage) : base(exceptionMessage.Code, exceptionMessage.Message)
    {
    }

    public MakerCheckerException(string code, string message) : base(code, message)
    {
    }
}
