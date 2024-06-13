using Microsoft.Extensions.Options;
using CodeNet.Abstraction.Model;
using RabbitMQ.Client;

namespace CodeNet.RabbitMQ
{
    public abstract class BaseRabbitMQService<TData>(IOptions<RabbitMQSettings> Config)
    {
        protected ConnectionFactory CreateFactory()
        {
            var factory = new ConnectionFactory()
            {
                HostName = Config.Value.HostName,
                UserName = Config.Value.Username,
                Password = Config.Value.Password
            };

            if (Config.Value.SocketReadTimeout.HasValue)
                factory.SocketReadTimeout = Config.Value.SocketReadTimeout.Value;
            if (!string.IsNullOrEmpty(Config.Value.ClientProvidedName))
                factory.ClientProvidedName = Config.Value.ClientProvidedName;
            if (Config.Value.Port.HasValue)
                factory.Port = Config.Value.Port.Value;
            if (Config.Value.MaxMessageSize.HasValue)
                factory.MaxMessageSize = Config.Value.MaxMessageSize.Value;

            return factory;
        }
    }
}
