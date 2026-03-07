using CodeNet.Core.Models;

namespace CodeNet.RabbitMQ.Exception;

internal static class ExceptionMessages
{
    public static ExceptionMessage AutoAck { get { return new ExceptionMessage("RQ0001", "This method cannot be used if 'AutoAck' is on."); } }

    public static ExceptionMessage UseParams(this ExceptionMessage exceptionMessage, params string[] args) => new(exceptionMessage.Code, string.Format(exceptionMessage.Message, args));
}
