using CodeNet.StackExchange.Redis.Services;
using StackExchange.Redis;

namespace CodeNet.StackExchange.Redis.Settings;

public class StackExchangeProducerOptions : StackExchangeConsumerOptions
{
    public CommandFlags CommandFlags { get; set; }
}

public class StackExchangeProducerOptions<TProducerService> : StackExchangeProducerOptions
    where TProducerService : StackExchangeProducerService
{

}