using CodeNet.Transport.Client;

namespace CodeNet.Transport.Models;

internal class ServerConfirmationMessage
{
    public bool UseSecurity { get; set; }
    public IEnumerable<ClientItem> Clients { get; set; } = [];
}
