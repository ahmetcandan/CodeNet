using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CodeNet.RabbitMQ.Services;

public class RabbitMQProducerService(IOptions<RabbitMQProducerOptions> Config)
        : BaseRabbitMQService(Config ?? throw new NullReferenceException("Config is null"))
{
    public bool Publish<TModel>(TModel data)
    {
        return Publish(data, Guid.NewGuid().ToString("N"));
    }

    public bool Publish(byte[] data)
    {
        return Publish(data, Guid.NewGuid().ToString("N"));
    }
    public bool Publish<TModel>(TModel data, string messageId)
    {
        return Publish(data, messageId, new Dictionary<string, object>(1) { { "MessageId", messageId } });
    }

    public bool Publish(byte[] data, string messageId)
    {
        return Publish(data, messageId, new Dictionary<string, object>(1) { { "MessageId", messageId } });
    }

    public bool Publish<TModel>(TModel data, string messageId, IDictionary<string, object> headers)
    {
        return Publish(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)), messageId, headers);
    }

    public bool Publish(byte[] data, string messageId, IDictionary<string, object> headers)
    {
        try
        {
            var factory = CreateFactory();
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: Config.Value.Queue,
                                 durable: Config.Value.Durable,
                                 exclusive: Config.Value.Exclusive,
                                 autoDelete: Config.Value.AutoDelete,
                                 arguments: null);
            var basicProperties = channel.CreateBasicProperties();
            basicProperties.MessageId = messageId;
            basicProperties.Headers = headers;

            channel.BasicPublish(exchange: Config.Value.Exchange,
                                 routingKey: Config.Value.RoutingKey,
                                 basicProperties: basicProperties,
                                 body: data);
            Console.WriteLine($"RabbitMQ Exchange: {Config.Value.Exchange}, RoutingKey: {Config.Value.RoutingKey}, Queue: {Config.Value.Queue}, MessageId: {messageId}");
            return true;
        }
        catch
        {
            return false;
        }
    }
}
