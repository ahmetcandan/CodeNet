using CodeNet.StackExchange.Redis.Services;

namespace CodeNet.StackExchange.Redis.Settings;

public class StackExchangeSubscribeOptions
{
    public string Configuration { get; set; }
    public string Channel { get; set; } = "default";
}

public class StackExchangeSubscribeOptions<TSubscribeService> : StackExchangeSubscribeOptions
    where TSubscribeService : StackExchangeSubscribeService
{
}
