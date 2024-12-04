namespace CodeNet.EventBus.Settings;

public class EventBusSubscriberOptions : BaseEventBusOptions
{
    public string? ConsumerGroup { get; set; }
}

public class RabbitMQConsumerOptions<TSubscriberService> : EventBusSubscriberOptions
    where TSubscriberService : EventBusSubscribeService
{
}
