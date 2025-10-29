namespace CodeNet.ApiHost.Attributes;


[AttributeUsage(AttributeTargets.Class)]
public class XApiRouteAttribute(string route) : Attribute
{
    public string Route { get; } = route;
}
