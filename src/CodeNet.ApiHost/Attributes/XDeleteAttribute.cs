namespace CodeNet.ApiHost.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class XDeleteAttribute : XHttpMethodAttribute
{
    private const string _method = "DELETE";

    public XDeleteAttribute(string route) : base(route, _method)
    { }

    public XDeleteAttribute() : base(_method)
    { }
}
