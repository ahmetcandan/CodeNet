namespace CodeNet.ApiHost.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class XPutAttribute : XHttpMethodAttribute
{
    private const string _method = "PUT";

    public XPutAttribute(string route) : base(route, _method)
    { }

    public XPutAttribute() : base(_method)
    { }
}
