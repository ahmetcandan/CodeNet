using CodeNet.EventBus.Services;

namespace CodeNet.EventBus.Settings;

public class EventBusSubscriberOptions : BaseEventBusOptions
{
    public string? ConsumerGroup { get; set; }
}

public class EventBusSubscriberOptions<TSubscriberService> : EventBusSubscriberOptions
    where TSubscriberService : EventBusSubscriberService
{
}
