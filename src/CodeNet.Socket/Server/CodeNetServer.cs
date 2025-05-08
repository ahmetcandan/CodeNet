using System.Net.Sockets;
using System.Net;
using CodeNet.Socket.Client;
using CodeNet.Socket.Models;
using CodeNet.Socket.EventDefinitions;
using System.Text;

namespace CodeNet.Socket.Server;

public abstract class CodeNetServer<TClient>(int port) : IDisposable
    where TClient : CodeNetClient, new()
{
    private readonly List<TClient> _clients = [];
    private readonly int _port = port;
    TcpListener? _tcpListener;
    private TcpStatus _status = TcpStatus.Stop;
    private ulong _lastClientId = 0;
    private Thread? _thread;

    public event ClientConnected<TClient>? ClientConnected;
    public event ServerNewMessageReceived<TClient>? NewMessgeReceived;
    public event ClientDisconnected<TClient>? ClientDisconnected;

    public abstract string ApplicationKey { get; }

    public TcpStatus Status { get { return _status; } }
    public List<TClient> Clients { get { return _clients; } }

    public void Start()
    {
        _tcpListener = new TcpListener(IPAddress.Any, _port);
        _tcpListener.Start();

        _thread = new Thread(new ThreadStart(ClientAccept));
        _thread.Start();

        _status = TcpStatus.Starting;
    }

    public void Stop()
    {
        if (_status is TcpStatus.Starting)
        {
            _tcpListener?.Stop();
            _status = TcpStatus.Stop;
        }
        _thread?.Join();
    }

    private async void ClientAccept()
    {
        while (_status is TcpStatus.Starting)
        {
            try
            {
                if (_tcpListener is not null)
                {
                    var tcpClient = await _tcpListener.AcceptTcpClientAsync();
                    if (tcpClient is not null)
                    {
                        TClient client = new();
                        client.SetTcpClient(tcpClient, ++_lastClientId);
                        client.NewMessgeReceived += (e) => Client_NewMessgeReceived(client, e);
                        client.Disconnected += (e) => Client_Disonnected(client);
                        _clients.Add(client);
                        Client_Connected(client);
                    }
                }
                else
                    Stop();
            }
            catch
            {
                Stop();
            }
        }
    }

    private void Client_Connected(TClient client)
    {
        ClientConnecting(client);
        ClientConnected?.Invoke(new(client));
    }

    protected internal virtual void ClientConnecting(TClient client)
    {
    }

    private readonly object _clientsLock = new();

    private void Client_Disonnected(TClient client)
    {
        lock (_clientsLock)
        {
            var _client = Clients.FirstOrDefault(c => c.ClientId == client.ClientId);
            if (_client != null)
            {
                Clients.Remove(_client);
            }
        }
        ClientDisconnecting(client);
        ClientDisconnected?.Invoke(new(client));
        client.Dispose();
    }

    internal virtual void ClientDisconnecting(TClient client)
    {
    }

    private void Client_NewMessgeReceived(TClient client, MessageReceivingArguments e)
    {
        if (e.Message.Type is (byte)MessageType.Validation && !Encoding.UTF8.GetString(e.Message.Data).Equals(ApplicationKey) && Clients.Exists(c => c.ClientId == client.ClientId))
        {
            lock (_clientsLock)
            {
                var _client = Clients.FirstOrDefault(c => c.ClientId == client.ClientId);
                if (_client != null)
                {
                    _client.Disconnect();
                    Clients.Remove(_client);
                }
            }
        }

        ReceivedMessage(client, e.Message);
        NewMessgeReceived?.Invoke(new(client, e.Message));
    }

    protected internal virtual void ReceivedMessage(TClient client, Message message)
    {
    }

    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }
}
