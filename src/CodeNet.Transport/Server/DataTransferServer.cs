using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using CodeNet.Socket.Server;
using CodeNet.Transport.Client;
using CodeNet.Transport.Helper;
using CodeNet.Transport.Models;
using System.Text;
namespace CodeNet.Transport.Server;

public class DataTransferServer(int port, bool withSecurity = false) : CodeNetServer<DataTransferClient>(port)
{
    private readonly bool _securityConnection = withSecurity;

    public bool SecurityConnection { get { return _securityConnection; } }

    public event ClientConnectFinish<DataTransferClient>? ClientConnectFinish;

    protected override void ReceivedMessage(DataTransferClient client, Message message)
    {
        ConsoleLog($"Client {client.ClientId} sent message: {message.Type} ({message.Data?.Length})");
        DataTransferClient? receiveClient;
        switch (message.Type)
        {
            case (byte)Models.MessageType.Connected:
                ConsoleLog("Server received connected.");
                client.SendMessage(new()
                {
                    Type = (byte)Models.MessageType.Connected,
                    Data = [1]
                });
                ClientConnectFinish?.Invoke(new(client));
                return;
            case (byte)Models.MessageType.Message:
                ConsoleLog("Server received Message.");
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
                ConsoleLog("Server received AES key.");
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

    protected override void ClientConnecting(DataTransferClient client)
    {
        ConsoleLog($"Client {client.ClientId} connecting");
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

    private void Client_ClientConnectFinish(ClientArguments<DataTransferClient> e)
    {
        ClientConnectFinish?.Invoke(e);
    }

    private void SendToClientList(DataTransferClient client)
    {
        ConsoleLog($"Sending client list to {client.ClientId}; Total Clients {Clients.Count}; Sendig List Count: {Clients.Count(c => c.ClientId != client.ClientId)}");

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
    private static void ConsoleLog(string log) => Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fffff}] [Server] {log}");
}
