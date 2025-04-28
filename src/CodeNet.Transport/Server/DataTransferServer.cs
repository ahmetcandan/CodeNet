using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using CodeNet.Socket.Server;
using CodeNet.Transport.Client;
using CodeNet.Transport.Helper;
namespace CodeNet.Transport.Server;

public class DataTransferServer(int port, bool withSecurity = false) : CodeNetServer<DataTransferClient>(port)
{
    private readonly bool _securityConnection = withSecurity;

    public bool SecurityConnection { get { return _securityConnection; } }

    public event ClientConnectFinish<DataTransferClient>? ClientConnectFinish;

    protected override void ReceivedMessage(DataTransferClient client, Message message)
    {
        ConsoleLog($"Client {client.ClientId} sent message: {message.Type} ({message.Data?.Length})");
        if (message.Type is (byte)Models.MessageType.Connected)
            ClientConnectFinish?.Invoke(new(client));
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

        ConsoleLog($"Client {client.ClientId} Send Message: Connected");
        client.SendMessage(new()
        {
            Type = (byte)Models.MessageType.Connected,
            Data = [1]
        });

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
