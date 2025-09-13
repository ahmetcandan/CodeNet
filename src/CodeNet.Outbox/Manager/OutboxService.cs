using CodeNet.Outbox.Models;
using CodeNet.Outbox.Repositories;
using CodeNet.Outbox.Settings;
using Microsoft.Extensions.Options;

namespace CodeNet.Outbox.Manager;

internal class OutboxService(OutboxRepository outboxRepository, IOptions<OutboxSettings> options) : IOutboxService
{
    public async Task<IEnumerable<MessageModel>> GetWaitingMessagesAsync(string queueName, CancellationToken cancellationToken = default)
    {
        var response = await outboxRepository.GetPagingListAsync(c => c.QueueName.Equals(queueName), c => c.CreatedAt, true, 1, options.Value.PrefetchCount, cancellationToken);

        return response.Select(c => new MessageModel
        {
            CreatedAt = c.CreatedAt,
            MessageId = c.MessageId,
            Data = c.Data,
            QueueName = c.QueueName,
            Id = c.Id
        });
    }

    public Task<MessageModel> AddMessageAsync(CreateMessageModel model, CancellationToken cancellationToken = default)
    {
        return AddMessageAsync(model, Guid.NewGuid().ToString("N"), cancellationToken);
    }

    public async Task<MessageModel> AddMessageAsync(CreateMessageModel model, string messageId, CancellationToken cancellationToken = default)
    {
        var response = await outboxRepository.CreateAsync(new OutboxModel
        {
            MessageId = messageId,
            CreatedAt = DateTime.UtcNow,
            Data = model.Data,
            QueueName = model.QueueName
        }, cancellationToken);

        return new MessageModel
        {
            CreatedAt = response.CreatedAt,
            MessageId = response.MessageId,
            Data = response.Data,
            QueueName = response.QueueName
        };
    }

    public IEnumerable<MessageModel> GetWaitingMessages(string queueName)
    {
        var response = outboxRepository.GetPagingListAsync(c => c.QueueName.Equals(queueName), c => c.CreatedAt, true, 0, options.Value.PrefetchCount, CancellationToken.None).Result;

        return response.Select(c => new MessageModel
        {
            CreatedAt = c.CreatedAt,
            MessageId = c.MessageId,
            Data = c.Data,
            QueueName = c.QueueName,
            Id = c.Id
        });
    }

    public MessageModel AddMessage(CreateMessageModel model)
    {
        return AddMessage(model, Guid.NewGuid().ToString("N"));
    }

    public MessageModel AddMessage(CreateMessageModel model, string messageId)
    {
        var response = outboxRepository.Create(new OutboxModel
        {
            MessageId = messageId,
            CreatedAt = DateTime.UtcNow,
            Data = model.Data,
            QueueName = model.QueueName,
            Id = Guid.NewGuid()
        });

        return new MessageModel
        {
            CreatedAt = response.CreatedAt,
            MessageId = response.MessageId,
            Data = response.Data,
            QueueName = response.QueueName,
            Id = response.Id
        };
    }

    public void DeleteMessage(Guid id)
    {
        outboxRepository.Delete(c => c.Id == id);
    }

    public void DeleteMessage(IEnumerable<Guid> ids)
    {
        outboxRepository.Delete(c => ids.Contains(c.Id));
    }

    public Task DeleteMessageAsync(Guid id, CancellationToken cancellationToken)
    {
        return outboxRepository.DeleteAsync(c => c.Id == id, cancellationToken);
    }

    public Task DeleteMessageAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        return outboxRepository.DeleteAsync(c => ids.Contains(c.Id), cancellationToken);
    }
}
