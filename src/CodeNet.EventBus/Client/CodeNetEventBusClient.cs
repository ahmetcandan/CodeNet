using CodeNet.EventBus.Models;
using CodeNet.Socket.Client;
using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using System.Net.Sockets;
using System.Text;

namespace CodeNet.EventBus.Client;

internal class CodeNetEventBusClient : CodeNetClient
{
    private ClientType _clientType;
    private string? _consumerGroup;
    private string _channel;

    public event ClientConnectFinish<CodeNetEventBusClient>? ClientConnectFinish;

    public ClientType ClientType { get { return _clientType; } internal set { _clientType = value; } }
    public string? ConsumerGroup { get { return _consumerGroup; } internal set { _consumerGroup = value; } }
    public string Channel { get { return _channel; } internal set { _channel = value; } }

    public bool SetClientType(ClientType clientType)
    {
        ClientType = clientType;
        return SendMessage(new()
        {
            Type = (byte)Models.MessageType.SetClientType,
            Data = [(byte)clientType]
        });
    }

    public bool SetConsumerGroup(string consumerGroup)
    {
        ConsumerGroup = consumerGroup;
        return SendMessage(new()
        {
            Type = (byte)Models.MessageType.SetConsumerGroup,
            Data = Encoding.UTF8.GetBytes(consumerGroup)
        });
    }

    public bool SetChannel(string channel)
    {
        Channel = channel;
        return SendMessage(new()
        {
            Type = (byte)Models.MessageType.SetChannel,
            Data = Encoding.UTF8.GetBytes(channel)
        });
    }

    public override string ApplicationKey => EventBusKey.ApplicationKey;

    internal CodeNetEventBusClient(TcpClient client, ulong clientId, ClientType clientType) : base()
    {
        SetTcpClient(client, clientId);
        _clientType = clientType;
        _channel = string.Empty;
    }

    internal CodeNetEventBusClient(string hostName, int port, ClientType clientType) : base(hostName, port)
    {
        _clientType = clientType;
        _channel = string.Empty;
    }

    public CodeNetEventBusClient()
    {
        _channel = string.Empty;
    }

    protected override void ReceivedMessage(Message message)
    {
        switch (message.Type)
        {
            case (byte)Models.MessageType.Publish:
            case (byte)Models.MessageType.Message:
                base.ReceivedMessage(message);
                return;
            case (byte)Models.MessageType.SetClientType:
                ClientType = (ClientType)message.Data[0];
                return;
            case (byte)Models.MessageType.SetConsumerGroup:
                ConsumerGroup = Encoding.UTF8.GetString(message.Data);
                return;
            case (byte)Models.MessageType.SetChannel:
                Channel = Encoding.UTF8.GetString(message.Data);
                ClientConnectFinish?.Invoke(new(this));
                return;
            case (byte)Models.MessageType.None:
            default:
                return;
        }
    }
}
