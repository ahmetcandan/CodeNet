using CodeNet.Mapper.Configurations;
using CodeNet.Mapper.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        services.AddSingleton(Options.Create(new MapperConfiguration(builder._mapperItems, builder._objectConstructor, builder._arrayConstructors, builder.MaxDepth)));

        return services.AddScoped<ICodeNetMapper, CodeNetMapper>();
    }
}
