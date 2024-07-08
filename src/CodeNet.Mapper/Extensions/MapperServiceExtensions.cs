using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace CodeNet.MakerChecker.Extensions;

public static class MapperServiceExtensions
{
    /// <summary>
    /// Add Mapper
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services.AddScoped<IMapper, Mapper>();
    }
}
