using CodeNet.Email.Repositories;
using CodeNet.Email.Services;
using CodeNet.Email.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Email.Extensions;

public static class MailServiceExtensions
{
    /// <summary>
    /// Add Email Service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="section"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfigurationSection section, Action<MailServiceOptionsBuilder> action = null)
    {
        var options = section.Get<SmtpOptions>() ?? throw new ArgumentNullException($"'{section.Path}' is null or empty in appSettings.json");
        return AddEmailService(services, options, action);
    }

    /// <summary>
    /// Add Email Service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddEmailService(this IServiceCollection services, SmtpOptions options, Action<MailServiceOptionsBuilder> action = null)
    {
        MailServiceOptionsBuilder builder = new(services);
        action.Invoke(builder);

        services.Configure<MailOptions>(c =>
        {
            c.SmtpClient = options.SmtpClient;
            c.EmailAddress = options.EmailAddress;
            c.HasMongoDB = builder.HasMongoDB;
        });

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<OutboxRepositories>();
        services.AddScoped<MailTemplateRepositories>();

        return services;
    }
}
