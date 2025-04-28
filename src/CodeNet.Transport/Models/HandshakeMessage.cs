namespace CodeNet.Transport.Models;

internal class HandshakeMessage
{
    public int ClientId { get; set; }

    /// <summary>
    /// Encrypted Key With RSA Algorithm
    /// </summary>
    public required byte[] EncryptedAESKey { get; set; }
}
