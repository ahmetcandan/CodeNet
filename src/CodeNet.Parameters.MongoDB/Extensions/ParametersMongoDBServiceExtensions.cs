using Microsoft.Extensions.DependencyInjection;
using CodeNet.Parameters.Manager;
using Microsoft.Extensions.Configuration;
using CodeNet.Parameters.Settings;
using CodeNet.Core.Extensions;
using CodeNet.Parameters.MongoDB.Manager;

namespace CodeNet.Parameters.MongoDB.Extensions;

public static class ParametersMongoDBServiceExtensions
{
    /// <summary>
    /// Add Parameters by MongoDB
    /// </summary>
    /// <param name="services"></param>
    /// <param name="parameterSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters(this IServiceCollection services, IConfigurationSection? parameterSection = null)
    {
        if (parameterSection is not null)
            services.Configure<ParameterSettings>(parameterSection);
        else
            services.Configure<ParameterSettings>(c => { });

        services.AddScoped<IParameterManager, ParameterMongoDBManager>();
        return services.AddCodeNetContext();
    }
}
