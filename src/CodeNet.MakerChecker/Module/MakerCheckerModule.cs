using Autofac;
using CodeNet.MakerChecker.Repositories;

namespace CodeNet.MakerChecker.Module;

/// <summary>
/// Maker Checker Module
/// </summary>
public class MakerCheckerModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MakerCheckerDefinitionRepository>().As<IMakerCheckerDefinitionRepository>().InstancePerLifetimeScope();
        builder.RegisterType<MakerCheckerFlowRepository>().As<IMakerCheckerFlowRepository>().InstancePerLifetimeScope();
        builder.RegisterType<MakerCheckerHistoryRepository>().As<IMakerCheckerHistoryRepository>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}