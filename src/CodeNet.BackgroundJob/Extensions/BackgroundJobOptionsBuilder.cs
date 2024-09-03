using CodeNet.BackgroundJob.Manager;
using CodeNet.BackgroundJob.Models;
using CodeNet.BackgroundJob.Settings;
using CodeNet.Core.Enums;
using CodeNet.Core.Extensions;
using CodeNet.Redis.Extensions;
using Cronos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.BackgroundJob.Extensions;

public class BackgroundJobOptionsBuilder(IServiceCollection services)
{
    /// <summary>
    /// Add Redis for Lock
    /// </summary>
    /// <param name="redisSection"></param>
    /// <returns></returns>
    public BackgroundJobOptionsBuilder AddRedis(IConfigurationSection redisSection)
    {
        services.AddScoped<IJobLock, JobLock>();
        services.AddRedisDistributedLock(redisSection);
        return this;
    }

    /// <summary>
    /// Add Schedule Job
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    /// <param name="serviceName"></param>
    /// <param name="cronExpression"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public BackgroundJobOptionsBuilder AddScheduleJob<TJob>(string serviceName, string cronExpression, JobOptions options)
        where TJob : class, IScheduleJob
    {
        var valid = CronExpression.TryParse(cronExpression, out CronExpression cron);
        if (!valid)
            throw new ArgumentException($"CronExpression is not valid: '{cronExpression}'");

        services.Configure<JobOptions<TJob>>(c =>
        {
            c.Cron = cron;
            c.PeriodTime = null;
            c.ExpryTime = options.ExpryTime;
            c.Timeout = options.Timeout;
            c.ServiceType = typeof(TJob).ToString();
            c.Title = serviceName;
        });

        AddServices<TJob>(services);
        return this;
    }

    /// <summary>
    /// Add Schedule Job
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    /// <param name="serviceName"></param>
    /// <param name="periodTime"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public BackgroundJobOptionsBuilder AddScheduleJob<TJob>(string serviceName, TimeSpan periodTime, JobOptions options)
        where TJob : class, IScheduleJob
    {
        services.Configure<JobOptions<TJob>>(c =>
        {
            c.Cron = null;
            c.PeriodTime = periodTime;
            c.ExpryTime = options.ExpryTime;
            c.Timeout = options.Timeout;
            c.ServiceType = typeof(TJob).ToString();
            c.Title = serviceName;
        });

        AddServices<TJob>(services);
        return this;
    }

    private static void AddServices<TJob>(IServiceCollection services)
        where TJob : class, IScheduleJob
    {
        services.AddSingleton<IScheduleJob, TJob>();
        services.AddSingleton<ICodeNetBackgroundService<TJob>, CodeNetBackgroundService<TJob>>();
    }

    /// <summary>
    /// Add DbContext
    /// </summary>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public BackgroundJobOptionsBuilder AddDbContext(Action<DbContextOptionsBuilder> optionsAction)
    {
        services.AddDbContext<BackgroundJobDbContext>(optionsAction);
        return this;
    }

    /// <summary>
    /// Add JWT Authentication
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be AuthenticationSettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be AuthenticationSettingsWithSymmetricKey.
    /// </summary>
    /// <param name="securityKeyType"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    public BackgroundJobOptionsBuilder AddJwtAuth(SecurityKeyType securityKeyType, IConfigurationSection identitySection, string users = "", string roles = "")
    {
        AddCurrentAuth(users, roles);
        new CodeNetOptionsBuilder(services).AddAuthentication(securityKeyType, identitySection);
        return this;
    }

    /// <summary>
    /// Add Current Authentication
    /// If SecurityKeyType is AsymmetricKey, IdentitySection should be AuthenticationSettingsWithAsymmetricKey.
    /// Else if SecurityKeyType is SymmetricKey, IdentitySection should be AuthenticationSettingsWithSymmetricKey.
    /// </summary>
    /// <param name="securityKeyType"></param>
    /// <param name="identitySection"></param>
    /// <returns></returns>
    public BackgroundJobOptionsBuilder AddCurrentAuth(string users = "", string roles = "")
    {
        services.Configure<JobAuthOptions>(c =>
        {
            c.AuthenticationType = AuthenticationType.JwtAuth;
            c.JwtAuthOptions = new JobJwtAuthOptions
            {
                Roles = roles,
                Users = users
            };
        });
        return this;
    }

    /// <summary>
    /// Add Basic Authentication
    /// </summary>
    /// <param name="userPass"></param>Username & Password
    /// <returns></returns>
    public BackgroundJobOptionsBuilder AddBasicAuth(Dictionary<string, string> userPass)
    {
        services.Configure<JobAuthOptions>(c =>
        {
            c.AuthenticationType = AuthenticationType.BasicAuth;
            c.BasicAuthOptions = new JobBasicAuthOptions
            {
                UserPass = userPass
            };
        });
        return this;
    }
}
