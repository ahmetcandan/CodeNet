using Autofac;
using NetCore.Abstraction;
using NetCore.Core;

namespace NetCore.Container.Module;

/// <summary>
/// NetCore Module
/// </summary>
public class NetCoreModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<IdentityContext>().As<IIdentityContext>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}
