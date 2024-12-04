using CodeNet.EventBus.Client;
using CodeNet.EventBus.Models;

namespace CodeNet.EventBus.Publisher;

public class CodeNetPublisher(string hostname, int port)
{
    private readonly CodeNetEventBusClient _client = new(hostname, port, ClientType.Publisher);

    public async Task<bool> ConnectAsync()
    {
        var result = await _client.ConnectAsync();
        _client.SetClientType(ClientType.Publisher);
        return result;
    }

    public bool Connect()
    {
        var result = _client.Connect(); 
        _client.SetClientType(ClientType.Publisher);
        return result;
    }

    public bool Publish(byte[] message)
    {
        return _client.SendMessage(new()
        {
            Type = MessageType.Publish,
            Data = message
        });
    }
}
