namespace CodeNet.Mapper.Configurations;

internal struct MapType(Type sourceType, Type destinationType)
{
    public Type SourceType { get; set; } = sourceType;
    public Type DestinationType { get; set; } = destinationType;

    public static MapType Create(Type sourceType, Type destinationType) => new(sourceType, destinationType);
}
