using CodeNet.Mapper.Configurations;
using CodeNet.Mapper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Mapper.Extensions;

public static class MapperServiceExtensions
{
    /// <summary>
    /// Add Mapper
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMapper(this IServiceCollection services, Action<MapperConfigurationBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        MapperConfigurationBuilder builder = new();
        action(builder);
        services.Configure<MapperConfiguration>(c =>
        {
            c.MapperItems = builder.MapperItems;
            c.MaxDepth = builder.MaxDepth;
        });

        return services.AddScoped<ICodeNetMapper, CodeNetMapper>();
    }
}
