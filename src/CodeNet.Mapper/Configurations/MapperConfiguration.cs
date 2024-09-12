namespace CodeNet.Mapper.Configurations;

internal class MapperConfiguration
{
    public IList<MapperItem> MapperItems { get; set; } = [];
    public int MaxDepth { get; set; }
}
