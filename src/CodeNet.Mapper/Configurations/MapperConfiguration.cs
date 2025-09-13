namespace CodeNet.Mapper.Configurations;

internal class MapperConfiguration(Dictionary<MapType, MapperItemProperties[]> mapperItems, Dictionary<Type, Func<object>> objectConstructors, Dictionary<Type, Func<int, Array>> arrayConstructors, Dictionary<Type, Func<object>> listConstructors, int maxDepth)
{
    public Dictionary<MapType, MapperItemProperties[]> MapperItems { get; } = mapperItems;
    public int MaxDepth { get; } = maxDepth;
    public Dictionary<Type, Func<int, Array>> ArrayConstructors { get; } = arrayConstructors;
    public Dictionary<Type, Func<object>> ListConstructors { get; } = listConstructors;
    public Dictionary<Type, Func<object>> ObjectConstructors { get; } = objectConstructors;
}
