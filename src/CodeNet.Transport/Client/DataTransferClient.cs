using CodeNet.Transport.EventDefinitions;

namespace CodeNet.Transport.Client;

public class DataTransferClient(string host, int port, string clientName)
{
    private readonly DataTransferClientItem _client = new(host, port, clientName);

    public event DataReceived? DataReceived;

    public bool Connect()
    {
        var result = _client.Connect();
        if (result)
        {
            _client.DataReceived += OnDataReceived;
        }

        return result;
    }

    public async Task<bool> ConnectAsync()
    {
        var result = await _client.ConnectAsync();
        if (result)
        {
            _client.DataReceived += OnDataReceived;
        }

        return result;
    }

    public bool CanConnection()
    {
        return _client.CanConnection();
    }

    public async Task<bool> CanConnectionAsync()
    {
        return await _client.CanConnectionAsync();
    }

    private void OnDataReceived(DataReceivedArgs e)
    {
        DataReceived?.Invoke(new(e.Data) { ClientName = e.ClientName });
    }

    public void Disconnect()
    {
        _client.DataReceived -= OnDataReceived;
        _client.Disconnect();
    }

    public bool SendData(string to, byte[] data)
    {
        return _client.SendData(to, data);
    }
}
