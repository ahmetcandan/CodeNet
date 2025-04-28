namespace CodeNet.Transport.Models;

internal class SendDataMessage
{
    public int ClientId { get; set; }
    public required byte[] Data { get; set; }
}
