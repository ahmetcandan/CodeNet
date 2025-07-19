using CodeNet.Mapper.Configurations;
using CodeNet.Mapper.Extensions;
using Microsoft.Extensions.Options;

namespace CodeNet.Mapper.Services;

public static class MapperCreator
{
    public static ICodeNetMapper Create(Action<MapperConfigurationBuilder>? action = null)
    {
        if (action is not null)
        {
            MapperConfigurationBuilder builder = new();
            action(builder);
            return new CodeNetMapper(Options.Create<MapperConfiguration>(new()
            {
                MapperItems = builder.MapperItems,
                MaxDepth = builder.MaxDepth
            }));
        }
        else
            return new CodeNetMapper(Options.Create<MapperConfiguration>(new()
            {
                MapperItems = [],
                MaxDepth = MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH
            }));
    }
}