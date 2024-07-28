using CodeNet.StackExchange.Redis.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using CodeNet.StackExchange.Redis.Models;

namespace CodeNet.StackExchange.Redis.Services;

public class StackExchangeConsumerService(IOptions<StackExchangeConsumerOptions> options)
{
    public async Task StartListening()
    {
        var connection = await ConnectionMultiplexer.ConnectAsync(options.Value.Configuration);
        var subscriber = connection.GetSubscriber();

        await subscriber.SubscribeAsync(RedisChannel.Pattern(options.Value.Channel), (channel, message) =>
        {
            ReceivedMessage?.Invoke(new ReceivedMessageEventArgs
            {
                Message = message.ToString(),
                Channel = channel.ToString()
            });
        });
    }

    public event MessageReceived? ReceivedMessage;
}
