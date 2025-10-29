namespace CodeNet.ApiHost.Attributes;

public class XGetAttribute : XHttpMethodAttribute
{
    private const string _method = "GET";

    public XGetAttribute(string route) : base(route, _method)
    { }

    public XGetAttribute() : base(_method)
    { }
}
