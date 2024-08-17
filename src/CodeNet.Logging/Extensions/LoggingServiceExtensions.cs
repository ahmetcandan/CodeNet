using CodeNet.Core.Extensions;
using CodeNet.Logging.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Logging.Extensions;

public static class LoggingServiceExtensions
{
    /// <summary>
    /// Add Logging
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppLogger(this IServiceCollection services)
    {
        new CodeNetOptionsBuilder(services).AddCodeNetContext();
        return services.AddScoped<IAppLogger, AppLogger>();
    }

    /// <summary>
    /// Use Logging
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoggingMiddleware>();
    }
}
