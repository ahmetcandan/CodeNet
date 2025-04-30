namespace CodeNet.Transport.Models;

internal class HandshakeMessage
{
    public ulong ClientId { get; set; }

    /// <summary>
    /// Encrypted Key With RSA Algorithm
    /// </summary>
    public required byte[] EncryptedAESKey { get; set; }
}
