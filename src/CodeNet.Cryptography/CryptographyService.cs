using System.Security.Cryptography;

namespace CodeNet.Cryptography;

public static class CryptographyService
{
    public static RsaKey GenerateRSAKeys(int size = 2048)
    {
        var rsa = new RSACryptoServiceProvider(size);
        return new(rsa.ToXmlString(false), rsa.ToXmlString(true));
    }

    public static byte[] RSADecrypt(this byte[] data, string privateKey)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(privateKey);
        return rsa.Decrypt(data, false);
    }

    public static byte[] RSAEncrypt(this byte[] data, string publicKey)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(publicKey);
        return [.. rsa.Encrypt(data, false)];
    }

    public static AesKey GenerateAESKey(int size = 256)
    {
        using var aes = Aes.Create();
        aes.KeySize = size;
        aes.GenerateKey();
        aes.GenerateIV();
        return new(aes.Key, aes.IV);
    }

    public static byte[] AESEncrypt(byte[] data, AesKey aesKey)
    {
        using Aes aes = Aes.Create();
        aes.Key = aesKey.Key;
        aes.IV = aesKey.IV;
        using MemoryStream msEncrypt = new();
        using CryptoStream csEncrypt = new(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write);
        csEncrypt.Write(data, 0, data.Length);
        csEncrypt.FlushFinalBlock();

        return msEncrypt.ToArray();
    }

    public static byte[] AESDecrypt(byte[] cipherData, AesKey aesKey)
    {
        using Aes aes = Aes.Create();
        aes.Key = aesKey.Key;
        aes.IV = aesKey.IV;
        using MemoryStream msDecrypt = new(cipherData);
        using CryptoStream csDecrypt = new(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using MemoryStream result = new();
        csDecrypt.CopyTo(result);

        return result.ToArray();
    }
}
