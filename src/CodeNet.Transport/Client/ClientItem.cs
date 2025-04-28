namespace CodeNet.Transport.Client;

internal class ClientItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? RSAPublicKey { get; set; }
    public string? AESKey { get; set; }
}
