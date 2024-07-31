using CodeNet.MongoDB.Extensions;
using CodeNet.Parameters.Extensions;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.MongoDB.Manager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Parameters.MongoDB.Extensions;

public static class ParametersMongoDBServiceExtensions
{
    /// <summary>
    /// Add Parameters by MongoDB
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="mongoSection"></param>
    /// <returns></returns>
    public static ParameterOptionsBuilder AddMongoDb(this ParameterOptionsBuilder builder, IConfigurationSection mongoSection)
    {
        builder.Services.AddMongoDB(mongoSection);
        builder.Services.AddScoped<IParameterManager, ParameterMongoDBManager>();
        return builder;
    }
}
