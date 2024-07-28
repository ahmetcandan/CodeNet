using CodeNet.StackExchange.Redis.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CodeNet.StackExchange.Redis.Services;

public class StackExchangeProducerService(IOptions<StackExchangeProducerOptions> options)
{
    public Task<bool> PublishAsync(object data)
    {
        var json = JsonConvert.SerializeObject(data);
        return PublishAsync(json);
    }

    public async Task<bool> PublishAsync(string message)
	{
		try
		{
            var connection = await ConnectionMultiplexer.ConnectAsync(options.Value.Configuration);
            var subscriber = connection.GetSubscriber();
            return (await subscriber.PublishAsync(RedisChannel.Pattern(options.Value.Channel), message, options.Value.CommandFlags)) > 0;
        }
        catch
        {
            return false;
        }
    }
}
