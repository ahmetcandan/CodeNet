using CodeNet.Socket.EventDefinitions;
using CodeNet.Transport.Client;
using CodeNet.Transport.EventDefinitions;

namespace CodeNet.Transport.Server;

public class DataTransferServer(int port, bool withSecurity = false)
{
    private readonly DataTransferServerItem _server = new(port, withSecurity);

    public event ClientConnected? ClientConnected;
    public event ClientDisconnected? ClientDisconnected;

    public void Start()
    {
        _server.ClientConnectFinish += OnClientConnectFinish;
        _server.ClientDisconnected += OnClientDisconnected;

        _server.Start();
    }

    private void OnClientDisconnected(ClientArguments<DataTransferClientItem> e)
    {
        ClientDisconnected?.Invoke(new() { ClientName = e.Client.ClientName });
        e.Client.Dispose();
    }

    private void OnClientConnectFinish(ClientArguments<DataTransferClientItem> e)
    {
        ClientConnected?.Invoke(new() { ClientName = e.Client.ClientName });
    }

    public void Stop()
    {
        _server.ClientConnectFinish -= OnClientConnectFinish;
        _server.ClientDisconnected -= OnClientDisconnected;

        _server.Stop();
    }
}
