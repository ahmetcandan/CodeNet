using CodeNet.Mapper.Configurations;
using Microsoft.Extensions.Options;

namespace CodeNet.Mapper.Services;

public static class MapperCreator
{
    public static ICodeNetMapper Create(Action<MapperConfigurationBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        MapperConfigurationBuilder builder = new();
        action(builder);
        return new CodeNetMapper(Options.Create<MapperConfiguration>(new()
        {
            MapperItems = builder.MapperItems,
            MaxDepth = builder.MaxDepth
        }));
    }
}