namespace CodeNet.Transport.Models;

internal class SendDataMessage
{
    public ulong ClientId { get; set; }
    public required byte[] Data { get; set; }
}
