using Autofac;

namespace CodeNet.Core.Module;

/// <summary>
/// CodeNet Module
/// </summary>
public class CodeNetModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CodeNetHttpContext>().As<ICodeNetHttpContext>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}
