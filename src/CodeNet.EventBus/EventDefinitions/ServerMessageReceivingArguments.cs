using CodeNet.EventBus.Client;
using CodeNet.EventBus.Models;

namespace CodeNet.EventBus.EventDefinitions;

public class ServerMessageReceivingArguments<TClient>(TClient client, Message message)
    where TClient : CodeNetClient
{
    public TClient Client { get; } = client;
    public Message Message { get; } = message;
    public DateTime Date { get; } = DateTime.Now;
}
