namespace CodeNet.Mapper.Configurations;

internal class MapperConfiguration
{
    public Dictionary<MapType, MapperItemProperties[]> MapperItems { get; set; } = [];
    public int MaxDepth { get; set; }
}
