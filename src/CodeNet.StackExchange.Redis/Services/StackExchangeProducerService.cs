using CodeNet.StackExchange.Redis.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CodeNet.StackExchange.Redis.Services;

public class StackExchangeProducerService(IOptions<StackExchangeProducerOptions> options)
{
    public async Task<bool> PublishAsync(string message)
	{
		try
		{
            var connection = await ConnectionMultiplexer.ConnectAsync(options.Value.Configuration);
            var subscriber = connection.GetSubscriber();
            await subscriber.PublishAsync(RedisChannel.Pattern(options.Value.Channel), message, options.Value.CommandFlags);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
