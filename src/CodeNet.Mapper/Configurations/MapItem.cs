namespace CodeNet.Mapper.Configurations;

internal class MapperItem
{
    public bool AddColumn(string sourceColumnName, string destinationColumnName)
    {
        return Columns.TryAdd(sourceColumnName, destinationColumnName) & RevertColumns.TryAdd(destinationColumnName, sourceColumnName);
    }

    public Dictionary<string, string> Columns { get; set; } = [];
    public Dictionary<string, string> RevertColumns { get; set; } = [];
    public int? MaxDepth { get; set; }
}
