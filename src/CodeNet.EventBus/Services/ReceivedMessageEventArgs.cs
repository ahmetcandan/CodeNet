namespace CodeNet.EventBus.Services;

public delegate void MessageReceived(ReceivedMessageEventArgs e);

public class ReceivedMessageEventArgs : EventArgs
{
    public byte[] Message { get; set; }
}
