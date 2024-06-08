using Microsoft.AspNetCore.Builder;

namespace NetCore.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add RabbitMQ
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddRabbitMQ(this WebApplicationBuilder webBuilder)
    {
        return webBuilder;
    }
}
