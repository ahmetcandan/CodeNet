﻿using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace CodeNet.RabbitMQ.Services;

public class RabbitMQConsumerService(IOptions<RabbitMQConsumerOptions> options)
        : BaseRabbitMQService(options ?? throw new NullReferenceException("Config is null"))
{
    private EventingBasicConsumer? _consumer;
    private IConnection? _connection;
    private IModel? _channel;

    public void StartListening()
    {
        var factory = CreateFactory();
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: options.Value.Queue,
                             durable: options.Value.Durable,
                             exclusive: options.Value.Exclusive,
                             autoDelete: options.Value.AutoDelete,
                             arguments: null);
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += SentMessage;
        _channel.BasicConsume(queue: options.Value.Queue,
                                 autoAck: options.Value.AutoAck,
                                 consumer: _consumer);
    }

    public event MessageReceived? ReceivedMessage;

    private void SentMessage(object? model, BasicDeliverEventArgs args)
    {
        ReceivedMessage?.Invoke(new ReceivedMessageEventArgs
        {
            Data = args.Body,
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
