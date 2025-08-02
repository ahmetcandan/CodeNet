using CodeNet.Mapper.Configurations;
using Microsoft.Extensions.Options;

namespace CodeNet.Mapper.Services;

internal class CodeNetMapper(IOptions<MapperConfiguration> options) : ICodeNetMapper
{
    readonly MapperConfiguration _config = options.Value ?? throw new ArgumentNullException(nameof(MapperConfiguration));

    public TDestination? MapTo<TSource, TDestination>(TSource source)
        where TSource : new()
        where TDestination : new()
    {
        if (source is null)
            return default;

        return (TDestination?)MapToObject(_config, typeof(TSource), typeof(TDestination), source, 0);
    }

    private static object? MapToObject(MapperConfiguration _config, Type sourceType, Type destinationType, object? source, int depth = default)
    {
        if (source is null || sourceType == destinationType)
            return source;

        if (depth > _config.MaxDepth || !_config.MapperItems.TryGetValue(MapType.Create(sourceType, destinationType), out MapperItemProperties[]? columns))
            return null;

        var destination = _config.ObjectConstructors[destinationType]();
        if (destination is null)
            return null;

        for (int i = 0; i < columns.Length; i++)
            columns[i].DestinationSetter(destination, SetColumnValue(_config, columns[i], columns[i].SourceGetter(source), columns[i].DestinationTypeHasElementType ? _config.ArrayConstructors[columns[i].DestinationElementType!] : null, depth));

        return destination;
    }

    private static object? SetColumnValue(MapperConfiguration _config, MapperItemProperties column, object? value, Func<int, Array>? arrayConstractor, int depth)
    {
        if (value is null || column.ColumnsIsEquals)
            return value;
        else if (value is Array sourceArray && column.ColumnHasElementType)
        {
            var destinationList = arrayConstractor!(sourceArray.Length);
            for (int i = 0; i < sourceArray.Length; i++)
                destinationList.SetValue(column.ElementTypeIsAssignableEnum
                                            ? sourceArray.GetValue(i)
                                            : MapToObject(_config, column.SourceElementType!, column.DestinationElementType!, sourceArray.GetValue(i), depth + 1),
                                        i);
            return destinationList;
        }
        else if (column.SourceTypeIsClass && column.SourceType != typeof(string))
            return MapToObject(_config, column.SourceType, column.DestinationType, value, depth + 1);

        return null;
    }
}
