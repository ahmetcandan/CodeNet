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
            SourceGetters = builder.SourceGetters,
            DestinationSetters = builder.DestinationSetters,
            MaxDepth = builder.MaxDepth,
            ObjectConstructor = builder.ObjectConstructor,
            ArrayConstructors = builder.ArrayConstructors
        }));
    }
}