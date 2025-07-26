namespace CodeNet.Mapper.Configurations;

internal class MapperConfiguration
{
    public Dictionary<MapType, MapperItemProperties[]> MapperItems { get; set; } = [];
    public int MaxDepth { get; set; }

    public Dictionary<Type, Dictionary<string, Func<object, object>>> SourceGetters { get; set; } = [];
    public Dictionary<Type, Dictionary<string, Action<object, object?>>> DestinationSetters { get; set; } = [];

    public Dictionary<Type, Func<int, Array>> ArrayConstructors { get; set; } = [];
    public Dictionary<Type, Func<object>> ObjectConstructor { get; set; } = [];
}
