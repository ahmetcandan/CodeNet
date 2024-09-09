using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace CodeNet.HealthCheck.Kafka;

internal class TestKafkaService(IOptions<HealthCheckKafkaSettings> options)
{
    public bool CanConnection()
    {
        try
		{
            using var producer = new ProducerBuilder<Null, string>(options.Value.Config).Build();
            return true;
		}
		catch
		{
			return false;
		}
    }
}
