namespace CodeNet.Mapper.Configurations;

internal class MapperConfiguration(Dictionary<MapType, MapperItemProperties[]> mapperItems, Dictionary<Type, Func<object>> objectConstructors, Dictionary<Type, Func<int, Array>> arrayConstructors, Dictionary<Type, Func<object>> listConstructors, int maxDepth)
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

    private readonly Dictionary<Type, Func<object>> _listConstructors = listConstructors;
    public Dictionary<Type, Func<object>> ListConstructors
    {
        get => _listConstructors;
    }

    private readonly Dictionary<Type, Func<object>> _objectConstructors = objectConstructors;
    public Dictionary<Type, Func<object>> ObjectConstructors
    {
        get => _objectConstructors;
    }
}
