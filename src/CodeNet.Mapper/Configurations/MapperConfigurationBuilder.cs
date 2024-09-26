using CodeNet.Mapper.Extensions;

namespace CodeNet.Mapper.Configurations;

public class MapperConfigurationBuilder
{
    internal IList<MapperItem> MapperItems { get; } = [];
    internal int MaxDepth { get; private set; } = MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH;

    public MapperColumnBuilder<TSource, TDestination> CreateMap<TSource, TDestination>()
        where TSource : new()
        where TDestination : new()
    {
        var map = new MapperColumnBuilder<TSource, TDestination>();
        MapperItems.Add(map.MapperItem);
        return map;
    }

    public void SetMaxDepth(int maxDepth)
    {
        MaxDepth = maxDepth;
    }
}
