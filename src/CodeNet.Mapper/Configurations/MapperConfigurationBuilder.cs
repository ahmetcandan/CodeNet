using System.Linq.Expressions;

namespace CodeNet.Mapper.Configurations;

public class MapperConfigurationBuilder
{
    private readonly IList<MapperItem> _mappers = [];

    internal IList<MapperItem> GetMapperItems { get { return _mappers; } }

    public MapperColumnBuilder<TSource, TDestination> CreateMap<TSource, TDestination>()
        where TSource : new()
        where TDestination : new()
    {
        var map = new MapperColumnBuilder<TSource, TDestination>();
        _mappers.Add(map.GetMapperItem);
        return map;
    }
}
