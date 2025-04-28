using System.Security.Cryptography;
using System.Text;

namespace CodeNet.Transport.Helper;

internal static class CryptographyHelper
{
    public static string RSADecrypt(this byte[] data, string privateKey)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(privateKey);
        return Encoding.UTF8.GetString(rsa.Decrypt(data, false));
    }

    public static byte[] RSAEncrypt(this byte[] data, string publicKey)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(publicKey);
        return [.. rsa.Encrypt(data, false)];
    }

    public static string GenerateAESKey()
    {
        byte[] key = new byte[48];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        return Convert.ToBase64String(key);
    }

    public static byte[] AESEncrypt(byte[] data, string aesKey)
    {
        var aes = GetAes(ToAesKey(aesKey));
        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write);
        cs.Write(data, 0, data.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }

    public static byte[] Decrypt(byte[] encryptedData, string aesKey)
    {
        var aes = GetAes(ToAesKey(aesKey));
        using MemoryStream ms = new(encryptedData);
        using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        var decrypted = new byte[encryptedData.Length];
        int bytesRead = cs.Read(decrypted, 0, decrypted.Length);
        Array.Resize(ref decrypted, bytesRead);
        return decrypted;
    }

    private static AesKey ToAesKey(string key)
    {
        byte[] keys = Convert.FromBase64String(key);
        return new(keys[..32], keys[^16..]);
    }

    private static Aes GetAes(AesKey key)
    {
        using Aes aes = Aes.Create();
        aes.Key = key.Key;
        aes.IV = key.IV;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        return aes;
    }

    private readonly struct AesKey(byte[] key, byte[] iv)
    {
        public readonly byte[] Key { get; } = key;
        public readonly byte[] IV { get; } = iv;
    }
}
