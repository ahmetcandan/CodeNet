using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using System.Net.Sockets;
using System.Text;

namespace CodeNet.Socket.Client;

public class CodeNetClient : ICodeNetClient
{
    private readonly string? _hostName;
    private readonly int? _port;

    private TcpClient? _client;
    private int _clientId = 0;
    private bool _working = false;
    private Thread? _thread;
    private NetworkStream? _stream;
    private BinaryReader? _reader;
    private BinaryWriter? _writer;

    public void Dispose()
    {
        Disconnect(false);
        GC.SuppressFinalize(this);
    }

    public bool Working { get { return _working; } }
    public int ClientId { get { return _clientId; } }

    public event NewMessageReceived? NewMessgeReceived;
    public event ClientDisconnected<CodeNetClient>? Disconnected;

    public CodeNetClient()
    {
    }

    public CodeNetClient(string hostName, int port)
    {
        _hostName = hostName;
        _port = port;
        _client = new(hostName, port);
        _clientId = -1;
    }

    public void SetTcpClient(TcpClient client, int clientId)
    {
        _client = client;
        _clientId = clientId;
        Start();
    }

    public virtual async Task<bool> ConnectAsync()
    {
        if (_client is not null)
        {
            if (_clientId > 0)
                await _client.ConnectAsync(_hostName ?? string.Empty, _port ?? 0);

            Start();
            return true;
        }

        return false;
    }

    public virtual bool Connect()
    {
        if (_client is not null)
        {
            if (_clientId > 0)
                _client.Connect(_hostName ?? string.Empty, _port ?? 0);

            Start();
            return true;
        }

        return false;
    }

    private void Start()
    {
        _stream = _client!.GetStream();
        _reader = new BinaryReader(_stream, Encoding.BigEndianUnicode);
        _writer = new BinaryWriter(_stream, Encoding.BigEndianUnicode);
        _thread = new(new ThreadStart(StartListening));
        _working = true;
        _thread.Start();
    }

    public void Disconnect()
    {
        Disconnect(true);
    }

    private void Disconnect(bool notify)
    {
        if (notify)
            SendMessage(new() { Type = (byte)MessageType.Disconnected, Data = [] });

        _client?.Close();
        _working = false;
        _thread?.Join();
        Disconnected?.Invoke(new(this));
    }

    private async void StartListening()
    {
        while (_working)
        {
            try
            {
                byte[] buffer = new byte[5];
                _reader?.Read(buffer, 0, 5);

                if (buffer[0] == 0)
                    continue;

                int length = BitConverter.ToInt32(buffer, 1);
                if (length > 0)
                {
                    byte[] message = new byte[length];
                    _reader?.Read(message, 0, length);
                    ReceivedMessage(Message.Deseriliaze(buffer[0], message));
                }
                else
                    ReceivedMessage(Message.Deseriliaze(buffer[0], []));
            }
            catch
            {
                Disconnect(_clientId == 0);
            }
        }
    }

    protected internal virtual void ReceivedMessage(Message message)
    {
        if (message.Type is (byte)MessageType.Disconnected)
        {
            Disconnect(false);
            return;
        }

        NewMessgeReceived?.Invoke(new(message));
    }

    public bool SendMessage(Message message)
    {
        if (_working is false || !(_writer?.BaseStream.CanWrite ?? false))
            return false;

        _writer?.Write(message.Seriliaze());
        return true;
    }
}
