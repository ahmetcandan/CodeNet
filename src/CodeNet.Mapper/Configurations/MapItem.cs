namespace CodeNet.Mapper.Configurations;

internal class MapperItem(Type sourceType, Type destinationType)
{
    public bool AddColumn(string sourceColumnName, string destinationColumnName)
    {
        return Columns.TryAdd(sourceColumnName, destinationColumnName) & RevertColumns.TryAdd(destinationColumnName, sourceColumnName);
    }

    public Type SourceType { get; set; } = sourceType;
    public Type DestinationType { get; set; } = destinationType;
    public Dictionary<string, string> Columns { get; set; } = [];
    public Dictionary<string, string> RevertColumns { get; set; } = [];
    public int? MaxDepth { get; set; }
}
