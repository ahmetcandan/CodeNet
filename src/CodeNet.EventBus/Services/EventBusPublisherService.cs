using CodeNet.EventBus.Publisher;
using CodeNet.EventBus.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace CodeNet.EventBus.Services;

public class EventBusPublisherService(IOptions<EventBusPublisherOptions> options) : IDisposable
{
    private CodeNetPublisher? _publisher;

    ~EventBusPublisherService()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _publisher?.Disconnect();
    }

    public virtual void Publish(byte[] message)
    {
        _publisher ??= new(options.Value.HostName, options.Value.Port, options.Value.Channel);

        if (!_publisher.Connected)
            _publisher.Connect();

        _publisher.Publish(message);
    }
    public virtual void Publish(IList<byte[]> messages)
    {
        _publisher ??= new(options.Value.HostName, options.Value.Port, options.Value.Channel);

        if (!_publisher.Connected)
            _publisher.Connect();

        var queue = new ConcurrentQueue<byte[]>(messages);
        Parallel.For(0, messages.Count, _ =>
        {
            while (queue.TryDequeue(out var message))
                _publisher.Publish(message);
        });
    }
}
