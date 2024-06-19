using System.Security.Cryptography;

namespace CodeNet.Core.Security;

public sealed class AsymmetricKeyEncryption
{
    public static RSA CreateRSA(string filePath)
    {
        string privateKey = File.ReadAllText(filePath);
        var rsa = RSA.Create();
        rsa.ImportFromPem(privateKey.ToCharArray());
        return rsa;
    }
}
