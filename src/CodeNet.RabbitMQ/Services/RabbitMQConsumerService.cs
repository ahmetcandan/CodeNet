using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeNet.RabbitMQ.Services;

public class RabbitMQConsumerService(IOptions<RabbitMQConsumerOptions> options)
{
    private EventingBasicConsumer? _consumer;
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

        _consumer = new EventingBasicConsumer(_channel);

        if (options.Value.Qos is not null)
            _channel.BasicQos(prefetchSize: options.Value.Qos.PrefetchSize, prefetchCount: options.Value.Qos.PrefetchCount, global: options.Value.Qos.Global);

        _consumer.Received += MessageHandler;
        _channel.BasicConsume(queue: options.Value.Queue,
                                 autoAck: options.Value.AutoAck,
                                 consumerTag: options.Value.ConsumerTag,
                                 noLocal: options.Value.NoLocal,
                                 exclusive: options.Value.Exclusive,
                                 arguments: options.Value.ConsumerArguments,
                                 consumer: _consumer);
    }

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
                Redelivered = args.Redelivered
            });
    }

    public void StopListening()
    {
        if (_channel?.IsOpen is true)
        {
            _channel.Close();

            if (_consumer is not null)
                _consumer.Received -= MessageHandler;
        }
    }

    public event MessageReceived? ReceivedMessage;

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
