using CodeNet.RabbitMQ.Settings;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.RabbitMQ;

internal class TestRabbitMqService(IOptions<BaseRabbitMQOptions> options)
{
    public bool CanConnection()
    {
        try
        {
            using var connection = options.Value.ConnectionFactory.CreateConnection();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
