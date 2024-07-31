using CodeNet.BackgroundJob.Manager;
using CodeNet.BackgroundJob.Models;
using CodeNet.BackgroundJob.Settings;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Logging.Extensions;
using CodeNet.Redis.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CodeNet.BackgroundJob.Extensions;

public static class BackgroundJobServiceExtensions
{
    public static IServiceCollection AddBackgroundJob(this IServiceCollection services, Action<BackgroundJobOptionsBuilder> action)
    {
        var builder = new BackgroundJobOptionsBuilder(services);
        services.AddAppLogger();
        action(builder);
        return services;
    }

    /// <summary>
    /// Use Background Job
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static WebApplication UseBackgroundService(this WebApplication app, string path)
    {
        var serviceScope = app.Services.CreateScope();
        app.UseEndPoint(serviceScope.ServiceProvider, path);

        var tJobs = serviceScope.ServiceProvider.GetServices<IScheduleJob>();
        foreach (var tJob in tJobs)
        {
            var tJobType = tJob.GetType();
            var serviceType = typeof(ICodeNetBackgroundService<>).MakeGenericType(tJobType);
            var backgroundService = serviceScope.ServiceProvider.GetService(serviceType) as ICodeNetBackgroundService ?? throw new NotImplementedException($"'builder.services.AddBackgroundService<{tJobType.Name}>(string cronExpression, TimeSpan? lockExperyTime = null)' not implemented background service.");
            app.Lifetime.ApplicationStarted.Register(async () => await backgroundService.StartAsync(CancellationToken.None));
        }
        return app;
    }

    private static WebApplication UseEndPoint(this WebApplication app, IServiceProvider serviceProvider, string path)
    {
        app.GetServices(serviceProvider, path);
        app.GetServiceDetails(serviceProvider, path);
        app.DeleteDetails(serviceProvider, path);
        app.ChangeServiceStatus(serviceProvider, path);
        app.GetJobStatus(serviceProvider, path);
        app.JobExecute(serviceProvider, path);

        return app;
    }
}
