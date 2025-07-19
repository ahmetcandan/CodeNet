using CodeNet.Mapper.Configurations;
using CodeNet.Mapper.Extensions;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CodeNet.Mapper.Services;

internal class CodeNetMapper(IOptions<MapperConfiguration> options) : ICodeNetMapper
{
    public TDestination? MapTo<TSource, TDestination>([NotNull] TSource source)
        where TSource : new()
        where TDestination : new()
    {
        return (TDestination?)MapTo(typeof(TSource), typeof(TDestination), source, new TDestination(), 0);
    }

    private object? MapTo(Type sourceType, Type destinationType, object source, object destination, int depth = 0)
    {
        if (depth > GetMaxDepth(sourceType, destinationType))
            return null;

        if (sourceType.Equals(destinationType))
            return source;

        var destinationProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(c => c.Name);
        var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var mapperColumns = GetMapperColumns(sourceType, destinationType);
        if (mapperColumns is null)
            return null;

        for (var i = 0; i < sourceProperties.Length; i++)
        {
            var sourceProp = sourceProperties[i];
            if (!sourceProp.CanRead)
                continue;

            string destinationColumnName = mapperColumns?.ContainsKey(sourceProp.Name) is true ? mapperColumns[sourceProp.Name] : sourceProp.Name;
            var targetProp = destinationProperties[destinationColumnName];
            if (targetProp?.CanWrite is not true)
                continue;

            var value = sourceProp.GetValue(source);
            if (value is null)
                continue;

            if (targetProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType) || targetProp.PropertyType.IsEnum)
                targetProp.SetValue(destination, value, null);
            else if (value is Array sourceArray)
            {
                if (Activator.CreateInstance(targetProp.PropertyType, sourceArray.Length) is not Array destinationList)
                    continue;

                var itemType = targetProp.PropertyType.GetElementType();
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

                targetProp.SetValue(destination, destinationList);
            }
            else if (value is IEnumerable sourceList && !sourceProp.PropertyType.Equals(typeof(string)))
            {
                if (Activator.CreateInstance(targetProp.PropertyType) is not IList destinationList)
                    continue;

                var itemType = targetProp.PropertyType.GetGenericArguments().FirstOrDefault();
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

                targetProp.SetValue(destination, destinationList);
            }
            else if (sourceProp.PropertyType.IsClass && !sourceProp.PropertyType.Equals(targetProp.PropertyType))
            {
                var destinationValue = Activator.CreateInstance(targetProp.PropertyType);
                if (destinationValue is null)
                    continue;

                var propValue = MapTo(sourceProp.PropertyType, targetProp.PropertyType, value, destinationValue, depth + 1);
                if (propValue is not null)
                    targetProp.SetValue(destination, propValue);
            }
        }

        return destination;
    }

    private int GetMaxDepth(Type sourceType, Type destinationType)
    {
        if (options.Value?.MapperItems.ContainsKey(MapType.Create(sourceType, destinationType)) is true)
            return options.Value.MapperItems[MapType.Create(sourceType, destinationType)].MaxDepth
                ?? (options.Value?.MaxDepth ?? MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH);

        if (options.Value?.MapperItems.ContainsKey(MapType.Create(destinationType, sourceType)) is true)
            return options.Value.MapperItems[MapType.Create(destinationType, sourceType)].MaxDepth
                ?? (options.Value?.MaxDepth ?? MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH);

        return options.Value?.MaxDepth ?? MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH;
    }

    private Dictionary<string, string>? GetMapperColumns(Type sourceType, Type destinationType)
    {
        if (options.Value?.MapperItems.ContainsKey(MapType.Create(sourceType, destinationType)) is true)
            return options.Value.MapperItems[MapType.Create(sourceType, destinationType)].Columns;

        if (options.Value?.MapperItems.ContainsKey(MapType.Create(destinationType, sourceType)) is true)
            return options.Value?.MapperItems[MapType.Create(destinationType, sourceType)].Columns;

        return null;
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
