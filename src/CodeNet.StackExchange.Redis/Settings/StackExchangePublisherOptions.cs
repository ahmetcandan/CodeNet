using CodeNet.StackExchange.Redis.Services;
using StackExchange.Redis;

namespace CodeNet.StackExchange.Redis.Settings;

public class StackExchangePublisherOptions : StackExchangeSubscribeOptions
{
    public CommandFlags CommandFlags { get; set; }
}

public class StackExchangePublisherOptions<TProducerService> : StackExchangePublisherOptions
    where TProducerService : StackExchangePublisherService
{
}