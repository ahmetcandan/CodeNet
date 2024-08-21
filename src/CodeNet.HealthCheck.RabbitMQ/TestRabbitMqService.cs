using CodeNet.RabbitMQ.Services;
using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.RabbitMQ;

internal class TestRabbitMqService(IOptions<BaseRabbitMQOptions> Config)
{
    public bool CanConnection()
    {
		try
		{
            using var connection = Config.Value.ConnectionFactory.CreateConnection();
            return true;
		}
		catch
		{
			return false;
		}
    }
}
