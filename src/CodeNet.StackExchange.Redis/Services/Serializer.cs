using Newtonsoft.Json;

namespace CodeNet.StackExchange.Redis.Services;

internal class Serializer : ISerializer
{
    public object? Deserialize(string value) => JsonConvert.DeserializeObject(value);

    public string Serialize(object value) => JsonConvert.SerializeObject(value);
}
