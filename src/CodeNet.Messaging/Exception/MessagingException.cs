using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.Messaging.Exception;

public class MessagingException : UserLevelException
{
    public MessagingException(ExceptionMessage exceptionMessage) : base(exceptionMessage.Code, exceptionMessage.Message) { }

    public MessagingException(string code, string message) : base(code, message) { }
}
