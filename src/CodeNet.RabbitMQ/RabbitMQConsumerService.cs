using Microsoft.Extensions.Options;
using CodeNet.Abstraction;
using CodeNet.Abstraction.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace CodeNet.RabbitMQ;

public class RabbitMQConsumerService<TData>(IOptions<RabbitMQSettings> Config)
        : BaseRabbitMQService<TData>(Config ?? throw new NullReferenceException("Config is null")),
          IRabbitMQConsumerService<TData>
    where TData : class, new()
{
    private EventingBasicConsumer? _consumer;
    private IConnection? _connection;
    private IModel? _channel;

    public void StartListening()
    {
        var factory = CreateFactory();
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: Config.Value.Queue,
                             durable: Config.Value.Durable,
                             exclusive: Config.Value.Exclusive,
                             autoDelete: Config.Value.AutoDelete,
                             arguments: null);
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += SentMessage;
        _channel.BasicConsume(queue: Config.Value.Queue,
                                 autoAck: Config.Value.AutoAck,
                                 consumer: _consumer);
    }

    public event MessageReceived<TData>? ReceivedMessage;

    private void SentMessage(object? model, BasicDeliverEventArgs args)
    {
        var byteArr = args.Body.ToArray();
        var json = Encoding.UTF8.GetString(byteArr);
        if (string.IsNullOrEmpty(json))
            throw new NullReferenceException("Message body is null");

        var data = JsonConvert.DeserializeObject<TData>(json) ?? throw new NullReferenceException("Message data is null");

        ReceivedMessage?.Invoke(new ReceivedMessageEventArgs<TData>
        {
            Data = data,
            MessageId = args.BasicProperties?.MessageId,
            Headers = args.BasicProperties?.Headers,
            ConsumerTag = args.ConsumerTag,
            DeliveryTag = args.DeliveryTag,
            Exchange = args.Exchange,
            RoutingKey = args.RoutingKey,
            Redelivered = args.Redelivered
        });
    }
}
