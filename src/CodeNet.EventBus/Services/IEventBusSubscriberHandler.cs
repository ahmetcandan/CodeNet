namespace CodeNet.EventBus.Services;

public interface IEventBusSubscriberHandler<TSubscriberService>
    where TSubscriberService : EventBusSubscriberService
{
    void Handler(ReceivedMessageEventArgs args);
}
