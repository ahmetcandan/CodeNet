namespace CodeNet.ExceptionHandling;

public class UserLevelException : CodeNetException
{
    public UserLevelException(string code, string message) : base(code, message) { }

    public UserLevelException(string code, string message, int httpStatusCode) : base(code, message, httpStatusCode) { }
}
