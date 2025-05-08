using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using System.Net.Sockets;
using System.Text;

namespace CodeNet.Socket.Client;

public abstract class CodeNetClient : ICodeNetClient
{
    private readonly string? _hostName;
    private readonly int? _port;

    private TcpClient? _client;
    private ulong? _clientId = 0;
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
    public ulong ClientId { get { return _clientId ?? 0; } }
    public bool IsServerSide { get { return _clientId is not null; } }

    public event NewMessageReceived? NewMessgeReceived;
    public event ClientDisconnected<CodeNetClient>? Disconnected;

    public abstract string ApplicationKey { get; }

    public CodeNetClient()
    {
        _clientId = 0;
    }

    public CodeNetClient(string hostName, int port)
    {
        _hostName = hostName;
        _port = port;
        _clientId = null;
    }

    public void SetTcpClient(TcpClient client, ulong clientId)
    {
        _client = client;
        _clientId = clientId;
        Start();
    }

    public virtual async Task<bool> ConnectAsync()
    {
        _client = NewTcpClient(_hostName, _port);

        if (_clientId > 0)
            await _client.ConnectAsync(_hostName ?? string.Empty, _port ?? 0);

        Start();
        return true;
    }

    public virtual bool Connect()
    {
        _client = NewTcpClient(_hostName, _port);

        if (_clientId > 0)
            _client.Connect(_hostName ?? string.Empty, _port ?? 0);

        Start();
        return true;
    }

    private static TcpClient NewTcpClient(string? hostname, int? port)
    {
        if (string.IsNullOrEmpty(hostname))
            throw new ArgumentNullException(nameof(hostname), "Host name cannot be null or empty.");

        if (port is null or 0)
            throw new ArgumentNullException(nameof(port), "Port cannot be null or Zero (0).");

        return new(hostname, port.Value);
    }

    private void Start()
    {
        _stream = _client!.GetStream();
        _reader = new BinaryReader(_stream, Encoding.BigEndianUnicode);
        _writer = new BinaryWriter(_stream, Encoding.BigEndianUnicode);
        _thread = new(new ThreadStart(StartListening));
        _working = true;
        _thread.Start();

        Thread.Sleep(100);
        Validation();
    }

    public void Disconnect()
    {
        Disconnect(true);
    }

    private void Disconnect(bool notify, bool listening = false)
    {
        if (notify)
            SendMessage(new() { Type = (byte)MessageType.Disconnected, Data = [] });
        if (_client?.Connected is true)
            _client?.Close();
        _working = false;
        if (!listening)
            _thread?.Join();
        Disconnected?.Invoke(new(this));
    }

    private void StartListening()
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
                break;
            }
        }

        try
        {
            if (_client?.Connected is true)
                _client.Close();
        }
        catch { }
        Disconnect(!IsServerSide, true);
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

    private bool Validation()
    {
        return SendMessage(new Message
        {
            Type = (byte)MessageType.Validation,
            Data = Encoding.UTF8.GetBytes(ApplicationKey)
        });
    }
}
