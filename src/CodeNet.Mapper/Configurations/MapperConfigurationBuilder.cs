using CodeNet.Mapper.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeNet.Mapper.Configurations;

public class MapperConfigurationBuilder
{
    internal Dictionary<MapType, MapperItemProperties[]> MapperItems { get; } = [];
    internal Dictionary<Type, Func<int, Array>> ArrayConstructors { get; set; } = [];
    internal Dictionary<Type, Func<object>> ObjectConstructor { get; set; } = [];
    internal int MaxDepth { get; private set; } = MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH;
    
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

        if (reverse)
        {
            SetColumnProperties(MapperColumnBuilder<TDestination, TSource>.MapType, map.Columns.ToDictionary(c => c.Value, c => c.Key));
            CreateInstance(typeof(TSource));
            CreateArrayInstance(typeof(TSource));
        }

        return map;
    }

        public MapperColumnBuilder<TSource, TDestination> CreateMap<TSource, TDestination>(Action<MapperColumnBuilder<TSource, TDestination>>? action = null)
        where TSource : new()
        where TDestination : new()
    {
        return CreateMap(action, false);
    }

    private bool SetColumnProperties(MapType mapType, Dictionary<string, string> mapperColumns)
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

            properties.Add(new MapperItemProperties
            {
                SourceProp = sourceProp,
                DestinationProp = destinationProp,
                SourceGetter = Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Property(Expression.Convert(instanceSourceParam, mapType.SourceType), sourceProp), typeof(object)), instanceSourceParam).Compile(),
                DestinationSetter = Expression.Lambda<Action<object, object?>>(Expression.Assign(Expression.Property(Expression.Convert(instanceDestinationParam, mapType.DestinationType), destinationProp), Expression.Convert(valueParam, destinationProp.PropertyType)), instanceDestinationParam, valueParam).Compile(),
                SourceType = sourceProp.PropertyType,
                DestinationType = destinationProp.PropertyType,
                DestinationTypeIsEnum = destinationProp.PropertyType.IsEnum,
                DestinationTypeIsArray = destinationProp.PropertyType.IsArray,
                DestinationTypeHasElementType = destinationProp.PropertyType.HasElementType,
                SourceTypeHasElementType = sourceProp.PropertyType.HasElementType,
                SourceElementType = sourceProp.PropertyType.HasElementType ? sourceProp.PropertyType.GetElementType() : null,
                DestinationElementType = destinationProp.PropertyType.HasElementType ? destinationProp.PropertyType.GetElementType() : null,
                SourceTypeIsClass = sourceProp.PropertyType.IsClass,
                DestinationTypeIsAssignableFrom = sourceType => destinationProp.PropertyType.IsAssignableFrom(sourceType)
            });
        }

        return MapperItems.TryAdd(mapType, [.. properties]);
    }

    public void SetMaxDepth(int maxDepth) => MaxDepth = maxDepth;

    private void CreateInstance(Type type)
    {
        if (!ObjectConstructor.ContainsKey(type))
            ObjectConstructor[type] = Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(type), typeof(object))).Compile();
    }

    private void CreateArrayInstance(Type type)
    {
        if (!ArrayConstructors.ContainsKey(type))
        {
            ParameterExpression sizeParam = Expression.Parameter(typeof(int), "size");
            ArrayConstructors[type] = Expression.Lambda<Func<int, Array>>(Expression.NewArrayBounds(type, sizeParam), sizeParam).Compile();
        }
    }
}
