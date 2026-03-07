using CodeNet.Core.Models;
using CodeNet.ExceptionHandling;

namespace CodeNet.Kafka.Exception;

public class KafkaException(string code, string message) : UserLevelException(code, message)
{
    public KafkaException(ExceptionMessage exceptionMessage) : this(exceptionMessage.Code, exceptionMessage.Message) { }
}
