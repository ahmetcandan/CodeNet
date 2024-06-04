namespace NetCore.Abstraction.Model;

public class RedisSettings
{
    public string Hostname { get; set; }
    public int Port { get; set; }

    private string _instanceName;
    public string InstanceName
    {
        get
        {
            return string.IsNullOrEmpty(_instanceName) ? "master" : _instanceName;
        }
        set
        {
            _instanceName = value;
        }
    }
}
