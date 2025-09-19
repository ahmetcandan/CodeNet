using CodeNet.ExceptionHandling.Middleware;
using CodeNet.Logging.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.ExceptionHandling.Extensions;

public static class ExceptionServiceExtensions
{
    /// <summary>
    /// Add Default Error Message
    /// </summary>
    /// <param name="services"></param>
    /// <param name="sectionName">ErrorResponseMessage</param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultErrorMessage(this IServiceCollection services, IConfiguration configuration, string sectionName)
        => services.AddDefaultErrorMessage(configuration.GetSection(sectionName));

    /// <summary>
    /// Json is ProblemDetails
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurationSection">ProblemDetails</param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultErrorMessage(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        services.AddProblemDetails();
        services.AddAppLogger();
        return services.Configure<ProblemDetails>(configurationSection);
    }

    /// <summary>
    /// Use Error Controller
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app) => app.UseMiddleware<ExceptionHandlerMiddleware>();
}
