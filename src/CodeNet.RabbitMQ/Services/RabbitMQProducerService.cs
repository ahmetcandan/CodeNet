using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CodeNet.RabbitMQ.Services;

public class RabbitMQProducerService(IOptions<RabbitMQProducerOptions> options)
{
    public void Publish<TModel>(TModel data)
    {
        Publish(data, Guid.NewGuid().ToString("N"));
    }

    public void Publish(byte[] data)
    {
        Publish(data, Guid.NewGuid().ToString("N"));
    }
    public void Publish<TModel>(TModel data, string messageId)
    {
        Publish(data, messageId, new Dictionary<string, object>(1) { { "MessageId", messageId } });
    }

    public void Publish(byte[] data, string messageId)
    {
        Publish(data, messageId, new Dictionary<string, object>(1) { { "MessageId", messageId } });
    }

    public void Publish<TModel>(TModel data, string messageId, IDictionary<string, object> headers)
    {
        if (typeof(TModel).Equals(typeof(string)))
            Publish(Encoding.UTF8.GetBytes(data?.ToString() ?? ""), messageId, headers);
        else
            Publish(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)), messageId, headers);
    }

    public void Publish(byte[] data, string messageId, IDictionary<string, object> headers)
    {
        using var connection = options.Value.ConnectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        if (options.Value.DeclareQueue)
            channel.QueueDeclare(queue: options.Value.Queue,
                                 durable: options.Value.Durable,
                                 exclusive: options.Value.Exclusive,
                                 autoDelete: options.Value.AutoDelete,
                                 arguments: options.Value.Arguments);

        if (options.Value.Exchange is not null)
            channel.ExchangeDeclare(exchange: options.Value.Exchange.Name,
                                    type: options.Value.Exchange.Type,
                                    durable: options.Value.Durable,
                                    arguments: options.Value.Arguments);

        var basicProperties = channel.CreateBasicProperties();
        basicProperties.MessageId = messageId;
        basicProperties.Headers = headers;

        channel.BasicPublish(exchange: options.Value.Exchange?.Name ?? "",
                             routingKey: options.Value.RoutingKey,
                             mandatory: options.Value.Mandatory ?? false,
                             basicProperties: basicProperties,
                             body: data);
    }
}
