using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;

namespace CodeNet.Core.Extensions;

public static class ModuleExtensions
{
    public static IModuleRegistrar AddModule<TModule>(this ContainerBuilder builder)
        where TModule : IModule, new() => builder.RegisterModule<TModule>();
}
