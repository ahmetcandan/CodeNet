using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CodeNet.Mapper.Services;

public class CodeNetMapper
{
    public static TDestination MapTo<TSource, TDestination>([NotNull] TSource source)
        where TDestination : new()
        where TSource : new()
    {
        return MapTo<TSource, TDestination>(source, 0);
    }

    private static TDestination MapTo<TSource, TDestination>([NotNull] TSource source, int depth = 0)
        where TDestination : new()
        where TSource : new()
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return (TDestination)MapTo(typeof(TSource), typeof(TDestination), source, new TDestination(), depth);
    }

    private static object MapTo(Type sourceType, Type destinationType, object source, object result, int depth = 0)
    {
        if (sourceType.Equals(destinationType))
            return source;

        var destinationProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        for (var i = 0; i < sourceProperties.Length; i++)
        {
            var sourceProp = sourceProperties[i];
            if (!sourceProp.CanRead)
                continue;

            var targetProp = destinationProperties.FirstOrDefault(c => c.Name.Equals(sourceProp.Name) && c.CanWrite);
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
}
