namespace CodeNet.StackExchange.Redis.Models;

public class ReceivedMessageEventArgs : EventArgs
{
    public string? Message { get; set; }
    public string? Channel { get; set; }
}
