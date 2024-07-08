using CodeNet.Logging.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.Logging.Extensions;

public static class LoggingServiceExtensions
{
    /// <summary>
    /// Add Logging
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IHostApplicationBuilder AddLogging(this IHostApplicationBuilder webBuilder)
    {
        webBuilder.Services.AddScoped<IAppLogger, AppLogger>();
        return webBuilder;
    }

    /// <summary>
    /// Use Logging
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoggingMiddleware?>();
    }
}
