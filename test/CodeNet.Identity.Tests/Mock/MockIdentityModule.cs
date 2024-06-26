using Autofac;
using CodeNet.Identity.Module;

namespace CodeNet.Identity.Tests.Mock;

public class MockIdentityModule : IdentityModule
{
    public void Load(ContainerBuilder builder) => base.Load(builder);
}
