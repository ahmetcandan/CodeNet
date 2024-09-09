using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

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
        return services.AddScoped<IMapper, AutoMapper.Mapper>();
    }
}
