using CodeNet.Core.Models;

namespace CodeNet.Kafka.Exception;

internal static class ExceptionMessages
{
    public static ExceptionMessage EnableAutoCommit { get { return new ExceptionMessage("KF0001", "This method cannot be used if 'EnableAutoCommit' is on!"); } }

    public static ExceptionMessage UseParams(this ExceptionMessage exceptionMessage, params string[] args) => new(exceptionMessage.Code, string.Format(exceptionMessage.Message, args));
}
