using CodeNet.StackExchange.Redis.Models;
using CodeNet.StackExchange.Redis.Services;
using CodeNet.StackExchange.Redis.Settings;
using Microsoft.Extensions.Options;

namespace StokTakip.Customer.Service.QueueService;

public class RedisConsumerServiceA(IOptions<StackExchangeSubscribeOptions<RedisConsumerServiceA>> options) : StackExchangeSubscribeService(options)
{
}

public class RedisConsumerServiceB(IOptions<StackExchangeSubscribeOptions<RedisConsumerServiceB>> options) : StackExchangeSubscribeService(options)
{
}

public class RedisProducerServiceA(IOptions<StackExchangePublisherOptions<RedisProducerServiceA>> options) : StackExchangePublisherService(options)
{
}

public class RedisProducerServiceB(IOptions<StackExchangePublisherOptions<RedisProducerServiceB>> options) : StackExchangePublisherService(options)
{
}

public class RedisMessageHandlerA : IStackExchangeSubscribeHandler<RedisConsumerServiceA>
{
    public Task Handler(ReceivedMessageEventArgs args)
    {
        Console.WriteLine($"Consumed A: {args.Channel} - {args.Message}");
        return Task.CompletedTask;
    }
}

public class RedisMessageHandlerB : IStackExchangeSubscribeHandler<RedisConsumerServiceB>
{
    public Task Handler(ReceivedMessageEventArgs args)
    {
        Console.WriteLine($"Consumed B: {args.Channel} - {args.Message}");
        return Task.CompletedTask;
    }
}
