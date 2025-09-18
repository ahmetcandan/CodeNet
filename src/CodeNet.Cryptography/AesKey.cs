namespace CodeNet.Cryptography;

public readonly struct AesKey(byte[] key, byte[] iv)
{
    public readonly byte[] Key { get; } = key;
    public readonly byte[] IV { get; } = iv;

    public static byte[] ToData(AesKey aes) => [.. aes.Key, .. aes.IV];

    public override string ToString() => Convert.ToBase64String(ToData(this));

    public static AesKey FromData(byte[] data) => new(data[..32], data[^16..]);
}
