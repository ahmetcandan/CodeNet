using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;

namespace CodeNet.MakerChecker.Extensions;

public static class MapperServiceExtensions
{
    /// <summary>
    /// Add Mapper
    /// </summary>
    /// <param name="webBuilder"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMapper(this IHostApplicationBuilder webBuilder)
    {
        webBuilder.Services.AddScoped<IMapper, Mapper>();
        return webBuilder;
    }
}
