using CodeNet.StackExchange.Redis.Models;
using CodeNet.StackExchange.Redis.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CodeNet.StackExchange.Redis.Services;

public class StackExchangeConsumerService(IOptions<StackExchangeConsumerOptions> options)
{
    private ConnectionMultiplexer? _connection;
    private ISubscriber? _subscriber;

    public async Task StartListening()
    {
        _connection = await ConnectionMultiplexer.ConnectAsync(options.Value.Configuration);
        _subscriber = _connection.GetSubscriber();

        await _subscriber.SubscribeAsync(RedisChannel.Pattern(options.Value.Channel), MessageHandler);
    }

    public async Task StopListening()
    {
        if (_connection is not null)
            await _connection.CloseAsync();

        if (_subscriber is not null)
            await _subscriber.UnsubscribeAllAsync();
    }

    public event MessageReceived? ReceivedMessage;

    private async void MessageHandler(RedisChannel channel, RedisValue message)
    {
        if (ReceivedMessage is not null)
            await ReceivedMessage.Invoke(new ReceivedMessageEventArgs
            {
                Message = message,
                Channel = channel
            });
    }
}
