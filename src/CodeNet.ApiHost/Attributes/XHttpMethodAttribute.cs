using SysHttp = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace CodeNet.ApiHost.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class XHttpMethodAttribute(string httpMethod) : Attribute
{
    protected XHttpMethodAttribute(string route, string httpMethod) : this(httpMethod)
    {
        Route = route;
    }

    public XHttpMethodAttribute(SysHttp.HttpMethod httpMethod) : this(httpMethod.ToString().ToUpper())
    { }

    protected XHttpMethodAttribute(string route, SysHttp.HttpMethod httpMethod) : this(route, httpMethod.ToString().ToUpper())
    { }

    public string? Route { get; }

    public string HttpMethod { get; } = httpMethod;
}
