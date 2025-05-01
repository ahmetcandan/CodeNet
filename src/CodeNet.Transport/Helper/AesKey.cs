namespace CodeNet.Transport.Helper;

public readonly struct AesKey(byte[] key, byte[] iv)
{
    public readonly byte[] Key { get; } = key;
    public readonly byte[] IV { get; } = iv;

    public byte[] ToData() => [.. Key, .. IV];

    public override string ToString() => Convert.ToBase64String(ToData());

    public static AesKey FromData(byte[] data) => new(data[..32], data[^16..]);
}
