using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using CodeNet.Socket.Server;
using CodeNet.Transport.Client;
using CodeNet.Transport.Helper;
using CodeNet.Transport.Models;

namespace CodeNet.Transport.Server;

class DataTransferServerItem(int port, bool withSecurity = false) : CodeNetServer<DataTransferClientItem>(port)
{
    private readonly bool _securityConnection = withSecurity;

    public bool SecurityConnection { get { return _securityConnection; } }

    public event ClientConnectFinish<DataTransferClientItem>? ClientConnectFinish;

    public override string ApplicationKey => TransportKey.ApplicationKey;

    protected override void ReceivedMessage(DataTransferClientItem client, Message message)
    {
        DataTransferClientItem? receiveClient;
        switch (message.Type)
        {
            case (byte)Models.MessageType.Connected:
                client.SendMessage(new()
                {
                    Type = (byte)Models.MessageType.Connected,
                    Data = [1]
                });
                ClientConnectFinish?.Invoke(new(client));
                return;
            case (byte)Models.MessageType.Message:
                if (message.Data is null)
                    return;

                var receiveMessage = SerializerHelper.DeserializeObject<SendDataMessage>(message.Data);
                if (receiveMessage is null)
                    return;

                receiveClient = Clients.FirstOrDefault(c => c.ClientId == receiveMessage.ClientId);
                if (receiveClient is null)
                    return;

                receiveClient.SendMessage(new()
                {
                    Type = (byte)Models.MessageType.Message,
                    Data = SerializerHelper.SerializeObject(new SendDataMessage
                    {
                        ClientId = client.ClientId,
                        Data = receiveMessage.Data,
                    })
                });
                return;
            case (byte)Models.MessageType.ShareAESKey:
                if (message.Data is null)
                    return;

                var handshakeMessage = SerializerHelper.DeserializeObject<HandshakeMessage>(message.Data);
                if (handshakeMessage is null)
                    return;

                receiveClient = Clients.FirstOrDefault(c => c.ClientId == handshakeMessage.ClientId);
                if (receiveClient is null)
                    return;

                receiveClient.SendMessage(new()
                {
                    Type = (byte)Models.MessageType.ShareAESKey,
                    Data = SerializerHelper.SerializeObject(new HandshakeMessage
                    {
                        ClientId = client.ClientId,
                        EncryptedAESKey = handshakeMessage.EncryptedAESKey
                    })
                });
                return;
            case (byte)Models.MessageType.SharePublicKey:
                foreach (var _client in Clients.Where(c => c.ClientId != client.ClientId))
                    SendToClientList(_client);
                return;
        }
    }

    protected override void ClientConnecting(DataTransferClientItem client)
    {
        client.ClientConnectFinish += Client_ClientConnectFinish;

        client.SendMessage(new()
        {
            Type = (byte)Models.MessageType.UseSecutity,
            Data = SecurityConnection ? [1] : [0]
        });

        SendToClientList(client);

        foreach (var _client in Clients.Where(c => c.ClientId != client.ClientId))
            SendToClientList(_client);
    }

    private void Client_ClientConnectFinish(ClientArguments<DataTransferClientItem> e)
    {
        ClientConnectFinish?.Invoke(e);
    }

    private void SendToClientList(DataTransferClientItem client)
    {
        client.SendMessage(new()
        {
            Type = (byte)Models.MessageType.ClienList,
            Data = SerializerHelper.SerializeObject(Clients.Where(c => c.ClientId != client.ClientId).Select(c => new ClientItem
            {
                Id = c.ClientId,
                Name = c.ClientName,
                RSAPublicKey = c.PublicKey
            }))
        });
    }
}
