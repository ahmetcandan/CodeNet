using System.Dynamic;

namespace CodeNet.Email.Builder;

internal static class ObjectExtension
{
    public static object? GetValue(this object obj, string property)
    {
        var type = obj.GetType();
        var properties = property.Split('.');
        object? value;
        if (type.Equals(typeof(ExpandoObject)))
            value = ((IDictionary<string, object>)obj)[properties[0]];
        else
            value = type.GetProperty(properties[0])?.GetValue(obj);

        if (value is null)
            return null;

        return properties.Length == 1
            ? value
            : value?.GetValue(string.Join('.', properties[1..]));
    }
}
