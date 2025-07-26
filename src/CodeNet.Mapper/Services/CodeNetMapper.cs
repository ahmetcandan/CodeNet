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

        return (TDestination?)MapTo(typeof(TSource), typeof(TDestination), source, new TDestination(), 0);
    }

    private object? MapTo(Type sourceType, Type destinationType, object source, object destination, int depth = 0)
    {
        if (sourceType == destinationType)
            return source;

        if (depth > _config.MaxDepth || !_config.MapperItems.TryGetValue(MapType.Create(sourceType, destinationType), out MapperItemProperties[]? columns))
            return null;

        for (int i = 0; i < columns.Length; i++)
        {
            var value = columns[i].SourceGetter(source);
            if (value is null)
                continue;
            
            if (columns[i].DestinationType == columns[i].SourceType
                    || columns[i].DestinationTypeIsAssignableFrom(columns[i].SourceProp.PropertyType)
                    || columns[i].DestinationTypeIsEnum)
                columns[i].DestinationSetter(destination, value);
            else if (value is Array sourceArray)
            {
                if (!columns[i].DestinationTypeHasElementType || !columns[i].SourceTypeHasElementType)
                    continue;

                var destinationList = _config.ArrayConstructors[columns[i].DestinationElementType!](sourceArray.Length);

                for (int j = 0; j < sourceArray.Length; j++)
                    destinationList.SetValue(GetArrayItem(sourceArray.GetType().GetElementType()!, columns[i].DestinationElementType!, sourceArray.GetValue(j), depth), j);

                columns[i].DestinationSetter(destination, destinationList);
            }
            else if (columns[i].SourceTypeIsClass)
                columns[i].DestinationSetter(destination, MapTo(columns[i].SourceType, columns[i].DestinationType, value, _config.ObjectConstructors[columns[i].DestinationType].Invoke(), depth + 1));
        }

        return destination;
    }

    private object? GetArrayItem(Type sourceType, Type targetType, object? item, int depth) => item is null || targetType.IsAssignableFrom(sourceType) || targetType.IsEnum ? item : MapTo(item.GetType(), targetType, item, _config.ObjectConstructors[targetType].Invoke(), depth + 1);
}
