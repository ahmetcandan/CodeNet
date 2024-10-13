using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.RabbitMQ.Extensions;

public static class HealthCheckRabbitMqServiceExtensions
{
    private const string _name = "rabbit-mq";

    /// <summary>
    /// Add RabbitMQ Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="name"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHealthChecksBuilder AddRabbitMqHealthCheck(this IHealthChecksBuilder builder, IServiceCollection services, IConfigurationSection configuration, string name = _name, TimeSpan? timeSpan = null)
    {
        return builder.AddRabbitMqHealthCheck(services, configuration.Get<HealthCheckRabitMQSettings>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json"), name, timeSpan);
    }

    /// <summary>
    /// Add RabbitMQ Health Check
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <param name="name"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static IHealthChecksBuilder AddRabbitMqHealthCheck(this IHealthChecksBuilder builder, IServiceCollection services, HealthCheckRabitMQSettings options, string name = _name, TimeSpan? timeSpan = null)
    {
        services.Configure<HealthCheckRabitMQSettings>(c =>
        {
            c.QueueBind = options.QueueBind;
            c.Queue = options.Queue;
            c.DeclareQueue = options.DeclareQueue;
            c.Durable = options.Durable;
            c.AutoDelete = options.AutoDelete;
            c.Arguments = options.Arguments;
            c.QueueBindArguments = options.QueueBindArguments;
            c.ConnectionFactory = options.ConnectionFactory;
            c.DeclareExchange = options.DeclareExchange;
            c.Exchange = options.Exchange;
            c.RoutingKey = options.RoutingKey;
            c.ExchangeArguments = options.ExchangeArguments;
            c.ExchangeType = options.ExchangeType;
            c.Exclusive = options.Exclusive;
        });
        return builder.AddCheck<RabbitMqHealthCheck>(name, HealthStatus.Unhealthy, ["rabbit-mq", "queue"], timeSpan ?? TimeSpan.FromSeconds(5));
    }
}
