using CodeNet.EventBus.EventDefinitions;
using CodeNet.EventBus.Settings;
using CodeNet.EventBus.Subscriber;
using Microsoft.Extensions.Options;

namespace CodeNet.EventBus.Services;

public class EventBusSubscriberService(IOptions<EventBusSubscriberOptions> options)
{
    private CodeNetSubscriber? _subscriber;

    public void StartListening()
    {
        _subscriber = new(options.Value.HostName, options.Value.Port, options.Value.ConsumerGroup);
        _subscriber.Connect();
        _subscriber.MessageConsumed += MessageHandler;
    }

    public event MessageReceived? ReceivedMessage;

    private void MessageHandler(MessageConsumedArguments e) => ReceivedMessage?.Invoke(new() { Message = e.Message });

    public void StopListening()
    {
        _subscriber?.Disconnect();

        if (_subscriber is not null)
            _subscriber.MessageConsumed -= MessageHandler;
    }
}
