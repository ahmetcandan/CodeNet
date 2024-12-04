using CodeNet.EventBus.Models;

namespace CodeNet.EventBus.EventDefinitions;

public class MessageReceivingArguments(Message message)
{
    public Message Message { get; } = message;
    public DateTime Date { get; } = DateTime.Now;
}
