using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Services;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Service.Handler;

public class MessageConsumerHandler : IRabbitMQConsumerHandler<MongoModel>
{
    public void Handler(ReceivedMessageEventArgs<MongoModel> args)
    {
        Console.WriteLine($"MessageId: {args.MessageId}, Value: {args.Data.Value}");
    }
}
