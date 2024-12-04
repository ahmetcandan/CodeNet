namespace CodeNet.EventBus.Settings;

public abstract class BaseEventBusOptions
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string Channel { get; set; }
}
