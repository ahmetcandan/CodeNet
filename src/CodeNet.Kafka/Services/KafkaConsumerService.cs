using CodeNet.Kafka.Models;
using CodeNet.Kafka.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace CodeNet.Kafka.Services;

public class KafkaConsumerService(IOptions<KafkaConsumerOptions> options) : KafkaConsumerService<string>(options)
{
}

public class KafkaConsumerService<TValue>(IOptions<KafkaConsumerOptions> options) : KafkaConsumerService<Null, TValue>(options)
{
}

public class KafkaConsumerService<TKey, TValue>(IOptions<KafkaConsumerOptions> options)
{
    private IConsumer<TKey, TValue>? _consumer;
    private bool _listen = false;

    public void StartListening()
    {
        _listen = true;
        _consumer = new ConsumerBuilder<TKey, TValue>(options.Value.Config).Build();
        _consumer.Subscribe(options.Value.Topic);
        while (_listen)
        {
            var result = _consumer.Consume();
            if (result is not null && ReceivedMessage is not null)
            {
                ReceivedMessage.Invoke(new ReceivedMessageEventArgs<TKey, TValue>
                {
                    Headers = result.Message.Headers,
                    Key = result.Message.Key,
                    Value = result.Message.Value,
                    Offset = result.Offset,
                    Partition = result.Partition,
                    Timestamp = result.Message.Timestamp
                });
            }
        }
    }

    public void StopListening()
    {
        _consumer?.Close();
        _consumer?.Unsubscribe();
        _listen = false;
    }

    public event MessageReceived<TKey, TValue>? ReceivedMessage;

    public void CommitCheckPoint(int partition, long offset)
    {
        if (options.Value.Config.EnableAutoCommit is true)
            throw new Exception("This method cannot be used if 'EnableAutoCommit' is on.");

        _consumer?.Commit([new(options.Value.Topic, partition, offset)]);
    }
}
