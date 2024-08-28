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
        _channel.QueueDeclare(queue: options.Value.Queue,
                             durable: options.Value.Durable,
                             exclusive: options.Value.Exclusive,
                             autoDelete: options.Value.AutoDelete,
                             arguments: options.Value.Arguments);
        _consumer = new EventingBasicConsumer(_channel);

        if (options.Value.Qos is not null)
            _channel.BasicQos(prefetchSize: options.Value.Qos.PrefetchSize, prefetchCount: options.Value.Qos.PrefetchCount, global: options.Value.Qos.Global);

        _consumer.Received += async (object? model, BasicDeliverEventArgs args) => await SentMessage(model, args);
        _channel.BasicConsume(queue: options.Value.Queue,
                                 autoAck: options.Value.AutoAck,
                                 consumerTag: options.Value.ConsumerTag,
                                 noLocal: options.Value.NoLocal,
                                 exclusive: options.Value.Exclusive,
                                 arguments: options.Value.Arguments,
                                 consumer: _consumer);
    }

    public event MessageReceived? ReceivedMessage;

    private async Task SentMessage(object? model, BasicDeliverEventArgs args)
    {
        if (!options.Value.AutoAck)
        {
            try
            {
                await MessageInvoke(args);

                if (!options.Value.AutoAck)
                    _channel?.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
            }
            catch
            {
                if (!options.Value.AutoAck)
                    _channel?.BasicNack(deliveryTag: args.DeliveryTag, multiple: false, requeue: true);
            }
        }
        else
        {
            await MessageInvoke(args);
        }
    }

    private Task MessageInvoke(BasicDeliverEventArgs args)
    {
        return ReceivedMessage is not null ?
            ReceivedMessage.Invoke(new ReceivedMessageEventArgs
            {
                Data = args.Body,
                MessageId = args.BasicProperties?.MessageId,
                Headers = args.BasicProperties?.Headers,
                ConsumerTag = args.ConsumerTag,
                DeliveryTag = args.DeliveryTag,
                Exchange = args.Exchange,
                RoutingKey = args.RoutingKey,
                Redelivered = args.Redelivered
            })
            : Task.CompletedTask;
    }
}
