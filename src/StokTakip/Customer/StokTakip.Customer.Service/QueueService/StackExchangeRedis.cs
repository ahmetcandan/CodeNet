using CodeNet.StackExchange.Redis.Models;
using CodeNet.StackExchange.Redis.Services;
using CodeNet.StackExchange.Redis.Settings;
using Microsoft.Extensions.Options;

namespace StokTakip.Customer.Service.QueueService;

public class RedisConsumerServiceA(IOptions<StackExchangeConsumerOptions<RedisConsumerServiceA>> options) : StackExchangeConsumerService(options)
{
}

public class RedisConsumerServiceB(IOptions<StackExchangeConsumerOptions<RedisConsumerServiceB>> options) : StackExchangeConsumerService(options)
{
}

public class RedisProducerServiceA(IOptions<StackExchangeProducerOptions<RedisProducerServiceA>> options) : StackExchangeProducerService(options)
{
}

public class RedisProducerServiceB(IOptions<StackExchangeProducerOptions<RedisProducerServiceB>> options) : StackExchangeProducerService(options)
{
}

public class RedisMessageHandlerA : IStackExchangeConsumerHandler<RedisConsumerServiceA>
{
    public void Handler(ReceivedMessageEventArgs args)
    {
        Console.WriteLine($"Consumed A: {args.Channel} - {args.Message}");
    }
}

public class RedisMessageHandlerB : IStackExchangeConsumerHandler<RedisConsumerServiceB>
{
    public void Handler(ReceivedMessageEventArgs args)
    {
        Console.WriteLine($"Consumed B: {args.Channel} - {args.Message}");
    }
}
