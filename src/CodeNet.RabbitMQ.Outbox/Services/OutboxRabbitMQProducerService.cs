using CodeNet.Outbox.Manager;
using CodeNet.Outbox.Models;
using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Services;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;

namespace CodeNet.RabbitMQ.Outbox.Services;

internal class OutboxRabbitMQProducerService<TProducerService>(IOutboxService outboxService, IOptions<RabbitMQProducerOptions<TProducerService>> options) : OutboxRabbitMQProducerService(outboxService, options)
    where TProducerService : RabbitMQProducerService
{
}

internal class OutboxRabbitMQProducerService(IOutboxService outboxService, IOptions<RabbitMQProducerOptions> options) : RabbitMQProducerService(options)
{
    public override IEnumerable<PublishModel> Publish(IEnumerable<PublishModel> messages)
    {
        foreach (var message in messages)
            outboxService.AddMessage(new CreateMessageModel
            {
                Data = message.Data,
                QueueName = QueueName,
            }, string.IsNullOrEmpty(message.MessageId) ? Guid.NewGuid().ToString("N") : message.MessageId);

        return messages;
    }

    public IEnumerable<PublishModel> PublishRange(IEnumerable<PublishModel> messages)
    {
        return base.Publish(messages);
    }
}
