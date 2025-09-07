using CodeNet.Kafka.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace CodeNet.Kafka.Services;

public class KafkaProducerService(IOptions<KafkaProducerOptions> options) : KafkaProducerService<string>(options)
{
}

public class KafkaProducerService<TValue>(IOptions<KafkaProducerOptions> options) : KafkaProducerService<Null, TValue>(options)
{
}


public class KafkaProducerService<TKey, TValue>(IOptions<KafkaProducerOptions> options)
{
    public Task Publish(TKey key, TValue value)
    {
        return Publish(key, value, CancellationToken.None);
    }

    public Task Publish(TKey key, TValue value, CancellationToken cancellationToken)
    {
        return Publish(key, value, null, cancellationToken);
    }

    public Task Publish(TKey key, TValue value, Headers? headers, CancellationToken cancellationToken)
        => Publish(key, value, headers, new Timestamp(DateTime.Now), cancellationToken);

    public async Task Publish(TKey key, TValue value, Headers? headers, Timestamp timestamp, CancellationToken cancellationToken = default)
    {
        using var producer = new ProducerBuilder<TKey, TValue>(options.Value.Config).Build();
        var result = await producer.ProduceAsync(topic: options.Value.Topic,
                                                 message: new Message<TKey, TValue> { Key = key, Value = value, Headers = headers, Timestamp = timestamp },
                                                 cancellationToken: cancellationToken);
    }
}
