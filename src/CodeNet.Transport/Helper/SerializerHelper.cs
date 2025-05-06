using System.Text;
using System.Text.Json;

namespace CodeNet.Transport.Helper;

internal class SerializerHelper
{
    public static T? DeserializeObject<T>(byte[] data)
    {
        return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(data));
    }
    public static byte[] SerializeObject(object obj)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
    }
}
