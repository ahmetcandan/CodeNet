using System.Text;

namespace CodeNet.Transport.Models;

internal class ClientConfirmationMessage
{
    private const int _defaultLength = 4;

    public string ClientName { get; set; } = string.Empty;
    public string? PublicKey { get; set; }

    public static byte[] SerializeObject(ClientConfirmationMessage message)
    {
        if (message is null)
            return [];

        var clientNameData = Encoding.UTF8.GetBytes(message.ClientName);
        var publicKeyData = string.IsNullOrEmpty(message.PublicKey) ? [] : Encoding.UTF8.GetBytes(message.PublicKey);
        var bytes = new byte[clientNameData.Length + publicKeyData.Length + _defaultLength];

        byte[] clientNameLengthData = BitConverter.GetBytes(clientNameData.Length);
        bytes[0] = clientNameLengthData[0];
        bytes[1] = clientNameLengthData[1];
        bytes[2] = clientNameLengthData[2];
        bytes[3] = clientNameLengthData[3];

        for (int i = 0; i < clientNameData.Length; i++)
            bytes[i + _defaultLength] = clientNameData[i];

        for (int i = 0; i < publicKeyData.Length; i++)
            bytes[i + _defaultLength + clientNameData.Length] = publicKeyData[i];

        return bytes;
    }

    public static ClientConfirmationMessage DeserializeObject(byte[] bytes)
    {
        var clientNameLength = BitConverter.ToInt32(bytes.AsSpan(0, _defaultLength));
        return new()
        {
            ClientName = Encoding.UTF8.GetString(bytes.AsSpan(_defaultLength, clientNameLength).ToArray()),
            PublicKey = bytes.Length == clientNameLength + _defaultLength ? null : Encoding.UTF8.GetString(bytes.AsSpan(_defaultLength + clientNameLength).ToArray())
        };
    }
}
