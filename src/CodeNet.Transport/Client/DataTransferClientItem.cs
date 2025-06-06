using CodeNet.Cryptography;
using CodeNet.Socket.Client;
using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using CodeNet.Transport.EventDefinitions;
using CodeNet.Transport.Helper;
using CodeNet.Transport.Models;
using System.Text.RegularExpressions;

namespace CodeNet.Transport.Client;

internal partial class DataTransferClientItem : CodeNetClient
{
    private string? _privateKey;
    private bool _connected = false;
    private IList<ClientItem>? _clients = null;

    public bool SecurityConnection { get; private set; } = false;

    public new bool Connected { get { return base.Connected && _connected; } }

    public DataTransferClientItem(string hostName, int port, string clientName) : base(hostName, port)
    {
        if (!ClientNameValidation(clientName))
            throw new ArgumentException($"Client name is not valid ({clientName}).");

        ClientName = clientName;
        _clients = [];
    }

    public DataTransferClientItem()
    {
    }

    public override async Task<bool> ConnectAsync()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ConnectionTimeout));
        return ConnectedHandler(await base.ConnectAsync(), cts);
    }

    public override bool Connect()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ConnectionTimeout));
        return ConnectedHandler(base.Connect(), cts);
    }

    public override string ApplicationKey => TransportKey.ApplicationKey;

    private bool ConnectedHandler(bool result, CancellationTokenSource cancellationTokenSource)
    {
        if (!result)
            return false;

        while (!Connected)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
                throw new TimeoutException("Connection timeout.");

            Thread.Sleep(100);
        }

        return true;
    }

    public event ClientConnectFinish<DataTransferClientItem>? ClientConnectFinish;

    public string ClientName { get; private set; } = string.Empty;
    public string? PublicKey { get; private set; }

    public event DataReceived? DataReceived;

    protected override void ReceivedMessage(Message message)
    {
        switch (message.Type)
        {
            case (byte)Models.MessageType.Message:
                if (IsServerSide)
                    base.ReceivedMessage(message);
                else
                    DataReceivedHandler(new(message));
                return;
            case (byte)Models.MessageType.ServerConfirmation:
                if (IsServerSide)
                    return;

                var serverConfirmationMessage = SerializerHelper.DeserializeObject<ServerConfirmationMessage>(message.Data);
                if (serverConfirmationMessage?.UseSecurity is true)
                    GenerateRSAKeys();

                _clients = serverConfirmationMessage?.Clients?.ToList() ?? [];
                SendMessage(new()
                {
                    Type = (byte)Models.MessageType.ClientConfirmation,
                    Data = SerializerHelper.SerializeObject(new ClientConfirmationMessage
                    {
                        ClientName = ClientName,
                        PublicKey = PublicKey
                    })
                });
                _connected = true;
                return;
            case (byte)Models.MessageType.ClientConfirmation:
                if (!IsServerSide)
                    return;

                var clientConfirmationMessage = SerializerHelper.DeserializeObject<ClientConfirmationMessage>(message.Data);
                ClientName = clientConfirmationMessage?.ClientName ?? string.Empty;
                ClientConnectFinish?.Invoke(new(this));
                PublicKey = clientConfirmationMessage?.PublicKey;
                base.ReceivedMessage(message);
                _connected = true;
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
                    client.AESKey = AesKey.FromData(CryptographyService.RSADecrypt(handshakeMessage.EncryptedAESKey, _privateKey!));
                return;
            case (byte)Models.MessageType.ClienList:
                if (IsServerSide)
                    return;

                _clients = SerializerHelper.DeserializeObject<IList<ClientItem>>(message.Data);
                return;
            case (byte)Models.MessageType.None:
            default:
                return;
        }
    }

    private void GenerateRSAKeys()
    {
        SecurityConnection = true;
        var rsa = CryptographyService.GenerateRSAKeys();
        _privateKey = rsa.PrivateKey;
        PublicKey = rsa.PublicKey;
    }

    public bool SendData(string clientName, byte[] data)
    {
        if (!(_clients?.Any(c => c.Name.Equals(clientName)) ?? false))
            return false;

        foreach (var client in _clients.Where(c => c.Name.Equals(clientName)))
            return SendDataByClient(client, data);

        return true;
    }

    private bool SendDataByClient(ClientItem client, byte[] data)
    {
        if (SecurityConnection)
        {
            if (!client.AESKey.HasValue)
            {
                if (!Handshake(client))
                    return false;
            }

            data = CryptographyService.AESEncrypt(data, client.AESKey!.Value);
        }

        return SendMessage(new()
        {
            Type = (byte)Models.MessageType.Message,
            Data = SerializerHelper.SerializeObject(new SendDataMessage() { ClientId = client.Id, Data = data })
        });
    }

    private bool Handshake(ClientItem client)
    {
        if (string.IsNullOrEmpty(client.RSAPublicKey))
            throw new InvalidOperationException("Client public key is not set.");
        else
        {
            client.AESKey = CryptographyService.GenerateAESKey();
            return SendMessage(new()
            {
                Type = (byte)Models.MessageType.ShareAESKey,
                Data = SerializerHelper.SerializeObject(new HandshakeMessage
                {
                    ClientId = client.Id,
                    EncryptedAESKey = CryptographyService.RSAEncrypt(client.AESKey.Value.ToData(), client.RSAPublicKey)
                })
            });
        }
    }

    private void DataReceivedHandler(MessageReceivingArguments e)
    {
        var message = SerializerHelper.DeserializeObject<SendDataMessage>(e.Message.Data);
        if (message is null)
            return;

        var client = _clients?.FirstOrDefault(c => c.Id == message.ClientId);
        if (client is null)
            return;

        if (SecurityConnection)
        {
            if (!client.AESKey.HasValue)
                return;

            DataReceived?.Invoke(new(CryptographyService.AESDecrypt(message.Data, client.AESKey.Value)) { ClientName = client.Name });
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
}
