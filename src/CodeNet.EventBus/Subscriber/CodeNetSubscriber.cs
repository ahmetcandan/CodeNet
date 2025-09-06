using CodeNet.EventBus.Client;
using CodeNet.EventBus.EventDefinitions;
using CodeNet.EventBus.Models;
using CodeNet.Socket.EventDefinitions;

namespace CodeNet.EventBus.Subscriber;

public class CodeNetSubscriber(string hostname, int port, string channel)
{
    public CodeNetSubscriber(string hostname, int port, string channel, string consumerGroup) : this(hostname, port, channel) => ConsumerGroup = consumerGroup;

    private readonly CodeNetEventBusClient _client = new(hostname, port, ClientType.Subscriber);

    public string ConsumerGroup { get; } = string.Empty;

    public bool Connected { get { return _client.Working; } }

    public event MessageConsumed? MessageConsumed;

    public async Task<bool> ConnectAsync()
    {
        var result = await _client.ConnectAsync();
        if (result is false)
            return false;

        ConnectProcess();
        return result;
    }

    public bool Connect()
    {
        var result = _client.Connect();
        if (result is false)
            return false;

        ConnectProcess();
        return result;
    }

    private void ConnectProcess()
    {
        _client.NewMessgeReceived += Client_NewMessgeReceived;
        _client.SetClientType(ClientType.Subscriber);
        if (!string.IsNullOrWhiteSpace(ConsumerGroup))
            _client.SetConsumerGroup(ConsumerGroup);
        _client.SetChannel(channel);
    }

    public void Disconnect() => _client?.Disconnect();

    private void Client_NewMessgeReceived(MessageReceivingArguments e) => MessageConsumed?.Invoke(new(e.Message.Data, ConsumerGroup));
}
