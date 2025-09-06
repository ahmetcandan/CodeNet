using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.HttpClient.Extensions;

/// <summary>
/// HttpClientServiceExtensions
/// </summary>
public static class HttpClientServiceExtensions
{
    /// <summary>
    /// Add HttpRequest
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddHttpRequest(this IServiceCollection services) => services.AddScoped<IHttpRequest, HttpRequest>();
}