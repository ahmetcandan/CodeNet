using CodeNet.Core.Extensions;
using CodeNet.MongoDB.Extensions;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.MongoDB.Manager;
using CodeNet.Parameters.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Parameters.MongoDB.Extensions;

public static class ParametersMongoDBServiceExtensions
{
    /// <summary>
    /// Add Parameters by MongoDB
    /// </summary>
    /// <param name="services"></param>
    /// <param name="mongoSection"></param>
    /// <param name="parameterSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters(this IServiceCollection services, IConfigurationSection mongoSection, IConfigurationSection? parameterSection = null)
    {
        if (parameterSection is not null)
            services.Configure<ParameterSettings>(parameterSection);
        else
            services.Configure<ParameterSettings>(c => { });

        services.AddMongoDB(mongoSection);
        services.AddScoped<IParameterManager, ParameterMongoDBManager>();
        return services.AddCodeNetContext();
    }
}
