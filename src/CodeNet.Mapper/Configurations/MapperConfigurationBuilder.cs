using CodeNet.Mapper.Extensions;

namespace CodeNet.Mapper.Configurations;

public class MapperConfigurationBuilder
{
    private readonly IList<MapperItem> _mappers = [];
    private int _maxDepth = MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH;

    internal IList<MapperItem> MapperItems { get { return _mappers; } }
    internal int MaxDepth { get { return _maxDepth; } }

    public MapperColumnBuilder<TSource, TDestination> CreateMap<TSource, TDestination>()
        where TSource : new()
        where TDestination : new()
    {
        var map = new MapperColumnBuilder<TSource, TDestination>();
        _mappers.Add(map.MapperItem);
        return map;
    }

    public void SetMaxDepth(int maxDepth)
    {
        _maxDepth = maxDepth;
    }
}
