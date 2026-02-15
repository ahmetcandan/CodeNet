using CodeNet.Mapper.Configurations;
using Microsoft.Extensions.Options;
using System.Collections;

namespace CodeNet.Mapper.Services;

internal class CodeNetMapper(IOptions<MapperConfiguration> options) : ICodeNetMapper
{
    private readonly MapperConfiguration _config = options.Value ?? throw new ArgumentNullException(nameof(MapperConfiguration));
    private readonly Dictionary<MapType, Dictionary<object, object>> _cache = [];

    public TDestination? MapTo<TSource, TDestination>(TSource source)
        where TSource : new()
        where TDestination : new()
        => source is null ? default : (TDestination?)MapToObject(_config, typeof(TSource), typeof(TDestination), source, _cache, 0);

    private static object? MapToObject(MapperConfiguration _config, Type sourceType, Type destinationType, object? source, Dictionary<MapType, Dictionary<object, object>> memoryCache, int depth = default)
    {
        if (source is null || sourceType == destinationType)
            return source;

        var mapType = MapType.Create(sourceType, destinationType);
        if (depth > _config.MaxDepth || !_config.MapperItems.TryGetValue(mapType, out MapperItemProperties[]? columns))
            return null;

        if (memoryCache.TryGetValue(mapType, out Dictionary<object, object>? cache) && cache.TryGetValue(source, out object? cachedValue))
            return cachedValue;

        var destination = _config.ObjectConstructors[destinationType]();
        if (destination is null)
            return null;

        for (int i = 0; i < columns.Length; i++)
            columns[i].DestinationSetter(destination, SetColumnValue(_config, columns[i], columns[i].SourceGetter(source), columns[i].DestinationTypeHasElementType ? _config.ArrayConstructors[columns[i].DestinationElementType!] : null, depth, memoryCache));

        if (cache is not null)
            cache.TryAdd(source, destination);
        else
            cache = new Dictionary<object, object> { { source, destination } };
        memoryCache[mapType] = cache;

        return destination;
    }

    private static object? SetColumnValue(MapperConfiguration _config, MapperItemProperties column, object? value, Func<int, Array>? arrayConstractor, int depth, Dictionary<MapType, Dictionary<object, object>> memoryCache)
    {
        if (value is null || column.ColumnsIsEquals)
            return value;
        else if (value is Array sourceArray && column.ColumnHasElementType)
        {
            var destinationArray = arrayConstractor!(sourceArray.Length);
            for (int i = 0; i < sourceArray.Length; i++)
                destinationArray.SetValue(column.ElementTypeIsAssignableEnum
                                            ? sourceArray.GetValue(i)
                                            : MapToObject(_config, column.SourceElementType!, column.DestinationElementType!, sourceArray.GetValue(i), memoryCache, depth + 1),
                                        i);
            return destinationArray;
        }
        else if (value is IEnumerable sourceList)
        {
            var destinationList = (_config.ListConstructors[column.DestinationElementType!].Invoke() as IList)!;
            foreach (var item in sourceList)
                destinationList.Add(column.ElementTypeIsAssignableEnum
                                        ? item
                                        : MapToObject(_config, column.SourceElementType!, column.DestinationElementType!, item, memoryCache, depth + 1));
            return destinationList;
        }
        else if (column.SourceTypeIsClass && column.SourceType != typeof(string))
            return MapToObject(_config, column.SourceType, column.DestinationType, value, memoryCache, depth + 1);

        return null;
    }
}
