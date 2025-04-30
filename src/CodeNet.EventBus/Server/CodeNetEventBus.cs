using CodeNet.EventBus.Client;
using CodeNet.EventBus.Models;
using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;

namespace CodeNet.EventBus.Server;

public class CodeNetEventBus(int port)
{
    private readonly CodeNetEventBusServer _server = new(port);
    private readonly Dictionary<string, List<ConsumerGroup>> _channels = [];
    private readonly Dictionary<ulong, CodeNetEventBusClient> _clients = [];
    private readonly Dictionary<ChannelToConsumerGroup, int> _consumerGroupIndex = [];

    public void Start()
    {
        _server.Start();
        _server.NewMessgeReceived += Server_NewMessgeReceived;
        _server.ClientConnected += Server_ClientConnected;
        _server.ClientDisconnected += Server_ClientDisconnected;
        _server.ClientConnectFinish += Server_ClientConnectedFinish;
    }

    public void Stop()
    {
        _server.Stop();
        _server.NewMessgeReceived -= Server_NewMessgeReceived;
        _server.ClientConnected -= Server_ClientConnected;
        _server.ClientDisconnected -= Server_ClientDisconnected;
        _server.ClientConnectFinish -= Server_ClientConnectedFinish;
    }

    private void Server_NewMessgeReceived(ServerMessageReceivingArguments<CodeNetEventBusClient> e)
    {
        if (e.Client.ClientType is not ClientType.Publisher || e.Message.Type is not (byte)Models.MessageType.Publish)
            return;

        PublishMessageToSubscribers(e.Message, e.Client.Channel);
    }

    private void Server_ClientConnected(ClientArguments<CodeNetEventBusClient> e)
    {
        _clients.Add(e.Client.ClientId, e.Client);
    }

    private void Server_ClientConnectedFinish(ClientArguments<CodeNetEventBusClient> e)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Client (ClientId: {e.Client.ClientId}, ClientType: {e.Client.ClientType}, ConsumerGroup: {e.Client.ConsumerGroup}) connected");
        if (e.Client.ClientType is ClientType.Subscriber)
        {
            if (!_channels.ContainsKey(e.Client.Channel))
                _channels.Add(e.Client.Channel, [new ConsumerGroup() { Name = null, ClientIds = [] }]);

            if (string.IsNullOrWhiteSpace(e.Client.ConsumerGroup))
                _channels[e.Client.Channel].First(c => c.Name is null).ClientIds.Add(e.Client.ClientId);
            else
            {
                var consumerGroup = _channels[e.Client.Channel].FirstOrDefault(c => c.Name == e.Client.ConsumerGroup);
                if (consumerGroup is not null)
                    consumerGroup.ClientIds.Add(e.Client.ClientId);
                else
                {
                    consumerGroup = new()
                    {
                        Name = e.Client.ConsumerGroup,
                        ClientIds = [e.Client.ClientId]
                    };
                    _channels[e.Client.Channel].Add(consumerGroup);
                    _consumerGroupIndex.Add(new(e.Client.Channel, e.Client.ConsumerGroup), 0);
                }
            }
        }
    }

    private void Server_ClientDisconnected(ClientArguments<CodeNetEventBusClient> e)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Client (ClientId: {e.Client.ClientId}, ClientType: {e.Client.ClientType}) disconnected");
        _clients.Remove(e.Client.ClientId);

        if (e.Client.ClientType is ClientType.Subscriber)
        {
            if (string.IsNullOrWhiteSpace(e.Client.ConsumerGroup))
            {
                var consumerGroup = _channels[e.Client.Channel].FirstOrDefault(c => c.Name is null && c.ClientIds.Contains(e.Client.ClientId));
                if (consumerGroup is not null)
                    _channels[e.Client.Channel].Remove(consumerGroup);
            }
            else
            {
                var consumerGroup = _channels[e.Client.Channel].FirstOrDefault(c => c.Name == e.Client.ConsumerGroup && c.ClientIds.Contains(e.Client.ClientId));
                if (consumerGroup is not null)
                {
                    consumerGroup.ClientIds.Remove(e.Client.ClientId);
                    if (consumerGroup.ClientIds.Count == 0)
                    {
                        _channels[e.Client.Channel].Remove(consumerGroup);
                        _consumerGroupIndex.Remove(new(e.Client.Channel, e.Client.ConsumerGroup));
                    }
                }
            }
        }
    }

    private void PublishMessageToSubscribers(Message message, string channel)
    {
        foreach (var consumerGroup in _channels[channel])
        {
            if (consumerGroup.Name is null)
                foreach (var clientId in consumerGroup.ClientIds)
                    ClientToMessage(clientId, message);

            else
            {
                var group = new ChannelToConsumerGroup(channel, consumerGroup.Name!);
                var index = _consumerGroupIndex[group];
                ClientToMessage(consumerGroup.ClientIds[index], message);
                index = index >= (consumerGroup.ClientIds.Count - 1) ? 0 : index + 1;
                _consumerGroupIndex[group] = index;
            }
        }
    }

    private void ClientToMessage(ulong clientId, Message message) => _clients[clientId]?.SendMessage(message);

    class ConsumerGroup
    {
        public string? Name { get; set; }
        public List<ulong> ClientIds { get; set; } = [];
    }

    struct ChannelToConsumerGroup(string channel, string consumerGroup)
    {
        public string Channel { get; set; } = channel;
        public string ConsumerGroup { get; set; } = consumerGroup;
    }
}
