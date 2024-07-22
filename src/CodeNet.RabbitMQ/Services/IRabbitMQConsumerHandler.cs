using CodeNet.RabbitMQ.Models;

namespace CodeNet.RabbitMQ.Services;

public interface IRabbitMQConsumerHandler<TConsumerService>
    where TConsumerService : RabbitMQConsumerService
{
    void Handler(ReceivedMessageEventArgs args);
}
