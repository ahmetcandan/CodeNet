using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace NetCore.RabbitMQ;

public class RabbitMQProducerService<TData>(IOptions<RabbitMQSettings> Config)
        : BaseRabbitMQService<TData>(Config ?? throw new NullReferenceException("Config is null")),
          IRabbitMQProducerService<TData>
    where TData : class, new()
{
    public bool Publish(TData data)
    {
        return Publish(data, Guid.NewGuid().ToString());
    }

    public bool Publish(TData data, string messageId)
    {
        return Publish(data, messageId, new Dictionary<string, object>(1) { { "MessageId", messageId } });
    }

    public bool Publish(TData data, string messageId, IDictionary<string, object> headers)
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
}
