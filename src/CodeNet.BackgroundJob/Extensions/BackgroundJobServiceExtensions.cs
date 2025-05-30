﻿using CodeNet.BackgroundJob.Manager;
using CodeNet.BackgroundJob.Middleware;
using CodeNet.BackgroundJob.Models;
using CodeNet.BackgroundJob.Settings;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Logging.Extensions;
using CodeNet.SignalR.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeNet.BackgroundJob.Extensions;

public static class BackgroundJobServiceExtensions
{
    /// <summary>
    /// Add Background Job
    /// </summary>
    /// <param name="services"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddBackgroundJob(this IServiceCollection services, Action<BackgroundJobOptionsBuilder> action)
    {
        var builder = new BackgroundJobOptionsBuilder(services);
        services.AddAppLogger();
        action(builder);

        if (services.Any(c => c.ServiceType.Equals(typeof(BackgroundJobDbContext))))
            services.AddSignalRNotification();

        return services;
    }

    /// <summary>
    /// Use Background Job
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static WebApplication UseBackgroundService(this WebApplication app, string path = "/job")
    {
        var serviceScope = app.Services.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<BackgroundJobDbContext>();
        if (dbContext is not null)
        {
            app.UseEndPoint(serviceScope.ServiceProvider, path);
            app.UseSignalR<StatusChangeHub>($"{path}/jobEvents");
            SetPrivateJobs(dbContext);
        }

        var tJobs = serviceScope.ServiceProvider.GetServices<IScheduleJob>();
        var notificationHub = serviceScope.ServiceProvider.GetService<IHubContext<StatusChangeHub>>();

        foreach (var tJobType in tJobs.Select(c => c.GetType()).Distinct())
        {
            var serviceType = typeof(ICodeNetBackgroundService<>).MakeGenericType(tJobType);
            var backgroundService = serviceScope.ServiceProvider.GetService(serviceType) as ICodeNetBackgroundService ?? throw new NotImplementedException($"'BackgroundJobOptionsBuilder.AddScheduleJob<{tJobType.Name}>(JobOptions options)' not implemented background service.");
            if (notificationHub is not null)
                backgroundService.StatusChanged += async (ReceivedMessageEventArgs e) =>
                {
                    await notificationHub.Clients.All.SendAsync("ChangeStatus_ScheculeJob", e, CancellationToken.None);
                };
            app.Lifetime.ApplicationStarted.Register(async () => await backgroundService.StartAsync(CancellationToken.None));
        }
        return app;
    }

    private static WebApplication UseEndPoint(this WebApplication app, IServiceProvider serviceProvider, string path)
    {
        var authOptions = app.Services.GetService<IOptions<JobAuthOptions>>();

        var routeHandlerBuilders = new RouteHandlerBuilder[6]
        {
            app.GetServices(serviceProvider, path),
            app.GetServiceDetails(serviceProvider, path),
            app.DeleteDetails(serviceProvider, path),
            app.ChangeServiceStatus(serviceProvider, path),
            app.GetJobStatus(serviceProvider, path),
            app.JobExecute(serviceProvider, path)
        };

        switch (authOptions?.Value?.AuthenticationType)
        {
            case AuthenticationType.BasicAuth:
                app.UseMiddleware<BasicAuthMiddleware>();
                break;
            case AuthenticationType.JwtAuth:
                var users = string.IsNullOrEmpty(authOptions.Value?.JwtAuthOptions?.Users) ? [] : (authOptions.Value?.JwtAuthOptions?.Users.Split(',') ?? []);
                var roles = string.IsNullOrEmpty(authOptions.Value?.JwtAuthOptions?.Roles) ? [] : (authOptions.Value?.JwtAuthOptions?.Roles.Split(',') ?? []);
                foreach (var routeHandlerBuilder in routeHandlerBuilders)
                {
                    if (users.Length == 0 && roles.Length == 0)
                        routeHandlerBuilder.RequireAuthorization();
                    else
                        routeHandlerBuilder.RequireAuthorization(policy =>
                        {
                            if (users.Length > 0)
                                policy.RequireAssertion(context => users.Contains(context.User?.Identity?.Name));

                            if (roles.Length > 0)
                                policy.RequireRole(roles);
                        });
                }
                break;
            default:
                break;
        }

        return app;
    }

    private static void SetPrivateJobs(BackgroundJobDbContext dbContext)
    {
        var serviceRepository = new Repository<Job>(dbContext);
        var jobEntities = serviceRepository.Find(c => c.IsActive);
        foreach (var jobEntity in jobEntities)
        {
            jobEntity.IsActive = false;
            serviceRepository.Update(jobEntity);
        }
        dbContext.SaveChanges();
    }
}
