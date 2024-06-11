using Autofac;

namespace NetCore.Abstraction.Module;

public class MongoDBModule<TMongoDBContext> : Autofac.Module
        where TMongoDBContext : MongoDBContext
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TMongoDBContext>().As<MongoDBContext>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}