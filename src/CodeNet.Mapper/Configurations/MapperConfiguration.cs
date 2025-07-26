namespace CodeNet.Mapper.Configurations;

internal class MapperConfiguration
{
    public Dictionary<MapType, MapperItemProperties[]> MapperItems { get; set; } = [];
    public int MaxDepth { get; set; }

    public Dictionary<Type, Func<int, Array>> ArrayConstructors { get; set; } = [];
    public Dictionary<Type, Func<object>> ObjectConstructors { get; set; } = [];
}
