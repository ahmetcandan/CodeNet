using System.Dynamic;

namespace CodeNet.Messaging.Builder;

internal static class ObjectExtension
{
    public static object? GetValue(this object obj, string property)
    {
        var type = obj.GetType();
        var properties = property.Split('.');
        object? value = type.Equals(typeof(ExpandoObject))
            ? ((IDictionary<string, object>)obj)[properties[0]]
            : (type.GetProperty(properties[0])?.GetValue(obj));

        return value is null
            ? null
            : properties.Length == 1
            ? value
            : value?.GetValue(string.Join('.', properties[1..]));
    }
}
