using Autofac;

namespace CodeNet.MakerChecker.Module;

/// <summary>
/// Maker Checker Module
/// </summary>
public class MakerCheckerModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MakerCheckerManager>().As<IMakerCheckerManager>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}