using Autofac;

namespace CodeNet.HttpClient.Module;

/// <summary>
/// Http Client Module
/// </summary>
public class HttpClientModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<HttpRequest>().As<IHttpRequest>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}