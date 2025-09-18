namespace CodeNet.Transport.Client;

internal class ClientItemCollection
{
    private const int _defaultLength = 4;

    public ClientItemCollection() { }

    public ClientItemCollection(IEnumerable<ClientItem> clients)
    {
        Clients = clients;
    }

    public IEnumerable<ClientItem> Clients { get; set; } = [];

    public static byte[] SerializeObject(ClientItemCollection obj)
    {
        if (obj is null)
            return [];

        var bytes = new List<byte>();
        bytes.AddRange(BitConverter.GetBytes(obj.Clients.Count()));
        foreach (var client in obj.Clients)
        {
            var clientData = ClientItem.SerializeObject(client);
            bytes.AddRange(BitConverter.GetBytes(clientData.Length));
            bytes.AddRange(clientData);
        }


        return [.. bytes];
    }

    public static ClientItemCollection DeserializeObject(byte[] bytes)
    {
        var result = new ClientItemCollection();
        var clientsCount = BitConverter.ToInt32(bytes.AsSpan(0, 4));
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
