﻿using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CodeNet.RabbitMQ.Services;

public class RabbitMQProducerService(IOptions<RabbitMQProducerOptions> options)
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
        if (typeof(TModel).Equals(typeof(string)))
            return Publish(Encoding.UTF8.GetBytes(data.ToString()), messageId, headers);

        return Publish(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)), messageId, headers);
    }

    public bool Publish(byte[] data, string messageId, IDictionary<string, object> headers)
    {
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
                                        type: options.Value.Type,
                                        durable: options.Value.Durable,
                                        arguments: options.Value.Arguments);

            var basicProperties = channel.CreateBasicProperties();
            basicProperties.MessageId = messageId;
            basicProperties.Headers = headers;
            
            channel.BasicPublish(exchange: options.Value.Exchange,
                                 routingKey: options.Value.RoutingKey,
                                 mandatory: options.Value.Mandatory ?? false,
                                 basicProperties: basicProperties,
                                 body: data);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
