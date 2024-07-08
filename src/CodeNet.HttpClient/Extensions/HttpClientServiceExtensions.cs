using Microsoft.Extensions.DependencyInjection;

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
    public static IServiceCollection AddHttpClient(this IServiceCollection services)
    {
        return services.AddScoped<IHttpRequest, HttpRequest>();
    }
}