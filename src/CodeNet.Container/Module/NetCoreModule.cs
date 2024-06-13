using Autofac;
using CodeNet.Abstraction;
using CodeNet.Core;

namespace CodeNet.Container.Module;

/// <summary>
/// CodeNet Module
/// </summary>
public class NetCoreModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<IdentityContext>().As<IIdentityContext>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}
