using CodeNet.Transport.Client;

namespace CodeNet.Transport.Models;

internal class ServerConfirmationMessage
{
    private const int _defaultLength = 5;

    public bool UseSecurity { get; set; }
    public IEnumerable<ClientItem> Clients { get; set; } = [];

    public static byte[] SerializeObject(ServerConfirmationMessage message)
    {
        if (message is null)
            return [];

        var bytes = new List<byte>
        {
            message.UseSecurity ? (byte)1 : (byte)0
        };
        bytes.AddRange(BitConverter.GetBytes(message.Clients.Count()));
        foreach (var client in message.Clients)
        {
            var clientData = ClientItem.SerializeObject(client);
            bytes.AddRange(BitConverter.GetBytes(clientData.Length));
            bytes.AddRange(clientData);
        }


        return [.. bytes];
    }

    public static ServerConfirmationMessage DeserializeObject(byte[] bytes)
    {
        var result = new ServerConfirmationMessage()
        {
            UseSecurity = bytes[0] == 1
        };
        var clientsCount = BitConverter.ToInt32(bytes.AsSpan(1, 4));
        int cursor = _defaultLength;
        var clients = new List<ClientItem>();
        for (int i = 0; i < clientsCount; i++)
        {
            var clientDataLength = BitConverter.ToInt32(bytes.AsSpan(cursor, 4));
            cursor += 4;
            clients.Add(ClientItem.DeserializeObject(bytes.AsSpan(cursor, clientDataLength).ToArray()));
            cursor += clientDataLength;
        }
        result.Clients = clients;
        return result;
    }
}
