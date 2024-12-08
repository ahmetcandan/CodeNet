using CodeNet.EventBus.Publisher;
using CodeNet.EventBus.Settings;
using Microsoft.Extensions.Options;

namespace CodeNet.EventBus.Services;

public class EventBusPublisherService(IOptions<EventBusPublisherOptions> options) : IDisposable
{
    private CodeNetPublisher? _publisher;

    public void Dispose()
    {
        _publisher?.Disconnect();
        GC.SuppressFinalize(this);
    }

    public virtual void Publish(byte[] message)
    {
        _publisher ??= new(options.Value.HostName, options.Value.Port, options.Value.Channel);

        if (!_publisher.Connected)
            _publisher.Connect();

        _publisher.Publish(message);
    }
    public virtual void Publish(IEnumerable<byte[]> messages)
    {
        _publisher ??= new(options.Value.HostName, options.Value.Port, options.Value.Channel);

        if (!_publisher.Connected)
            _publisher.Connect();

        foreach (var message in messages)
            _publisher.Publish(message);
    }
}
