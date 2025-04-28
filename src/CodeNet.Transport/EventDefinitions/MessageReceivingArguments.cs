namespace CodeNet.Transport.EventDefinitions;

public class DataReceivedArgs(byte[] data)
{
    public required string ClientName { get; set; }
    public byte[] Data { get; set; } = data;
}
