using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CodeNet.RabbitMQ.Services;

public class RabbitMQProducerService(IOptions<RabbitMQProducerOptions> options)
{
    public PublishModel? Publish<TModel>(TModel data)
    {
        return Publish(data, Guid.NewGuid().ToString("N"));
    }

    public PublishModel? Publish(byte[] data)
    {
        return Publish(data, Guid.NewGuid().ToString("N"));
    }
    public PublishModel? Publish<TModel>(TModel data, string messageId)
    {
        return Publish(data, messageId, new Dictionary<string, object>(1) { { "MessageId", messageId } });
    }

    public PublishModel? Publish(byte[] data, string messageId)
    {
        return Publish(data, messageId, new Dictionary<string, object>(1) { { "MessageId", messageId } });
    }

    public PublishModel? Publish<TModel>(TModel data, string messageId, IDictionary<string, object> headers)
    {
        return typeof(TModel).Equals(typeof(string))
            ? Publish(Encoding.UTF8.GetBytes(data?.ToString() ?? ""), messageId, headers)
            : Publish(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)), messageId, headers);
    }

    public PublishModel? Publish(byte[] data, string messageId, IDictionary<string, object> headers)
    {
        return Publish([new PublishModel(data, messageId, headers)]).FirstOrDefault() ?? null;
    }

    public virtual IEnumerable<PublishModel> Publish(IEnumerable<PublishModel> messages)
    {
        List<PublishModel> result = [];
        try
        {
            using var connection = options.Value.ConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            if (options.Value.DeclareQueue)
                channel.QueueDeclare(queue: options.Value.Queue,
                                     durable: options.Value.Durable,
                                     exclusive: options.Value.Exclusive,
                                     autoDelete: options.Value.AutoDelete,
                                     arguments: options.Value.Arguments);

            if (options.Value.DeclareExchange)
                channel.ExchangeDeclare(exchange: options.Value.Exchange,
                                        type: options.Value.ExchangeType,
                                        durable: options.Value.Durable,
                                        arguments: options.Value.ExchangeArguments);

            if (options.Value.QueueBind)
                channel.QueueBind(queue: options.Value.Queue,
                                  exchange: options.Value.Exchange,
                                  routingKey: options.Value.RoutingKey,
                                  arguments: options.Value.QueueBindArguments);

            foreach (var message in messages)
            {
                var basicProperties = channel.CreateBasicProperties();
                basicProperties.MessageId = message.MessageId;
                basicProperties.Headers = message.Headers;
                basicProperties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                channel.BasicPublish(exchange: options.Value.Exchange,
                                     routingKey: options.Value.RoutingKey,
                                     mandatory: options.Value.Mandatory ?? false,
                                     basicProperties: basicProperties,
                                     body: message.Data);
                result.Add(message);
            }
        }
        catch { }

        return result;
    }
}
