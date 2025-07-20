using System.Reflection;

namespace CodeNet.Mapper.Configurations;

internal class MapperItemProperties
{
    public required PropertyInfo SourceProp { get; set; }
    public required PropertyInfo DestinationProp { get; set; }
}
