using CodeNet.BackgroundJob.Extensions;
using CodeNet.BackgroundJob.Settings;
using CodeNet.Outbox.Builder;
using CodeNet.Outbox.Settings;
using CodeNet.RabbitMQ.Outbox.Services;
using CodeNet.RabbitMQ.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CodeNet.RabbitMQ.Outbox.Extensions;

public static class RabbitMQOutboxServiceExtension
{
    /// <summary>
    /// Add RabbitMQ Outbox Module
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddRabbitMQOutboxModule(this IServiceCollection services, IConfigurationSection configuration, Action<OutboxOptionsBuilder> action)
    {
        var options = configuration.Get<OutboxSettings>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json");
        return services.AddRabbitMQOutboxModule(options, action);
    }

    public static IServiceCollection AddRabbitMQOutboxModule(this IServiceCollection services, OutboxSettings settings, Action<OutboxOptionsBuilder> action)
    {
        OutboxOptionsBuilder builder = new(services);
        action(builder);

        var producerServices = services.Where(c => IsRabbitMQProducerServiceType(c.ServiceType)).ToList();
        int index = 0;
        foreach (var producerService in producerServices)
        {
            index++;
            var outboxProducerServiceType = producerService.ServiceType.Equals(typeof(RabbitMQProducerService))
                ? typeof(OutboxRabbitMQProducerService)
                : typeof(OutboxRabbitMQProducerService<>).MakeGenericType(producerService.ServiceType);

            services.Replace(ServiceDescriptor.Scoped(producerService.ServiceType, outboxProducerServiceType));
            services.AddScoped(outboxProducerServiceType);
            var sendServiceType = producerService.ServiceType.Equals(typeof(RabbitMQProducerService))
                ? typeof(RabbitMQSendService)
                : typeof(RabbitMQSendService<,>).MakeGenericType(outboxProducerServiceType, producerService.ServiceType);
            services.AddScoped(sendServiceType);
            services.AddBackgroundJob(c =>
            {
                var addScheduleJobMethod = typeof(BackgroundJobOptionsBuilder).GetMethod("AddScheduleJob", [typeof(string), typeof(TimeSpan), typeof(JobOptions)])?.MakeGenericMethod(sendServiceType);
                addScheduleJobMethod?.Invoke(c, [$"{outboxProducerServiceType.Name}[{index:000}]", settings.SendPeriod, new JobOptions(settings?.LockSettings?.LockTime ?? TimeSpan.FromMinutes(1), settings?.LockSettings?.TimeOut ?? TimeSpan.FromMinutes(1))]);
            });
        }

        return services.Configure<OutboxSettings>(c =>
        {
            c.SendPeriod = settings.SendPeriod;
            c.PrefetchCount = settings.PrefetchCount;
            c.LockSettings = settings.LockSettings;
        });
    }

    private static bool IsRabbitMQProducerServiceType(Type type) 
        => type.Equals(typeof(RabbitMQProducerService)) || (type.BaseType is not null && !type.BaseType.Equals(typeof(object)) && IsRabbitMQProducerServiceType(type.BaseType));

    /// <summary>
    /// Use RabbitMQ Outbox Module
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseRabbitMQOutboxModule(this WebApplication app) => app.UseBackgroundService();
}
