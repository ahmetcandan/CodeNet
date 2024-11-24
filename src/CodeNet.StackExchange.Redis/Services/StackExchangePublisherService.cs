using CodeNet.StackExchange.Redis.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CodeNet.StackExchange.Redis.Services;

public class StackExchangePublisherService(ISerializer serializer, IOptions<StackExchangePublisherOptions> options)
{
    public Task<long> PublishAsync(object data)
    {
        return PublishAsync(serializer.Serialize(data));
    }

    public Task PublishAsync(object[] datas)
    {
        return PublishAsync(datas.Select(data => serializer.Serialize(data)).ToArray());
    }

    public async Task<long> PublishAsync(string message)
    {
        var connection = await ConnectionMultiplexer.ConnectAsync(options.Value.Configuration);
        var subscriber = connection.GetSubscriber();
        return await subscriber.PublishAsync(RedisChannel.Pattern(options.Value.Channel), message, options.Value.CommandFlags);
    }

    public async Task PublishAsync(string[] messages)
    {
        var connection = await ConnectionMultiplexer.ConnectAsync(options.Value.Configuration);
        var subscriber = connection.GetSubscriber();
        foreach (var message in messages)
            await subscriber.PublishAsync(RedisChannel.Pattern(options.Value.Channel), message, options.Value.CommandFlags);
    }
}
