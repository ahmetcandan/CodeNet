using CodeNet.ExceptionHandling.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.ExceptionHandling.Extensions;

public static class ExceptionServiceExtensions
{
    /// <summary>
    /// Add Default Error Message
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName">ErrorResponseMessage</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddDefaultErrorMessage(this IHostApplicationBuilder webBuilder, string sectionName)
    {
        webBuilder.Services.Configure<ErrorResponseMessage>(webBuilder.Configuration.GetSection(sectionName));
        return webBuilder;
    }

    /// <summary>
    /// Use Error Controller
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlerMiddleware?>();
    }
}
