namespace CodeNet.EventBus.Services;

public interface IEventBusSubscriberHandler<TSubscriberService>
    where TSubscriberService : EventBusSubscriberService
{
    Task Handler(ReceivedMessageEventArgs args);
}
