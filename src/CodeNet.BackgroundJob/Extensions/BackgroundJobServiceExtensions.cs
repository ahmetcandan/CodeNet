using CodeNet.BackgroundJob.Manager;
using CodeNet.BackgroundJob.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CodeNet.BackgroundJob.Extensions;

public static class BackgroundJobServiceExtensions
{
    public static IServiceCollection AddBackgroundService<TJob>(this IServiceCollection services, Action<TJob> action, string cronExpression)
        where TJob : class
    {
        return services.AddBackgroundService((TJob tJob) =>
        {
            action(tJob);
            return Task.CompletedTask;
        }, cronExpression);
    }

    public static IServiceCollection AddBackgroundService<TJob>(this IServiceCollection services, Func<TJob, Task> func, string cronExpression)
        where TJob : class
    {
        if (!CronExpression.IsValidExpression(cronExpression))
            throw new ArgumentException($"CronExpression is not valid: '{cronExpression}'");

        services.Configure<BackgroundServiceOptions<TJob>>(c =>
        {
            c.Func = func;
            c.CronExpression = cronExpression;
        });
        services.AddScoped<ICodeNetBackgroundService<TJob>, CodeNetBackgroundService<TJob>>();
        return services.AddScoped<TJob>();
    }

    public static WebApplication UseBackgroundService<TJob>(this WebApplication app)
    {
        var serviceScope = app.Services.CreateScope();
        var backgroundService = serviceScope.ServiceProvider.GetService<ICodeNetBackgroundService<TJob>>() ?? throw new NotImplementedException($"'{nameof(TJob)}' not implemented background service.");

        app.Lifetime.ApplicationStarted.Register(async () =>
        {
            await backgroundService.StartAsync();
        });

        return app;
    }
}
