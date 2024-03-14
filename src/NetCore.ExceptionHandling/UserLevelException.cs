using System;

namespace NetCore.ExceptionHandling;

public class UserLevelException : Exception
{
    public UserLevelException()
    {

    }

    public UserLevelException(string code, string message)
    {
        Code = code;
        UserMessage = message;
    }

    public string UserMessage { get; set; }
    public string Code { get; set; }
    public override string StackTrace => string.Empty;
    public override string Message => UserMessage;
}
