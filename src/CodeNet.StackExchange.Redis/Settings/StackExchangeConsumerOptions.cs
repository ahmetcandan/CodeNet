using CodeNet.StackExchange.Redis.Services;

namespace CodeNet.StackExchange.Redis.Settings;

public class StackExchangeConsumerOptions
{
    public string Configuration { get; set; }
    public string Channel { get; set; } = "default";
}

public class StackExchangeConsumerOptions<TConsumerService> : StackExchangeConsumerOptions
    where TConsumerService : StackExchangeConsumerService
{

}
