using System.Security.Cryptography;

namespace CodeNet.Transport.Helper;

internal static class CryptographyHelper
{
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

    public static AesKey GenerateAESKey()
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
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
