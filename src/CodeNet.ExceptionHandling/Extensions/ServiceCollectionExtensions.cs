using Microsoft.AspNetCore.Builder;

namespace CodeNet.ExceptionHandling.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Use Error Controller
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseErrorController(this WebApplication app)
    {
        app.UseExceptionHandler("/Error");
        return app;
    }
}
