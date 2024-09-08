using CodeNet.Kafka.Models;
using Confluent.Kafka;

namespace CodeNet.Kafka.Services;

public interface IKafkaConsumerHandler<TConsumerService> : IKafkaConsumerHandler<TConsumerService, string>
    where TConsumerService : KafkaConsumerService
{
}

public interface IKafkaConsumerHandler<TConsumerService, TValue> : IKafkaConsumerHandler<TConsumerService, Null, TValue>
    where TConsumerService : KafkaConsumerService<TValue>
{
}

public interface IKafkaConsumerHandler<TConsumerService, TKey, TValue>
    where TConsumerService : KafkaConsumerService<TKey, TValue>
{
    Task Handler(ReceivedMessageEventArgs<TKey, TValue> args);
}
