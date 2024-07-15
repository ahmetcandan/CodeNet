namespace CodeNet.Core.Settings;

public class ApplicationSettings
{
    public required string Name { get; set; }
    public required string Title { get; set; }
    public string Version { get; set; } = Constant.DefaultVersion;
}
