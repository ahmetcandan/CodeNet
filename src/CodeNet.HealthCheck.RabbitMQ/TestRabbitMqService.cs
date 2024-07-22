using CodeNet.RabbitMQ.Services;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.RabbitMQ;

internal class TestRabbitMqService(IOptions<BaseRabbitMQOptions> Config) : BaseRabbitMQService(Config)
{
    public bool CanConnection()
    {
		try
		{
			var factory = CreateFactory();
            using var connection = factory.CreateConnection();
            return true;
		}
		catch
		{
			return false;
		}
    }
}
