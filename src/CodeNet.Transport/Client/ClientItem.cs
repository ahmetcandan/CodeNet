using CodeNet.Transport.Helper;

namespace CodeNet.Transport.Client;

internal class ClientItem
{
    public ulong Id { get; set; }
    public required string Name { get; set; }
    public string? RSAPublicKey { get; set; }
    public AesKey? AESKey { get; set; }
}
