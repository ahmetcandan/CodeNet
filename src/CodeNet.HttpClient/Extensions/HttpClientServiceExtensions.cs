using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeNet.HttpClient.Extensions;

/// <summary>
/// HttpClientServiceExtensions
/// </summary>
public static class HttpClientServiceExtensions
{
    /// <summary>
    /// Add HttpClient
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddHttpClient(this IHostApplicationBuilder webBuilder)
    {
        webBuilder.Services.AddScoped<IHttpRequest, HttpRequest>();
        return webBuilder;
    }
}