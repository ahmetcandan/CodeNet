using CodeNet.EventBus.Services;

namespace CodeNet.EventBus.Settings;

public class EventBusPublisherOptions : BaseEventBusOptions
{
}

public class EventBusPublisherOptions<TPublisherService> : EventBusPublisherOptions
    where TPublisherService : EventBusPublisherService
{
}
