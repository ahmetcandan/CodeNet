using CodeNet.StackExchange.Redis.Models;

namespace CodeNet.StackExchange.Redis.Services;

public interface IStackExchangeConsumerHandler<TConsumerService>
    where TConsumerService : StackExchangeConsumerService
{
    Task Handler(ReceivedMessageEventArgs args);
}
