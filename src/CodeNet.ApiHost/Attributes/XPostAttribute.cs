namespace CodeNet.ApiHost.Attributes;

public class XPostAttribute : XHttpMethodAttribute
{
    private const string _method = "POST";

    public XPostAttribute(string route) : base(route, _method)
    { }

    public XPostAttribute() : base(_method)
    { }
}
