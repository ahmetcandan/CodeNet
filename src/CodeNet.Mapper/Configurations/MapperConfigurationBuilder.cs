using CodeNet.Mapper.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeNet.Mapper.Configurations;

public class MapperConfigurationBuilder
{
    internal readonly Dictionary<MapType, MapperItemProperties[]> _mapperItems = [];
    internal readonly Dictionary<Type, Func<int, Array>> _arrayConstructors = [];
    internal readonly Dictionary<Type, Func<object>> _listConstructors = [];
    internal readonly Dictionary<Type, Func<object>> _objectConstructor = [];
    internal int MaxDepth { get; private set; } = MapperConfigurationBuilderExtensions._default_max_depth;

    public MapperColumnBuilder<TSource, TDestination> CreateMap<TSource, TDestination>(Action<MapperColumnBuilder<TSource, TDestination>>? action, bool reverse = false)
    where TSource : new()
    where TDestination : new()
    {
        MapperColumnBuilder<TSource, TDestination> map = new();

        if (action is not null)
            action(map);

        SetColumnProperties(MapperColumnBuilder<TSource, TDestination>.MapType, map.Columns);
        CreateInstance(typeof(TDestination));
        CreateArrayInstance(typeof(TDestination));
        CreateListInstance(typeof(TDestination));

        if (reverse)
        {
            SetColumnProperties(MapperColumnBuilder<TDestination, TSource>.MapType, map.Columns.ToDictionary(c => c.Value, c => c.Key));
            CreateInstance(typeof(TSource));
            CreateArrayInstance(typeof(TSource));
            CreateListInstance(typeof(TSource));
        }

        return map;
    }

    public MapperColumnBuilder<TSource, TDestination> CreateMap<TSource, TDestination>(Action<MapperColumnBuilder<TSource, TDestination>>? action = null)
        where TSource : new()
        where TDestination : new() => CreateMap(action, false);

    private void SetColumnProperties(MapType mapType, Dictionary<string, string> mapperColumns)
    {
        List<MapperItemProperties> properties = [];
        foreach (var sourceProp in mapType.SourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!sourceProp.CanRead)
                continue;

            ParameterExpression instanceSourceParam = Expression.Parameter(typeof(object), "sourceInstance");

            var destinationProp = mapType.DestinationType.GetProperty(mapperColumns?.TryGetValue(sourceProp.Name, out string? sourceColumn) is true ? sourceColumn : sourceProp.Name, BindingFlags.Public | BindingFlags.Instance);
            if (destinationProp?.CanWrite is not true)
                continue;

            ParameterExpression instanceDestinationParam = Expression.Parameter(typeof(object), "instance");
            ParameterExpression valueParam = Expression.Parameter(typeof(object), "value");

            Type? sourceElementType = sourceProp.PropertyType.HasElementType ? sourceProp.PropertyType.GetElementType() : sourceProp.PropertyType.GetGenericArguments().FirstOrDefault();
            Type? destinationElementType = destinationProp.PropertyType.HasElementType ? destinationProp.PropertyType.GetElementType() : destinationProp.PropertyType.GetGenericArguments().FirstOrDefault();
            properties.Add(new MapperItemProperties
            {
                SourceGetter = Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Property(Expression.Convert(instanceSourceParam, mapType.SourceType), sourceProp), typeof(object)), instanceSourceParam).Compile(),
                DestinationSetter = Expression.Lambda<Action<object, object?>>(Expression.Assign(Expression.Property(Expression.Convert(instanceDestinationParam, mapType.DestinationType), destinationProp), Expression.Convert(valueParam, destinationProp.PropertyType)), instanceDestinationParam, valueParam).Compile(),
                SourceType = sourceProp.PropertyType,
                DestinationType = destinationProp.PropertyType,
                DestinationTypeIsEnum = destinationProp.PropertyType.IsEnum,
                DestinationElementTypeIsEnum = destinationProp.PropertyType.GetElementType()?.IsEnum ?? false,
                SourceTypeHasElementType = sourceElementType is not null && !IsSimpleType(sourceElementType),
                DestinationTypeHasElementType = destinationElementType is not null && !IsSimpleType(sourceElementType),
                SourceElementType = sourceElementType,
                DestinationElementType = destinationElementType,
                SourceTypeIsClass = sourceProp.PropertyType.IsClass,
                IsAssignableFrom = destinationProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType),
                ElementTypeIsAssignableFrom = sourceElementType?.IsAssignableFrom(destinationElementType) ?? false
            });
        }

        _mapperItems.TryAdd(mapType, [.. properties]);
    }

    private static bool IsSimpleType(Type? type) => type is not null && (type.IsPrimitive || type.IsValueType || type == typeof(string));

    public void SetMaxDepth(int maxDepth) => MaxDepth = maxDepth;

    private void CreateInstance(Type type)
    {
        if (!_objectConstructor.ContainsKey(type))
            _objectConstructor[type] = Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(type), typeof(object))).Compile();
    }

    private void CreateArrayInstance(Type type)
    {
        if (!_arrayConstructors.ContainsKey(type))
        {
            ParameterExpression sizeParam = Expression.Parameter(typeof(int), "size");
            _arrayConstructors[type] = Expression.Lambda<Func<int, Array>>(Expression.NewArrayBounds(type, sizeParam), sizeParam).Compile();
        }
    }

    private void CreateListInstance(Type type)
    {
        if (!_listConstructors.ContainsKey(type))
            _listConstructors[type] = Expression.Lambda<Func<object>>(Expression.New(typeof(List<>).MakeGenericType(type))).Compile();
    }
}
