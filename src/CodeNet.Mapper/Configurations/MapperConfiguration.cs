namespace CodeNet.Mapper.Configurations;

internal class MapperConfiguration
{
    public Dictionary<MapType, MapperItem> MapperItems { get; set; } = [];
    public int MaxDepth { get; set; }
}
