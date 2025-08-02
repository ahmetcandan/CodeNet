namespace CodeNet.Mapper.Configurations;

internal class MapperItemProperties
{
    public required Func<object, object> SourceGetter { get; set; }
    public required Action<object, object?> DestinationSetter { get; set; }
    public required Type SourceType { get; set; }
    public required Type DestinationType { get; set; }
    public Type? SourceElementType { get; set; }
    public Type? DestinationElementType { get; set; }
    public bool DestinationTypeIsEnum { get; set; }
    public bool SourceTypeHasElementType { get; set; }
    public bool DestinationTypeHasElementType { get; set; }
    public bool SourceTypeIsClass { get; set; }
    public bool IsAssignableFrom { get; set; }
    public bool ElementTypeIsAssignableFrom { get; set; }
    public bool DestinationElementTypeIsEnum { get; set; }
}
