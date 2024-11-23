namespace CodeNet.StackExchange.Redis.Services;

public interface ISerializer
{
    string Serialize(object value);
    object? Deserialize(string value);
}