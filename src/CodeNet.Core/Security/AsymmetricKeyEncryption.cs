using System.Security.Cryptography;

namespace CodeNet.Core.Security;

public sealed class AsymmetricKeyEncryption
{
    public static RSA CreateRSA(string filePath)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(File.ReadAllText(filePath).ToCharArray());
        return rsa;
    }
}
