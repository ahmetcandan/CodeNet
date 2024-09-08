using CodeNet.Kafka.Services;
using Confluent.Kafka;

namespace CodeNet.Kafka.Settings;

public class KafkaConsumerOptions : BaseKafkaOptions
{
    public ConsumerConfig Config { get; set; }
}

public class KafkaConsumerOptions<TConsumerService, TKey, TValue> : KafkaConsumerOptions
    where TConsumerService : KafkaConsumerService<TKey, TValue>
{
}
