using CodeNet.Mapper.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeNet.Mapper.Configurations;

public class MapperConfigurationBuilder
{
    internal Dictionary<MapType, MapperItemProperties[]> MapperItems { get; } = [];
    internal Dictionary<Type, Func<int, Array>> ArrayConstructors { get; set; } = [];
    internal Dictionary<Type, Func<object>> ObjectConstructor { get; set; } = [];
    public Dictionary<Type, Dictionary<string, Func<object, object>>> SourceGetters { get; set; } = [];
    public Dictionary<Type, Dictionary<string, Action<object, object>>> DestinationSetters { get; set; } = [];
    internal int MaxDepth { get; private set; } = MapperConfigurationBuilderExtensions.DEFAULT_MAX_DEPTH;
    
    public MapperColumnBuilder<TSource, TDestination> CreateMap<TSource, TDestination>(Action<MapperColumnBuilder<TSource, TDestination>>? action, bool reverse = false)
    where TSource : new()
    where TDestination : new()
    {
        MapperColumnBuilder<TSource, TDestination> map = new();

        if (action is not null)
            action(map);

        SetColumnProperties(MapperColumnBuilder<TSource, TDestination>.MapType, map.Columns);
        SetGetterSetter<TSource, TDestination>();
        CreateInstance(typeof(TDestination));
        CreateArrayInstance(typeof(TDestination));

        if (reverse)
        {
            SetColumnProperties(MapperColumnBuilder<TDestination, TSource>.MapType, map.Columns.ToDictionary(c => c.Value, c => c.Key));
            SetGetterSetter<TDestination, TSource>();
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

    private void SetGetterSetter<TSource, TDestination>()
    {
        var sourceType = typeof(TSource);
        var destinationType = typeof(TDestination);

        SourceGetters[sourceType] = [];
        foreach (var prop in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.CanRead)
            {
                ParameterExpression instanceParam = Expression.Parameter(typeof(object), "sourceInstance");
                SourceGetters[sourceType][prop.Name] = Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Property(Expression.Convert(instanceParam, sourceType), prop), typeof(object)), instanceParam).Compile();
            }
        }

        DestinationSetters[destinationType] = [];
        foreach (var prop in destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.CanWrite)
            {
                ParameterExpression instanceParam = Expression.Parameter(typeof(object), "instance");
                ParameterExpression valueParam = Expression.Parameter(typeof(object), "value");
                DestinationSetters[destinationType][prop.Name] = Expression.Lambda<Action<object, object>>(Expression.Assign(Expression.Property(Expression.Convert(instanceParam, destinationType), prop), Expression.Convert(valueParam, prop.PropertyType)), instanceParam, valueParam).Compile();
            }
        }
    }

    private bool SetColumnProperties(MapType mapType, Dictionary<string, string> mapperColumns)
    {
        var sourceTypeProperties = mapType.SourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        List<MapperItemProperties> properties = [];
        foreach (var sourceProp in sourceTypeProperties)
        {
            if (!sourceProp.CanRead)
                continue;

            var destinationProp = mapType.DestinationType.GetProperty(mapperColumns?.TryGetValue(sourceProp.Name, out string? sourceColumn) is true ? sourceColumn : sourceProp.Name, BindingFlags.Public | BindingFlags.Instance);
            if (destinationProp?.CanWrite is not true)
                continue;

            properties.Add(new MapperItemProperties { SourceProp = sourceProp, DestinationProp = destinationProp });
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
