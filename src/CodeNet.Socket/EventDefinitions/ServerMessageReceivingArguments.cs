using CodeNet.Socket.Client;
using CodeNet.Socket.Models;

namespace CodeNet.Socket.EventDefinitions;

public class ServerMessageReceivingArguments<TClient>(TClient client, Message message)
    where TClient : CodeNetClient
{
    public TClient Client { get; } = client;
    public Message Message { get; } = message;
    public DateTime Date { get; } = DateTime.Now;
}
