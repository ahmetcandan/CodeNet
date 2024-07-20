using CodeNet.MongoDB;
using CodeNet.Parameters.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CodeNet.Parameters.MongoDB.Models;

[CollectionName("Parameters")]
public class ParameterDto : ParameterGroup
{
    [BsonId]
    public override string Code { get; set; } = string.Empty;

    public IEnumerable<ParameterModel> Parameters { get; set; } = [];
}
