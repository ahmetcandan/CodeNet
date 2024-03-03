using Autofac;
using NetCore.Abstraction;
using NetCore.Core;
using NetCore.MongoDB;
using NetCore.RabbitMQ;
using NetCore.Redis;

namespace NetCore.Container
{
    public class NetCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IdentityContext>().As<IIdentityContext>().InstancePerLifetimeScope();
            builder.RegisterType<RedisCacheRepository>().As<ICacheRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MongoDBLogRepository>().As<ILogRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RabbitMQService>().As<IQService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
