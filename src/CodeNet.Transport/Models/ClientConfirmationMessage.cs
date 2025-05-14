namespace CodeNet.Transport.Models;

internal class ClientConfirmationMessage
{
    public string ClientName { get; set; } = string.Empty;
    public string? PublicKey { get; set; }
}
