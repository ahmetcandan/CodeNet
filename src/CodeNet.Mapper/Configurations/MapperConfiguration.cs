namespace CodeNet.Mapper.Configurations;

internal class MapperConfiguration(Dictionary<MapType, MapperItemProperties[]> mapperItems, Dictionary<Type, Func<object>> objectConstructors, Dictionary<Type, Func<int, Array>> arrayConstructors, int maxDepth)
{
    private readonly Dictionary<MapType, MapperItemProperties[]> _mapperItems = mapperItems;
    public Dictionary<MapType, MapperItemProperties[]> MapperItems
    {
        get => _mapperItems;
    }

    private readonly int _maxDepth = maxDepth;
    public int MaxDepth
    {
        get => _maxDepth;
    }

    private readonly Dictionary<Type, Func<int, Array>> _arrayConstructors = arrayConstructors;
    public Dictionary<Type, Func<int, Array>> ArrayConstructors
    {
        get => _arrayConstructors;
    }

    private readonly Dictionary<Type, Func<object>> _objectConstructors = objectConstructors;
    public Dictionary<Type, Func<object>> ObjectConstructors
    {
        get => _objectConstructors;
    }
}
