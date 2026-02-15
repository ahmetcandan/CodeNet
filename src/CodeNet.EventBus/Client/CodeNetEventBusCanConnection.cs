namespace CodeNet.EventBus.Client;

public class CodeNetEventBusCanConnection(string hostName, int port)
{
    private readonly CodeNetEventBusClient _client = new(hostName, port, Models.ClientType.None);

    public Task<bool> CanConnectionAsync() => _client.CanConnectionAsync();

    public bool CanConnection() => _client.CanConnection();

    public void Disconnect() => _client?.Disconnect();
}
