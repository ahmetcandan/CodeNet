using Autofac;
using CodeNet.Identity.Manager;

namespace CodeNet.Identity.Module;

/// <summary>
/// Identity Module
/// </summary>
public class IdentityModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<IdentityTokenManager>().As<IIdentityTokenManager>().InstancePerLifetimeScope();
        builder.RegisterType<IdentityUserManager>().As<IIdentityUserManager>().InstancePerLifetimeScope();
        builder.RegisterType<IdentityRoleManager>().As<IIdentityRoleManager>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}