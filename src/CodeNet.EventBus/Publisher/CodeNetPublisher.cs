using CodeNet.EventBus.Client;
using CodeNet.EventBus.Models;

namespace CodeNet.EventBus.Publisher;

public class CodeNetPublisher(string hostname, int port, string channel)
{
    private readonly CodeNetEventBusClient _client = new(hostname, port, ClientType.Publisher);

    public async Task<bool> ConnectAsync()
    {
        var result = await _client.ConnectAsync();
        ConnectProcess();
        return result;
    }

    public bool Connected { get { return _client.Working; } }

    public bool Connect()
    {
        var result = _client.Connect();
        ConnectProcess();
        return result;
    }

    private void ConnectProcess()
    {
        _client.SetClientType(ClientType.Publisher);
        _client.SetChannel(channel);
    }

    public void Disconnect()
    {
        _client?.Disconnect();
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
