namespace CodeNet.Email.Builder;

internal static class ObjectExtension
{
    public static object? GetValue(this object obj, string property)
    {
        var properties = property.Split('.');
        var value = obj.GetType().GetProperty(properties[0])?.GetValue(obj);
        if (value is null)
            return null;

        return properties.Length == 1
            ? value
            : value?.GetValue(string.Join('.', properties[1..]));
    }
}
