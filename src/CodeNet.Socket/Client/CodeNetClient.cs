using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CodeNet.Socket.Client;

public abstract class CodeNetClient : ICodeNetClient
{
    private readonly string? _hostName;
    private readonly int? _port;

    private TcpClient? _client;
    private ulong? _clientId = null;
    private Thread? _thread;
    private SslStream? _sslStream;
    private BinaryReader? _reader;
    private BinaryWriter? _writer;
    private TimeSpan _connectionTimeout = TimeSpan.FromSeconds(30);
    private X509Certificate2? _certificate;
    private readonly bool _secureConnection = false;

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
    public TimeSpan ConnectionTimeout
    {
        get => _connectionTimeout;
        set => _connectionTimeout = !Working ? value : throw new InvalidOperationException("Cannot set connection timeout while connected.");
    }

    public event NewMessageReceived? NewMessgeReceived;
    public event ClientDisconnected<CodeNetClient>? Disconnected;
    public event ClientConnected<CodeNetClient>? ConnectedEvent;

    public abstract string ApplicationKey { get; }

    protected CodeNetClient() => ClientId = 0;

    public CodeNetClient(string hostName, int port)
    {
        _hostName = hostName;
        _port = port;
    }

    protected CodeNetClient(string hostName, int port, bool secureConnection) : this(hostName, port)
    {
        _secureConnection = secureConnection;
    }

    protected CodeNetClient(string hostName, int port, string certificatePath, string certificatePassword) : this(hostName, port)
    {
        _secureConnection = true;
        _certificate = new X509Certificate2(certificatePath, certificatePassword);
    }

    public void SetTcpClient(TcpClient client, ulong clientId)
    {
        _client = client;
        ClientId = clientId;
        Start();
    }

    public void SetTcpClient(TcpClient client, ulong clientId, X509Certificate2 certificate)
    {
        _certificate = certificate;
        SetTcpClient(client, clientId);
    }

    public virtual async Task<bool> ConnectAsync()
    {
        using var cts = new CancellationTokenSource(ConnectionTimeout);
        _client = NewTcpClient(_hostName, _port);
        await _client.ConnectAsync(_hostName!, _port!.Value);
        Start();
        ConnectedHandler(cts);
        return true;
    }

    public virtual bool Connect()
    {
        using var cts = new CancellationTokenSource(ConnectionTimeout);
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

    private static TcpClient NewTcpClient(string? hostname, int? port)
    {
        if (string.IsNullOrEmpty(hostname))
            throw new ArgumentNullException(nameof(hostname), "Host name cannot be null or empty.");

        return port is null or 0 ? throw new ArgumentNullException(nameof(port), "Port cannot be null or Zero (0).") : new();
    }

    private void Start()
    {
        var stream = _client!.GetStream();
        if (IsServerSide)
        {
            if (_certificate is not null)
            {
                _sslStream = new(stream, false);
                _sslStream.AuthenticateAsServer(_certificate,
                    clientCertificateRequired: false,
                    enabledSslProtocols: SslProtocols.None,
                    checkCertificateRevocation: false);
                _reader = new BinaryReader(_sslStream, Encoding.BigEndianUnicode);
                _writer = new BinaryWriter(_sslStream, Encoding.BigEndianUnicode);
            }
            else
            {
                _reader = new BinaryReader(stream, Encoding.BigEndianUnicode);
                _writer = new BinaryWriter(stream, Encoding.BigEndianUnicode);
            }
        }
        else
        {
            if (_secureConnection)
            {
                _sslStream = new SslStream(
                    stream,
                    false,
                    new RemoteCertificateValidationCallback(ValidateServerCertificate),
                    null
                );
                if (_certificate is not null)
                {
                    X509CertificateCollection clientCertificates = [_certificate];
                    _sslStream.AuthenticateAsClient(
                        _hostName!,
                        clientCertificates,
                        SslProtocols.None,
                        false
                    );
                }
                else
                    _sslStream.AuthenticateAsClient(_hostName!);
                _reader = new BinaryReader(_sslStream, Encoding.BigEndianUnicode);
                _writer = new BinaryWriter(_sslStream, Encoding.BigEndianUnicode);
            }
            else
            {
                _reader = new BinaryReader(stream, Encoding.BigEndianUnicode);
                _writer = new BinaryWriter(stream, Encoding.BigEndianUnicode);
            }
        }

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
            _client.Close();
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

        if (_client?.Connected is true)
            _client.Close();

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
            HandleValidationMessage(message);
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

    private void HandleValidationMessage(Message message)
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
    }

    protected internal virtual void ReceivedMessage(Message message) => NewMessgeReceived?.Invoke(new(message));

    public bool SendMessage(Message message)
    {
        if (!Working || !(_writer?.BaseStream.CanWrite ?? false))
            return false;

        _writer.Write(Message.Seriliaze(message));
        return true;
    }

    private void Validation() => SendMessage(new((byte)MessageType.Validation, Encoding.UTF8.GetBytes(ApplicationKey)));

    internal virtual bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return sslPolicyErrors == SslPolicyErrors.None;
    }
}
