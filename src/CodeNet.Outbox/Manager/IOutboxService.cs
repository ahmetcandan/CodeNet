using CodeNet.Outbox.Models;

namespace CodeNet.Outbox.Manager;

public interface IOutboxService
{
    Task<IEnumerable<MessageModel>> GetWaitingMessagesAsync(string queueName, CancellationToken cancellationToken = default);
    IEnumerable<MessageModel> GetWaitingMessages(string queueName);
    Task<MessageModel> AddMessageAsync(CreateMessageModel model, CancellationToken cancellationToken = default);
    Task<MessageModel> AddMessageAsync(CreateMessageModel model, string messageId, CancellationToken cancellationToken = default);
    MessageModel AddMessage(CreateMessageModel model, string messageId);
    MessageModel AddMessage(CreateMessageModel model);
    Task DeleteMessageAsync(Guid id, CancellationToken cancellationToken = default);
    void DeleteMessage(Guid id);
    Task DeleteMessageAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    void DeleteMessage(IEnumerable<Guid> ids);
}