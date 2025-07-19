using CodeNet.Mapper.Extensions;

namespace CodeNet.Mapper.Configurations;

public class MapperConfigurationBuilder
{
    internal Dictionary<MapType, MapperItem> MapperItems { get; } = [];
    internal int MaxDepth { get; private set; } = MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH;

    public MapperColumnBuilder<TSource, TDestination> CreateMap<TSource, TDestination>(Action<MapperColumnBuilder<TSource, TDestination>>? action = null)
        where TSource : new()
        where TDestination : new()
    {
        MapperColumnBuilder<TSource, TDestination> map = new();
        if (action is not null)
            action(map);

        if (MapperItems.ContainsKey(MapperColumnBuilder<TSource, TDestination>.MapType) is false)
            MapperItems.Add(MapperColumnBuilder<TSource, TDestination>.MapType, map.MapperItem);
        return map;
    }

    public void SetMaxDepth(int maxDepth)
    {
        MaxDepth = maxDepth;
    }
}
