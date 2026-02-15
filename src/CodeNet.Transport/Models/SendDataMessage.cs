namespace CodeNet.Transport.Models;

internal class SendDataMessage
{
    private const int _defaultLength = 8;

    public ulong ClientId { get; set; }
    public required byte[] Data { get; set; }

    public static byte[] SerializeObject(SendDataMessage message)
    {
        if (message is null)
            return [];
        var bytes = new byte[message.Data.Length + _defaultLength];

        byte[] clientId = BitConverter.GetBytes(message.ClientId);
        bytes[0] = clientId[0];
        bytes[1] = clientId[1];
        bytes[2] = clientId[2];
        bytes[3] = clientId[3];
        bytes[4] = clientId[4];
        bytes[5] = clientId[5];
        bytes[6] = clientId[6];
        bytes[7] = clientId[7];

        for (int i = 0; i < message.Data.Length; i++)
            bytes[i + _defaultLength] = message.Data[i];

        return bytes;
    }

    public static SendDataMessage DeserializeObject(byte[] bytes) => new()
    {
        ClientId = BitConverter.ToUInt64(bytes.AsSpan(0, _defaultLength)),
        Data = bytes.AsSpan(_defaultLength).ToArray()
    };
}
