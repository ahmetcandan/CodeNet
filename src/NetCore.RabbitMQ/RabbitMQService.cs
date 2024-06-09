using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NetCore.RabbitMQ;

public class RabbitMQService<TData>(IOptions<RabbitMQSettings> Config) : IRabbitMQService<TData>
    where TData : class, new()
{
    public bool Post(TData data)
    {
        return Post(data, Guid.NewGuid().ToString());
    }

    public bool Post(TData data, string messageId)
    {
        return Post(data, messageId, new Dictionary<string, object>(1) { { "MessageId", messageId } });
    }

    public bool Post(TData data, string messageId, IDictionary<string, object> headers)
    {
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = Config.Value.HostName,
                UserName = Config.Value.Username,
                Password = Config.Value.Password
            };

            if (Config.Value.SocketReadTimeout.HasValue)
                factory.SocketReadTimeout = Config.Value.SocketReadTimeout.Value;
            if (!string.IsNullOrEmpty(Config.Value.ClientProvidedName))
                factory.ClientProvidedName = Config.Value.ClientProvidedName;
            if (Config.Value.Port.HasValue)
                factory.Port = Config.Value.Port.Value;
            if (Config.Value.MaxMessageSize.HasValue)
                factory.MaxMessageSize = Config.Value.MaxMessageSize.Value;

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: Config.Value.Queue,
                                 durable: Config.Value.Durable,
                                 exclusive: Config.Value.Exclusive,
                                 autoDelete: Config.Value.AutoDelete,
                                 arguments: null);
            string message = JsonConvert.SerializeObject(data);
            var body = Encoding.UTF8.GetBytes(message);
            var basicProperties = channel.CreateBasicProperties();
            basicProperties.MessageId = messageId;
            basicProperties.Headers = headers;

            channel.BasicPublish(exchange: Config.Value.Exchange,
                                 routingKey: Config.Value.RoutingKey,
                                 basicProperties: basicProperties,
                                 body: body);
            Console.WriteLine($"RabbitMQ Exchange: {Config.Value.Exchange}, RoutingKey: {Config.Value.RoutingKey}, Queue: {Config.Value.Queue}, MessageId: {messageId}");
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void ListenConnection()
    {
        var factory = new ConnectionFactory()
        {
            HostName = Config.Value.HostName,
            UserName = Config.Value.Username,
            Password = Config.Value.Password
        };

        if (Config.Value.SocketReadTimeout.HasValue)
            factory.SocketReadTimeout = Config.Value.SocketReadTimeout.Value;
        if (!string.IsNullOrEmpty(Config.Value.ClientProvidedName))
            factory.ClientProvidedName = Config.Value.ClientProvidedName;
        if (Config.Value.Port.HasValue)
            factory.Port = Config.Value.Port.Value;
        if (Config.Value.MaxMessageSize.HasValue)
            factory.MaxMessageSize = Config.Value.MaxMessageSize.Value;

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: Config.Value.Queue,
                             durable: Config.Value.Durable,
                             exclusive: Config.Value.Exclusive,
                             autoDelete: Config.Value.AutoDelete,
                             arguments: null);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += SentMessage;
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
