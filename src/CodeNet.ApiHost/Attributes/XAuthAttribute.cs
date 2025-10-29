namespace CodeNet.ApiHost.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class XAuthAttribute : Attribute
{
    public string? Users { get; set; }

    public string? Roles { get; set; }
}
