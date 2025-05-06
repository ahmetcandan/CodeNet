namespace CodeNet.Cryptography;

public readonly struct RsaKey(string publicKey, string privateKey)
{
    public readonly string PublicKey { get; } = publicKey;
    public readonly string PrivateKey { get; } = privateKey;
}