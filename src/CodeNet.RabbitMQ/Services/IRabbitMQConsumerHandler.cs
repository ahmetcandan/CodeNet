using CodeNet.RabbitMQ.Models;

namespace CodeNet.RabbitMQ.Services;

public interface IRabbitMQConsumerHandler<TConsumerService>
    where TConsumerService : RabbitMQConsumerService
{
    Task Handler(ReceivedMessageEventArgs args);
}
