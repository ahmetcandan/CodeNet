using CodeNet.EventBus.Publisher;
using CodeNet.EventBus.Settings;
using Microsoft.Extensions.Options;

namespace CodeNet.EventBus.Services;

public class EventBusPublisherService(IOptions<EventBusPublisherOptions> options)
{
    private CodeNetPublisher? _publisher;

    public virtual void Publish(byte[] message)
    {
        _publisher ??= new(options.Value.HostName, options.Value.Port, options.Value.Channel);

        if (!_publisher.Connected)
            _publisher.Connect();

        _publisher.Publish(message);
    }
}
