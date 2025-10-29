namespace CodeNet.ApiHost.Attributes;

public class XDeleteAttribute : XHttpMethodAttribute
{
    private const string _method = "DELETE";

    public XDeleteAttribute(string route) : base(route, _method)
    { }

    public XDeleteAttribute() : base(_method)
    { }
}
