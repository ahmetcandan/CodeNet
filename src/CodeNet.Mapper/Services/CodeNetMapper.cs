using CodeNet.Mapper.Configurations;
using CodeNet.Mapper.Extensions;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CodeNet.Mapper.Services;

internal class CodeNetMapper(IOptions<MapperConfiguration> options) : ICodeNetMapper
{
    public TDestination MapTo<TSource, TDestination>([NotNull] TSource source)
        where TDestination : new()
        where TSource : new()
    {
        return MapTo<TSource, TDestination>(source, 0);
    }

    private TDestination MapTo<TSource, TDestination>([NotNull] TSource source, int depth = 0)
        where TDestination : new()
        where TSource : new()
    {
        return source is null
            ? throw new ArgumentNullException(nameof(source))
            : (TDestination)MapTo(typeof(TSource), typeof(TDestination), source, new TDestination(), depth);
    }

    private object? MapTo(Type sourceType, Type destinationType, object source, object result, int depth = 0)
    {
        if (depth > GetMaxDepth(sourceType, destinationType))
            return null;

        if (sourceType.Equals(destinationType))
            return source;

        var destinationProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var mapperColumns = GetMapperColumns(sourceType, destinationType);

        for (var i = 0; i < sourceProperties.Length; i++)
        {
            var sourceProp = sourceProperties[i];
            if (!sourceProp.CanRead)
                continue;

            string destinationColumnName = mapperColumns?.ContainsKey(sourceProp.Name) is true ? mapperColumns[sourceProp.Name] : sourceProp.Name;
            var targetProp = destinationProperties.FirstOrDefault(c => c.Name.Equals(destinationColumnName) && c.CanWrite);
            if (targetProp is null)
                continue;

            var value = sourceProp.GetValue(source);
            if (value is null)
                continue;

            if (value is IEnumerable sourceList && !sourceProp.PropertyType.Equals(typeof(string)))
            {
                if (Activator.CreateInstance(targetProp.PropertyType) is not IList destinationList)
                    continue;

                var itemType = targetProp.PropertyType.GetGenericArguments().FirstOrDefault();
                if (itemType is null)
                    continue;

                foreach (var item in sourceList)
                {
                    var destinationResult = Activator.CreateInstance(itemType);
                    if (destinationResult is null)
                        continue;

                    var mappedItem = MapTo(item.GetType(), itemType, item, destinationResult, depth + 1);
                    if (mappedItem is not null)
                        destinationList.Add(mappedItem);
                }

                targetProp.SetValue(result, destinationList);
            }
            else if (sourceProp.PropertyType.IsClass && !sourceProp.PropertyType.Equals(targetProp.PropertyType))
            {
                var destinationValue = Activator.CreateInstance(targetProp.PropertyType);
                if (destinationValue is null)
                    continue;

                var propValue = MapTo(sourceProp.PropertyType, targetProp.PropertyType, value, destinationValue, depth + 1);
                if (propValue is not null)
                    targetProp.SetValue(result, propValue);
            }
            else
            {
                if (targetProp.PropertyType.IsEnum || targetProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                    targetProp.SetValue(result, value, null);
            }
        }

        return result;
    }

    private int GetMaxDepth(Type sourceType, Type destinationType)
    {
        int? maxDepth = null;
        var source = options.Value?.MapperItems.FirstOrDefault(c => c.SourceType.Equals(sourceType) && c.DestinationType.Equals(destinationType));
        if (source is not null)
            maxDepth = source.MaxDepth;

        if (!maxDepth.HasValue)
        {
            var destination = options.Value?.MapperItems.FirstOrDefault(c => c.DestinationType.Equals(sourceType) && c.SourceType.Equals(destinationType));
            if (destination is not null)
                maxDepth = destination.MaxDepth;
        }

        return maxDepth ?? options.Value?.MaxDepth ?? MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH;
    }

    private Dictionary<string, string>? GetMapperColumns(Type sourceType, Type destinationType)
    {
        var source = options.Value?.MapperItems.FirstOrDefault(c => c.SourceType.Equals(sourceType) && c.DestinationType.Equals(destinationType));
        if (source is not null)
            return source.Columns;

        var destination = options.Value?.MapperItems.FirstOrDefault(c => c.DestinationType.Equals(sourceType) && c.SourceType.Equals(destinationType));
        return destination?.RevertColumns;
    }
}
