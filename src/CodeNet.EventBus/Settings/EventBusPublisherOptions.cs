namespace CodeNet.EventBus.Settings;

public class EventBusPublisherOptions : BaseEventBusOptions
{
}

public class RabbitMQProducerOptions<TPublisherService> : EventBusPublisherOptions
    where TPublisherService : EventBusPublisherService
{
    public override string ToString()
    {
        return base.ToString();
    }
}
