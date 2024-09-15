using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.MakerChecker.Exception;

public class MakerCheckerException : CodeNetException
{
    public MakerCheckerException(ExceptionMessage exceptionMessage) : base(exceptionMessage)
    {
    }

    public MakerCheckerException(string code, string message) : base(code, message)
    {
    }
}
