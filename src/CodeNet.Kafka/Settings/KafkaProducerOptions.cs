using CodeNet.Kafka.Services;
using Confluent.Kafka;

namespace CodeNet.Kafka.Settings;

public class KafkaProducerOptions : BaseKafkaOptions
{
    public required ProducerConfig Config { get; set; }
}

public class KafkaProducerOptions<TProducerService, TKey, TValue> : KafkaProducerOptions
    where TProducerService : KafkaProducerService<TKey, TValue>
{
}
