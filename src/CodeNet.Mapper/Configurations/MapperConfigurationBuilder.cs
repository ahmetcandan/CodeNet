using CodeNet.Mapper.Extensions;
using System.Reflection;

namespace CodeNet.Mapper.Configurations;

public class MapperConfigurationBuilder
{
    internal Dictionary<MapType, MapperItemProperties[]> MapperItems { get; } = [];
    internal int MaxDepth { get; private set; } = MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH;

    public MapperColumnBuilder<TSource, TDestination> CreateMap<TSource, TDestination>(Action<MapperColumnBuilder<TSource, TDestination>>? action = null)
        where TSource : new()
        where TDestination : new()
    {
        MapperColumnBuilder<TSource, TDestination> map = new();

        if (action is not null)
            action(map);

        SetColumnProperties(MapperColumnBuilder<TSource, TDestination>.MapType, map.Columns);
        SetColumnProperties(MapperColumnBuilder<TDestination, TSource>.MapType, map.Columns.ToDictionary(c => c.Value, c => c.Key));
        return map;
    }

    private bool SetColumnProperties(MapType mapType, Dictionary<string, string> mapperColumns)
    {
        var sourceTypeProperties = mapType.SourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        List<MapperItemProperties> properties = [];
        foreach (var sourceProp in sourceTypeProperties) 
        {
            if (!sourceProp.CanRead)
                continue;

            var destinationProp = mapType.DestinationType.GetProperty(mapperColumns?.TryGetValue(sourceProp.Name, out string? sourceColumn) is true ? sourceColumn : sourceProp.Name, BindingFlags.Public | BindingFlags.Instance);
            if (destinationProp?.CanWrite is not true)
                continue;

            properties.Add(new MapperItemProperties { SourceProp = sourceProp, DestinationProp = destinationProp });
        }

        return MapperItems.TryAdd(mapType, [.. properties]);
    }

    public void SetMaxDepth(int maxDepth)
    {
        MaxDepth = maxDepth;
    }
}
