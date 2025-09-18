using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using CodeNet.Socket.Server;
using CodeNet.Transport.Client;
using CodeNet.Transport.Helper;
using CodeNet.Transport.Models;

namespace CodeNet.Transport.Server;

internal class DataTransferServerItem(int port, bool withSecurity = false) : CodeNetServer<DataTransferClientItem>(port)
{
    public bool SecurityConnection { get; } = withSecurity;

    public event ClientConnectFinish<DataTransferClientItem>? ClientConnectFinish;

    public override string ApplicationKey => TransportKey.ApplicationKey;

    protected override void ReceivedMessage(DataTransferClientItem client, Message message)
    {
        DataTransferClientItem? receiveClient;
        switch (message.Type)
        {
            case (byte)Models.MessageType.ClientConfirmation:
                foreach (var _client in Clients.Where(c => c.ClientId != client.ClientId))
                    _client.SendMessage(new((byte)Models.MessageType.ClienList, SerializerHelper.SerializeObject(SendToClientList(_client))));
                ClientConnectFinish?.Invoke(new(client));
                return;
            case (byte)Models.MessageType.Message:
                if (message.Data is null)
                    return;

                var receiveMessage = SendDataMessage.DeserializeObject(message.Data);
                if (receiveMessage is null)
                    return;

                receiveClient = Clients.FirstOrDefault(c => c.ClientId == receiveMessage.ClientId);
                if (receiveClient is null)
                    return;

                receiveClient.SendMessage(new((byte)Models.MessageType.Message, SendDataMessage.SerializeObject(new SendDataMessage
                {
                    ClientId = client.ClientId,
                    Data = receiveMessage.Data,
                })));
                return;
            case (byte)Models.MessageType.ShareAESKey:
                if (message.Data is null)
                    return;

                var handshakeMessage = HandshakeMessage.DeserializeObject(message.Data);
                if (handshakeMessage is null)
                    return;

                receiveClient = Clients.FirstOrDefault(c => c.ClientId == handshakeMessage.ClientId);
                if (receiveClient is null)
                    return;

                receiveClient.SendMessage(new((byte)Models.MessageType.ShareAESKey, HandshakeMessage.SerializeObject(new HandshakeMessage
                {
                    ClientId = client.ClientId,
                    EncryptedAESKey = handshakeMessage.EncryptedAESKey
                })));
                return;
        }
    }

    protected override void ClientConnecting(DataTransferClientItem client)
    {
        client.SendMessage(new((byte)Models.MessageType.ServerConfirmation, ServerConfirmationMessage.SerializeObject(new ServerConfirmationMessage
        {
            UseSecurity = SecurityConnection,
            Clients = SendToClientList(client)
        })));

        foreach (var _client in Clients.Where(c => c.ClientId != client.ClientId))
            _client.SendMessage(new((byte)Models.MessageType.ClienList, SerializerHelper.SerializeObject(SendToClientList(_client))));
    }

    private IEnumerable<ClientItem> SendToClientList(DataTransferClientItem client) => Clients.Where(c => c.ClientId != client.ClientId).Select(c => new ClientItem
    {
        Id = c.ClientId,
        Name = c.ClientName,
        RSAPublicKey = c.PublicKey
    });
}
