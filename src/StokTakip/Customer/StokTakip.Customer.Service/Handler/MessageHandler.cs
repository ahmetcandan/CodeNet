using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Services;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Service.Handler;

public class MessageHandler : IRabbitMQConsumerHandler<KeyValueModel>
{
    public void Handler(ReceivedMessageEventArgs<KeyValueModel> args)
    {
        Console.WriteLine($"MessageId: {args.MessageId}, Value: {args.Data.Value}");
    }
}
