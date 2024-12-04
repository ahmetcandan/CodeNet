using CodeNet.EventBus.Client;
using CodeNet.EventBus.EventDefinitions;
using CodeNet.EventBus.Models;

namespace CodeNet.EventBus.Subscriber;

public class CodeNetSubscriber(string hostname, int port)
{
    public CodeNetSubscriber(string hostname, int port, string consumerGroup) : this(hostname, port)
    {
        _consumerGroup = consumerGroup;
    }

    private readonly CodeNetEventBusClient _client = new(hostname, port, ClientType.Subscriber);
    private readonly string _consumerGroup = string.Empty;

    public string ConsumerGroup { get { return _consumerGroup; } }

    public event MessageConsumed? MessageConsumed;

    public async Task<bool> ConnectAsync()
    {
        var result = await _client.ConnectAsync();
        if (result is false)
            return false;

        _client.NewMessgeReceived += Client_NewMessgeReceived;
        _client.SetClientType(ClientType.Subscriber);
        if (!string.IsNullOrWhiteSpace(ConsumerGroup))
            _client.SetConsumerGroup(ConsumerGroup);

        return result;
    }

    public bool Connect()
    {
        var result = _client.Connect();
        _client.NewMessgeReceived += Client_NewMessgeReceived;
        _client.SetClientType(ClientType.Subscriber);
        return result;
    }

    public void Disconnect()
    {
        _client?.Disconnect();
    }

    private void Client_NewMessgeReceived(MessageReceivingArguments e)
    {
        MessageConsumed?.Invoke(new(e.Message.Data, ConsumerGroup));
    }
}
