using CodeNet.Mapper.Configurations;
using Microsoft.Extensions.Options;

namespace CodeNet.Mapper.Services;

public static class MapperCreator
{
    public static ICodeNetMapper Create(Action<MapperConfigurationBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        MapperConfigurationBuilder builder = new();
        action(builder);
        return new CodeNetMapper(Options.Create(new MapperConfiguration(builder._mapperItems, builder._objectConstructor, builder._arrayConstructors, builder._listConstructors, builder.MaxDepth)));
    }
}