namespace NetCore.Abstraction.Model;

public class ApplicationSettings
{
    public required string Name { get; set; }
    public required string Title { get; set; }

    private string? _version;
    public required string Version
    {
        get
        {
            return string.IsNullOrEmpty(_version) ? "v1.0" : _version;
        }
        set 
        {
            _version = value;
        }
    }
}
