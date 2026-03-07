using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.RabbitMQ.Exception;

public class RabbitMQException : UserLevelException
{
    public RabbitMQException(ExceptionMessage exceptionMessage) : base(exceptionMessage.Code, exceptionMessage.Message) { }

    public RabbitMQException(string code, string message) : base(code, message) { }
}
