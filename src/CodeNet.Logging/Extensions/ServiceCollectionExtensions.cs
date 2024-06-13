using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace CodeNet.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Logging
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder webBuilder)
    {
        webBuilder.Logging.ClearProviders();
        webBuilder.Logging.AddConsole();
        return webBuilder;
    }
}
