using Autofac;
using NetCore.Abstraction;

namespace NetCore.MongoDB.Module;

/// <summary>
/// MongoDB Module
/// </summary>
/// <typeparam name="TMongoDBContext"></typeparam>
public class MongoDBModule<TMongoDBContext> : Autofac.Module
        where TMongoDBContext : MongoDBContext
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TMongoDBContext>().As<TMongoDBContext>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}

/// <summary>
/// MongoDB Module
/// </summary>
public class MongoDBModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MongoDBContext>().As<MongoDBContext>().InstancePerLifetimeScope();
        base.Load(builder);
    }
}