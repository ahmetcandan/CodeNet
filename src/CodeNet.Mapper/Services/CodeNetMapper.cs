using CodeNet.Mapper.Configurations;
using Microsoft.Extensions.Options;
using System.Collections;

namespace CodeNet.Mapper.Services;

internal class CodeNetMapper(IOptions<MapperConfiguration> options) : ICodeNetMapper
{
    public TDestination? MapTo<TSource, TDestination>(TSource source)
        where TSource : new()
        where TDestination : new()
    {
        ArgumentNullException.ThrowIfNull(options.Value, nameof(MapperConfiguration));

        if (source is null)
            return default;

        return (TDestination?)MapTo(typeof(TSource), typeof(TDestination), source, new TDestination(), 0);
    }

    private object? MapTo(Type sourceType, Type destinationType, object source, object destination, int depth = 0)
    {
        if (sourceType.Equals(destinationType))
            return source;

        var columns = options.Value.MapperItems[MapType.Create(sourceType, destinationType)];
        if (columns is null)
            return null;

        if (depth > options.Value.MaxDepth)
            return null;

        foreach (var column in columns)
        {
            var value = column.SourceProp.GetValue(source);
            if (value is null)
                continue;
            
            if (column.DestinationProp.PropertyType.IsAssignableFrom(column.SourceProp.PropertyType) || column.DestinationProp.PropertyType.IsEnum)
                column.DestinationProp.SetValue(destination, value);
            else if (value is Array sourceArray)
            {
                if (Activator.CreateInstance(column.DestinationProp.PropertyType, sourceArray.Length) is not Array destinationList)
                    continue;

                var itemType = column.DestinationProp.PropertyType.GetElementType();
                if (itemType is null)
                    continue;

                var sourceItemType = sourceArray.GetType().GetElementType();
                if (sourceItemType is null)
                    continue;

                int j = 0;
                foreach (var item in sourceArray)
                {
                    var mappedItem = GetArrayItem(sourceItemType, itemType, item, depth);
                    if (mappedItem is not null)
                        destinationList.SetValue(mappedItem, j++);
                }

                column.DestinationProp.SetValue(destination, destinationList);
            }
            else if (value is IEnumerable sourceList && !column.SourceProp.PropertyType.Equals(typeof(string)))
            {
                if (Activator.CreateInstance(column.DestinationProp.PropertyType, sourceList) is not IList destinationList)
                    continue;

                var itemType = column.DestinationProp.PropertyType.GetGenericArguments().FirstOrDefault();
                if (itemType is null)
                    continue;

                var sourceItemType = sourceList.GetType().GetGenericArguments().FirstOrDefault();
                if (sourceItemType is null)
                    continue;

                foreach (var item in sourceList)
                {
                    var mappedItem = GetArrayItem(sourceItemType, itemType, item, depth);
                    if (mappedItem is not null)
                        destinationList.Add(mappedItem);
                }

                column.DestinationProp.SetValue(destination, destinationList);
            }
            else if (column.SourceProp.PropertyType.IsClass && !column.SourceProp.PropertyType.Equals(column.DestinationProp.PropertyType))
            {
                var destinationValue = Activator.CreateInstance(column.DestinationProp.PropertyType);
                if (destinationValue is null)
                    continue;

                var propValue = MapTo(column.SourceProp.PropertyType, column.DestinationProp.PropertyType, value, destinationValue, depth + 1);
                if (propValue is not null)
                    column.DestinationProp.SetValue(destination, propValue);
            }
        }

        return destination;
    }

    private object? GetArrayItem(Type sourceType, Type targetType, object item, int depth)
    {
        if (targetType.IsAssignableFrom(sourceType) || targetType.IsEnum)
            return item;
        else if (targetType.IsClass)
        {
            var destinationResult = Activator.CreateInstance(targetType);
            if (destinationResult is null)
                return null;

            return MapTo(item.GetType(), targetType, item, destinationResult, depth + 1);
        }
        else
            return null;
    }
}
