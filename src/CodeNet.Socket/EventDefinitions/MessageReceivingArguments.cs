using CodeNet.Socket.Models;

namespace CodeNet.Socket.EventDefinitions;

public class MessageReceivingArguments(Message message)
{
    public Message Message { get; } = message;
    public DateTime Date { get; } = DateTime.Now;
}
