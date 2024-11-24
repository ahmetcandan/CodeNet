using CodeNet.StackExchange.Redis.Models;

namespace CodeNet.StackExchange.Redis.Services;

public interface IStackExchangeSubscribeHandler<TSubscribeService>
    where TSubscribeService : StackExchangeSubscribeService
{
    Task Handler(ReceivedMessageEventArgs args);
}
