using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeNet.RabbitMQ.Services;

public class RabbitMQConsumerService(IOptions<RabbitMQConsumerOptions> options)
{
    private EventingBasicConsumer? _consumer;
    private AsyncEventingBasicConsumer? _asyncConsumer;
    private IConnection? _connection;
    private IModel? _channel;

    public void StartListening()
    {
        _connection = options.Value.ConnectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        if (options.Value.DeclareQueue)
            _channel.QueueDeclare(queue: options.Value.Queue,
                                 durable: options.Value.Durable,
                                 exclusive: options.Value.Exclusive,
                                 autoDelete: options.Value.AutoDelete,
                                 arguments: options.Value.Arguments);

        if (options.Value.DeclareExchange)
            _channel.ExchangeDeclare(exchange: options.Value.Exchange,
                                    type: options.Value.ExchangeType,
                                    durable: options.Value.Durable,
                                    arguments: options.Value.ExchangeArguments);

        if (options.Value.QueueBind)
            _channel.QueueBind(queue: options.Value.Queue,
                                exchange: options.Value.Exchange,
                                routingKey: options.Value.RoutingKey,
                                arguments: options.Value.QueueBindArguments);

        if (options.Value.AsyncConsumer)
        {
            _asyncConsumer = new(_channel);
            _asyncConsumer.Received += AsyncMessageHandler;
        }
        else
        {
            _consumer = new(_channel);
            _consumer.Received += MessageHandler;
        }

        if (options.Value.Qos is not null)
            _channel.BasicQos(prefetchSize: options.Value.Qos.PrefetchSize, prefetchCount: options.Value.Qos.PrefetchCount, global: options.Value.Qos.Global);

        _channel.BasicConsume(queue: options.Value.Queue,
                                 autoAck: options.Value.AutoAck,
                                 consumerTag: options.Value.ConsumerTag,
                                 noLocal: options.Value.NoLocal,
                                 exclusive: options.Value.Exclusive,
                                 arguments: options.Value.ConsumerArguments,
                                 consumer: options.Value.AsyncConsumer ? _asyncConsumer : _consumer);
    }

    public event MessageReceived? ReceivedMessage;

    private async void MessageHandler(object? model, BasicDeliverEventArgs args)
    {
        if (ReceivedMessage is not null)
            await ReceivedMessage.Invoke(new ReceivedMessageEventArgs
            {
                Data = args.Body,
                MessageId = args.BasicProperties?.MessageId,
                Headers = args.BasicProperties?.Headers,
                ConsumerTag = args.ConsumerTag,
                DeliveryTag = args.DeliveryTag,
                Exchange = args.Exchange,
                RoutingKey = args.RoutingKey,
                Redelivered = args.Redelivered,
                AppId = args.BasicProperties?.AppId,
                ClusterId = args.BasicProperties?.ClusterId,
                Priority = args.BasicProperties?.Priority,
                CorrelationId = args.BasicProperties?.CorrelationId,
                Type = args.BasicProperties?.Type,
                DeliveryMode = GetDeliveryMode(args.BasicProperties),
                ReceivedTime = GetReceivedTime(args.BasicProperties)
            });
    }

    private Task AsyncMessageHandler(object? model, BasicDeliverEventArgs args)
    {
        if (ReceivedMessage is not null)
            return ReceivedMessage.Invoke(new ReceivedMessageEventArgs
            {
                Data = args.Body,
                MessageId = args.BasicProperties?.MessageId,
                Headers = args.BasicProperties?.Headers,
                ConsumerTag = args.ConsumerTag,
                DeliveryTag = args.DeliveryTag,
                Exchange = args.Exchange,
                RoutingKey = args.RoutingKey,
                Redelivered = args.Redelivered,
                AppId = args.BasicProperties?.AppId,
                ClusterId = args.BasicProperties?.ClusterId,
                Priority = args.BasicProperties?.Priority,
                CorrelationId = args.BasicProperties?.CorrelationId,
                Type = args.BasicProperties?.Type,
                DeliveryMode = GetDeliveryMode(args.BasicProperties),
                ReceivedTime = GetReceivedTime(args.BasicProperties)
            });

        return Task.CompletedTask;
    }

    private static DateTime? GetReceivedTime(IBasicProperties? basicProperties)
    {
        if (basicProperties is not null)
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(basicProperties.Timestamp.UnixTime);

        return null;
    }

    private static DeliveredMode GetDeliveryMode(IBasicProperties? basicProperties)
    {
        return (basicProperties?.DeliveryMode) switch
        {
            1 => DeliveredMode.NonPersistent,
            2 => DeliveredMode.Persistent,
            _ => DeliveredMode.None,
        };
    }

    public void StopListening()
    {
        if (_channel?.IsOpen is true)
        {
            _channel.Close();

            if (_consumer is not null)
                _consumer.Received -= MessageHandler;

            if (_asyncConsumer is not null)
                _asyncConsumer.Received -= AsyncMessageHandler;
        }
    }

    public void CheckSuccessfullMessage(ulong deliveryTag, bool multiple = false)
    {
        if (options.Value.AutoAck)
            throw new Exception("This method cannot be used if 'AutoAck' is on.");

        if (_channel?.IsOpen is true)
            _channel.BasicAck(deliveryTag: deliveryTag, multiple: multiple);
    }

    public void CheckFailMessage(ulong deliveryTag, bool multiple = false, bool requeue = true)
    {
        if (options.Value.AutoAck)
            throw new Exception("This method cannot be used if 'AutoAck' is on.");

        if (_channel?.IsOpen is true)
            _channel.BasicNack(deliveryTag: deliveryTag, multiple: multiple, requeue: requeue);
    }
}
