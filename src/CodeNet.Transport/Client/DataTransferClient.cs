using CodeNet.Socket.Client;
using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using CodeNet.Transport.EventDefinitions;
using CodeNet.Transport.Helper;
using CodeNet.Transport.Models;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeNet.Transport.Client;

public partial class DataTransferClient : CodeNetClient
{
    private string _clientName = string.Empty;
    private string? _publicKey;
    private string? _privateKey;
    private bool _secureConnection = false;
    private bool _connected = false;
    private int _connectionTimeout = 30;
    private IList<ClientItem>? _clients = null;

    public bool SecurityConnection { get { return _secureConnection; } private set { _secureConnection = value; } }

    /// <summary>
    /// Default: 30 seconds
    /// </summary>
    public int ConnectionTimeout
    {
        get
        {
            return _connectionTimeout;
        }
        set
        {
            if (!Working)
                _connectionTimeout = value;
            else
                throw new InvalidOperationException("Cannot set connection timeout while connected.");
        }
    }

    public DataTransferClient(string hostName, int port, string clientName) : base(hostName, port)
    {
        ConsoleLog($"Client: new to {hostName}:{port}...");
        if (!ClientNameValidation(clientName))
            throw new ArgumentException($"Client name is not valid ({clientName}).");

        _clientName = clientName;
        _clients = [];
    }

    public DataTransferClient()
    {
        ConsoleLog($"Server: new to...");
    }

    public override async Task<bool> ConnectAsync()
    {
        return ConnectedHandler(await base.ConnectAsync());
    }

    public override bool Connect()
    {
        return ConnectedHandler(base.Connect());
    }

    private bool ConnectedHandler(bool result)
    {
        ConsoleLog($"Connecting to handeler");
        if (!result)
            return false;

        SetClientName(_clientName);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ConnectionTimeout));
        while (!_connected)
        {
            if (cts.Token.IsCancellationRequested)
                throw new TimeoutException("Connection timeout.");

            Thread.Sleep(100);
        }
        ConsoleLog($"Connected to handeler");

        return true;
    }

    internal bool SetClientName(string clientName)
    {
        ConsoleLog($"Set client name: {clientName}");
        ClientName = clientName;
        return SendMessage(new()
        {
            Type = (byte)Models.MessageType.SetClientName,
            Data = Encoding.UTF8.GetBytes(clientName)
        });
    }

    public event ClientConnectFinish<DataTransferClient>? ClientConnectFinish;

    public string ClientName { get { return _clientName; } private set { _clientName = value; } }
    public string? PublicKey { get { return _publicKey; } private set { _publicKey = value; } }

    public event DataReceived? DataReceived;

    protected override void ReceivedMessage(Message message)
    {
        ConsoleLog($"Received message: {(Models.MessageType)message.Type} ({message.Data.Length}) bytes");
        switch (message.Type)
        {
            case (byte)Models.MessageType.Message:
                if (IsServerSide)
                    base.ReceivedMessage(message);
                else
                    DataReceivedHandler(new(message));
                return;
            case (byte)Models.MessageType.SetClientName:
                ClientName = Encoding.UTF8.GetString(message.Data);
                return;
            case (byte)Models.MessageType.Connected:
                _connected = true;
                ClientConnectFinish?.Invoke(new(this));
                base.ReceivedMessage(message);
                return;
            case (byte)Models.MessageType.UseSecutity:
                if (message.Data[0] is 1)
                    GenerateRSAKeys();
                return;
            case (byte)Models.MessageType.SharePublicKey:
                PublicKey = Encoding.UTF8.GetString(message.Data);
                base.ReceivedMessage(message);
                return;
            case (byte)Models.MessageType.ShareAESKey:
                if (IsServerSide)
                {
                    base.ReceivedMessage(message);
                    return;
                }

                if (string.IsNullOrEmpty(_privateKey))
                    throw new InvalidOperationException("Private key is not set.");

                var handshakeMessage = SerializerHelper.DeserializeObject<HandshakeMessage>(message.Data);
                if (handshakeMessage is null)
                    return;

                var client = _clients?.FirstOrDefault(c => c.Id == handshakeMessage?.ClientId);
                if (client is not null)
                    client.AESKey = CryptographyHelper.RSADecrypt(handshakeMessage.EncryptedAESKey, _privateKey!);
                return;
            case (byte)Models.MessageType.ClienList:
                if (IsServerSide)
                    return;

                _clients = SerializerHelper.DeserializeObject<IList<ClientItem>>(message.Data);
                ConsoleLog("Client send connected.");
                SendMessage(new()
                {
                    Type = (byte)Models.MessageType.Connected,
                    Data = [1]
                });
                return;
            case (byte)Models.MessageType.None:
            default:
                return;
        }
    }

    private void GenerateRSAKeys()
    {
        SecurityConnection = true;
        var rsa = new RSACryptoServiceProvider();
        _privateKey = rsa.ToXmlString(true);
        _publicKey = rsa.ToXmlString(false);
        ConsoleLog($"Generate RSA Key Public key: {_publicKey}");
        SendMessage(new()
        {
            Type = (byte)Models.MessageType.SharePublicKey,
            Data = Encoding.UTF8.GetBytes(PublicKey!)
        });
    }

    public bool SendData(string clientName, byte[] data)
    {
        ConsoleLog($"Send data to {clientName} ({data.Length}) bytes");
        if (!(_clients?.Any(c => c.Name.Equals(clientName)) ?? false))
            return false;

        foreach (var client in _clients.Where(c => c.Name.Equals(clientName)))
            return SendDataByClient(client, data);

        return true;
    }

    private bool SendDataByClient(ClientItem client, byte[] data)
    {
        ConsoleLog($"Send data by client {client.Name} ({data.Length}) bytes");
        if (SecurityConnection)
        {
            if (string.IsNullOrEmpty(client.AESKey))
            {
                if (!Handshake(client))
                    return false;
            }

            data = CryptographyHelper.AESEncrypt(data, client.AESKey!);
            ConsoleLog("Encrypted Data");
        }

        ConsoleLog($"Send data by client {client.Name} ({data.Length}) bytes. End");
        return SendMessage(new()
        {
            Type = (byte)Models.MessageType.Message,
            Data = SerializerHelper.SerializeObject(new SendDataMessage() { ClientId = client.Id, Data = data })
        });
    }

    private bool Handshake(ClientItem client)
    {
        ConsoleLog($"Handshake with {client.Name}");
        if (string.IsNullOrEmpty(client.RSAPublicKey))
            throw new InvalidOperationException("Client public key is not set.");
        else
        {
            client.AESKey = CryptographyHelper.GenerateAESKey();
            ConsoleLog($"Send Message AES Key: {client.AESKey}");
            return SendMessage(new()
            {
                Type = (byte)Models.MessageType.ShareAESKey,
                Data = SerializerHelper.SerializeObject(new HandshakeMessage
                {
                    ClientId = client.Id,
                    EncryptedAESKey = CryptographyHelper.RSAEncrypt(Encoding.UTF8.GetBytes(client.AESKey), client.RSAPublicKey)
                })
            });
        }
    }

    private void DataReceivedHandler(MessageReceivingArguments e)
    {
        ConsoleLog($"Data received handler: {e.Message.Type} ({e.Message.Data.Length}) bytes");
        var message = SerializerHelper.DeserializeObject<SendDataMessage>(e.Message.Data);
        if (message is null)
            return;

        var client = _clients?.FirstOrDefault(c => c.Id == message.ClientId);
        if (client is null)
            return;

        if (SecurityConnection)
        {
            if (string.IsNullOrEmpty(client.AESKey))
                return;

            DataReceived?.Invoke(new(CryptographyHelper.Decrypt(message.Data, client.AESKey)) { ClientName = client.Name });
            return;
        }

        DataReceived?.Invoke(new(message.Data) { ClientName = client.Name });
    }

    private static bool ClientNameValidation(string clientName)
    {
        return ClientNameRegex().Match(clientName).Success;
    }

    [GeneratedRegex(@"^[a-zA-Z_][a-zA-Z0-9_.-]*$")]
    private static partial Regex ClientNameRegex();

    private void ConsoleLog(string log) => Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fffff}] [{(IsServerSide ? $"Server.Client-[{ClientId}]" : $"Client-{ClientName}")}] {log}");
}
