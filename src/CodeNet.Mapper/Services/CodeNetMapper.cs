using CodeNet.Mapper.Configurations;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Security.AccessControl;

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
            var value = _config.SourceGetters[sourceType][columns[i].SourceProp.Name](source);
            var sourceGetDelegate = _config.SourceGetters[sourceType][columns[i].SourceProp.Name];
            var getSourceValueCall = Expression.Call(Expression.Constant(sourceGetDelegate.Target), sourceGetDelegate.Method, Expression.Parameter(sourceType, "source"));
            var valueX = Expression.Variable(typeof(object), "valueX");

            if (value is null)
                continue;
            
            if (columns[i].DestinationProp.PropertyType == columns[i].SourceProp.PropertyType
                    || columns[i].DestinationProp.PropertyType.IsAssignableFrom(columns[i].SourceProp.PropertyType)
                    || columns[i].DestinationProp.PropertyType.IsEnum)
                _config.DestinationSetters[destinationType][columns[i].DestinationProp.Name](destination, value);
            else if (value is Array sourceArray)
            {
                if (!columns[i].DestinationProp.PropertyType.HasElementType || !sourceArray.GetType().HasElementType)
                    continue;

                var itemType = columns[i].DestinationProp.PropertyType.GetElementType()!;
                var destinationList = _config.ArrayConstructors[itemType](sourceArray.Length);

                for (int j = 0; j < sourceArray.Length; j++)
                    destinationList.SetValue(GetArrayItem(sourceArray.GetType().GetElementType()!, itemType, sourceArray.GetValue(j), depth), j);

                _config.DestinationSetters[destinationType][columns[i].DestinationProp.Name](destination, destinationList);
            }
            else if (columns[i].SourceProp.PropertyType.IsClass)
                _config.DestinationSetters[destinationType][columns[i].DestinationProp.Name](destination, MapTo(columns[i].SourceProp.PropertyType, columns[i].DestinationProp.PropertyType, value, _config.ObjectConstructor[columns[i].DestinationProp.PropertyType].Invoke(), depth + 1));
        }

        return destination;
    }

    private object? GetArrayItem(Type sourceType, Type targetType, object? item, int depth) => item is null || targetType.IsAssignableFrom(sourceType) || targetType.IsEnum ? item : MapTo(item.GetType(), targetType, item, _config.ObjectConstructor[targetType].Invoke(), depth + 1);
}
