using Autofac;
using AutoMapper;

namespace CodeNet.Mapper.Module;

/// <summary>
/// Mapper Module
/// </summary>
public class MapperModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType(typeof(AutoMapper.Mapper)).As(typeof(IMapper)).AsSelf().InstancePerLifetimeScope();
        base.Load(builder);
    }
}
