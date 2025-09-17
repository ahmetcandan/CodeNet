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
    private ulong? _clientId = null;
    private Thread? _thread;
    private NetworkStream? _stream;
    private BinaryReader? _reader;
    private BinaryWriter? _writer;
    private int _connectionTimeout = 5;

    public void Dispose()
    {
        Disconnect_();
        GC.SuppressFinalize(this);
    }

    public bool Working { get; private set; } = false;
    public bool Connected { get; private set; } = false;
    public ulong ClientId { get => _clientId ?? 0; private set => _clientId = value; }
    public bool IsServerSide { get { return _clientId is not null; } }

    /// <summary>
    /// Default: 30 seconds
    /// </summary>
    public int ConnectionTimeout
    {
        get => _connectionTimeout;
        set => _connectionTimeout = !Working ? value : throw new InvalidOperationException("Cannot set connection timeout while connected.");
    }

    public event NewMessageReceived? NewMessgeReceived;
    public event ClientDisconnected<CodeNetClient>? Disconnected;
    public event ClientConnected<CodeNetClient>? ConnectedEvent;

    public abstract string ApplicationKey { get; }

    public CodeNetClient() => ClientId = 0;

    public CodeNetClient(string hostName, int port)
    {
        _hostName = hostName;
        _port = port;
    }

    public void SetTcpClient(TcpClient client, ulong clientId)
    {
        _client = client;
        ClientId = clientId;
        Start();
    }

    public virtual async Task<bool> ConnectAsync()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ConnectionTimeout));
        _client = NewTcpClient(_hostName, _port);
        await _client.ConnectAsync(_hostName!, _port!.Value);
        Start();
        ConnectedHandler(cts);
        return true;
    }

    public virtual bool Connect()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ConnectionTimeout));
        _client = NewTcpClient(_hostName, _port);
        _client.Connect(_hostName!, _port!.Value);
        Start();
        ConnectedHandler(cts);
        return true;
    }

    public virtual async Task<bool> CanConnectionAsync()
    {
        if (await ConnectAsync())
        {
            Disconnect();
            return true;
        }
        return false;
    }

    public virtual bool CanConnection()
    {
        if (Connect())
        {
            Disconnect();
            return true;
        }
        return false;
    }

    private void ConnectedHandler(CancellationTokenSource cancellationTokenSource)
    {
        while (!Connected)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
                throw new TimeoutException("Connection timeout.");

            Thread.Sleep(100);
        }
    }

    private static TcpClient NewTcpClient(string? hostname, int? port) => string.IsNullOrEmpty(hostname)
            ? throw new ArgumentNullException(nameof(hostname), "Host name cannot be null or empty.")
            : port is null or 0 ? throw new ArgumentNullException(nameof(port), "Port cannot be null or Zero (0).") : new();

    private void Start()
    {
        _stream = _client!.GetStream();
        _reader = new BinaryReader(_stream, Encoding.BigEndianUnicode);
        _writer = new BinaryWriter(_stream, Encoding.BigEndianUnicode);
        _thread = new(new ThreadStart(StartListening));
        Working = true;
        _thread.Start();

        Thread.Sleep(100);

        if (!IsServerSide)
            Validation();
    }

    public void Disconnect() => Disconnect_();

    private void Disconnect_(bool listening = false)
    {
        if (!IsServerSide)
            SendMessage(new((byte)MessageType.Disconnected, []));
        if (_client?.Connected is true)
            _client?.Close();
        Working = false;
        if (!listening)
            _thread?.Join();
        Disconnected?.Invoke(new(this));
    }

    private void StartListening()
    {
        while (Working)
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
                    ReceivedMessage_(Message.Deseriliaze(buffer[0], message));
                }
                else
                    ReceivedMessage_(Message.Deseriliaze(buffer[0], []));
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
        if (IsServerSide)
            Disconnected?.Invoke(new(this));
    }

    private void ReceivedMessage_(Message message)
    {
        if (IsServerSide && message.Type is (byte)MessageType.Disconnected)
        {
            Disconnect_(true);
            return;
        }

        if (message.Type is (byte)MessageType.Validation)
        {
            if (IsServerSide)
                NewMessgeReceived?.Invoke(new(message));
            else
            {
                Connected = message.Data.Length == 1 && message.Data[0] == 1;
                if (Connected)
                {
                    ConnectedEvent?.Invoke(new(this));
                    SendMessage(new((byte)MessageType.Connected, [1]));
                }
            }

            return;
        }
        if (IsServerSide && message.Type is (byte)MessageType.Connected)
        {
            Connected = message.Data.Length == 1 && message.Data[0] == 1;
            if (Connected)
                ConnectedEvent?.Invoke(new(this));
            return;
        }

        if (!IsServerSide && !Connected)
            return;

        ReceivedMessage(message);
    }

    protected internal virtual void ReceivedMessage(Message message) => NewMessgeReceived?.Invoke(new(message));

    public bool SendMessage(Message message)
    {
        if (!Working || !(_writer?.BaseStream.CanWrite ?? false))
            return false;

        _writer?.Write(Message.Seriliaze(message));
        return true;
    }

    private bool Validation() => SendMessage(new((byte)MessageType.Validation, Encoding.UTF8.GetBytes(ApplicationKey)));
}
