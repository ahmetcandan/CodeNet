using CodeNet.Cryptography;
using System.Text;

namespace CodeNet.Transport.Client;

internal class ClientItem
{
    private const int _defaultLength = 16;

    public ulong Id { get; set; }
    public required string Name { get; set; }
    public string? RSAPublicKey { get; set; }
    public AesKey? AESKey { get; set; }

    public static byte[] SerializeObject(ClientItem client)
    {
        if (client is null)
            return [];

        var idData = BitConverter.GetBytes(client.Id);
        var nameData = Encoding.UTF8.GetBytes(client.Name);
        var nameLengthData = BitConverter.GetBytes(nameData.Length);
        var rsaPublicKeyData = string.IsNullOrEmpty(client.RSAPublicKey) ? [] : Encoding.UTF8.GetBytes(client.RSAPublicKey);
        var rsaPublicKeyLengthData = BitConverter.GetBytes(rsaPublicKeyData.Length);
        var aesKeyData = client.AESKey is null ? [] : AesKey.ToData(client.AESKey.Value);

        var bytes = new byte[nameData.Length + rsaPublicKeyData.Length + aesKeyData.Length + _defaultLength];
        bytes[0] = idData[0];
        bytes[1] = idData[1];
        bytes[2] = idData[2];
        bytes[3] = idData[3];
        bytes[4] = idData[4];
        bytes[5] = idData[5];
        bytes[6] = idData[6];
        bytes[7] = idData[7];
        bytes[8] = nameLengthData[0];
        bytes[9] = nameLengthData[1];
        bytes[10] = nameLengthData[2];
        bytes[11] = nameLengthData[3];
        bytes[12] = rsaPublicKeyLengthData[0];
        bytes[13] = rsaPublicKeyLengthData[1];
        bytes[14] = rsaPublicKeyLengthData[2];
        bytes[15] = rsaPublicKeyLengthData[3];

        for (int i = 0; i < nameData.Length; i++)
            bytes[i + _defaultLength] = nameData[i];

        for (int i = 0; i < rsaPublicKeyData.Length; i++)
            bytes[i + _defaultLength + nameData.Length] = rsaPublicKeyData[i];

        for (int i = 0; i < aesKeyData.Length; i++)
            bytes[i + _defaultLength + nameData.Length + rsaPublicKeyData.Length] = aesKeyData[i];

        return bytes;
    }

    public static ClientItem DeserializeObject(byte[] bytes)
    {
        var nameLength = BitConverter.ToInt32(bytes.AsSpan(8, 4));
        var rsaPublicKeyLength = BitConverter.ToInt32(bytes.AsSpan(12, 4));
        return new()
        {
            Id = BitConverter.ToUInt64(bytes.AsSpan(0, 8)),
            Name = Encoding.UTF8.GetString(bytes.AsSpan(_defaultLength, nameLength).ToArray()),
            RSAPublicKey = rsaPublicKeyLength == 0 ? null : Encoding.UTF8.GetString(bytes.AsSpan(_defaultLength, nameLength).ToArray()),
            AESKey = bytes.Length == nameLength + rsaPublicKeyLength + _defaultLength ? null : AesKey.FromData(bytes.AsSpan(nameLength + rsaPublicKeyLength + _defaultLength).ToArray())
        };
    }
}
