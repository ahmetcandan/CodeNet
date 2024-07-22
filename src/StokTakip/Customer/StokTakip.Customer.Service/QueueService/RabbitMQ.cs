using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Services;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;

namespace StokTakip.Customer.Service.QueueService;

public class ConsumerServiceA(IOptions<RabbitMQConsumerOptions<ConsumerServiceA>> options) : RabbitMQConsumerService(options)
{
}

public class ConsumerServiceB(IOptions<RabbitMQConsumerOptions<ConsumerServiceB>> options) : RabbitMQConsumerService(options)
{
}

public class ProducerServiceA(IOptions<RabbitMQProducerOptions<ProducerServiceA>> options) : RabbitMQProducerService(options)
{
}

public class ProducerServiceB(IOptions<RabbitMQProducerOptions<ProducerServiceB>> options) : RabbitMQProducerService(options)
{
}

public class MessageHandlerA : IRabbitMQConsumerHandler<ConsumerServiceA>
{
    public void Handler(ReceivedMessageEventArgs args)
    {
        Console.WriteLine($"Consumed A: {args.GetDataToString()}");
    }
}

public class MessageHandlerB : IRabbitMQConsumerHandler<ConsumerServiceB>
{
    public void Handler(ReceivedMessageEventArgs args)
    {
        Console.WriteLine($"Consumed B: {args.GetDataToString()}");
    }
}
