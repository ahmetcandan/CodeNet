using System.ComponentModel;
using System.Reflection;

namespace CodeNet.Core.Convert;

public static class Converter
{
    public static T Convert<T>(this object value, T defaultValue)
    {
        var result = defaultValue;
        try
        {
            if (value is null || value == DBNull.Value) return result;
            result = typeof(T).IsEnum
                ? (T)Enum.ToObject(typeof(T), value.Convert(System.Convert.ToInt32(defaultValue)))
                : (T)System.Convert.ChangeType(value, typeof(T));
        }
        catch
        {

        }

        return result;
    }

    public static T? Convert<T>(this object value)
    {
        return value.Convert(default(T));
    }

    public static bool TryParse<T>(string source, out T? value)
    {

        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

        try
        {

            value = (T?)converter.ConvertFromString(source);
            return true;
        }
        catch
        {

            value = default;
            return false;
        }

    }

    public static bool TryChangeType<T>(object source, out T? value)
    {
        try
        {
            Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            value = source.Convert<T>();
            return true;

        }
        catch
        {
            value = default;
            return false;

        }
    }

    public static T Map<T>(this object value) where T : new()
    {
        T result = new();
        var valueProperties = value.GetType().GetProperties();
        var convertProperties = typeof(T).GetProperties();
        MethodInfo? convertMethod = null;
        foreach (var property in from c in convertProperties
                                 join v in valueProperties
                                 on 1 equals 1
                                 where PropertyNameEquals(c.Name, v.Name)
                                 select new { convertProperty = c, valueProperty = v })
        {
            var propValue = property.valueProperty.GetValue(value);
            if (property.valueProperty.PropertyType == property.convertProperty.PropertyType)
                property.convertProperty.SetValue(result, propValue);
            else
            {
                if (convertMethod == null)
                    convertMethod = typeof(Converter).GetMethod("Convert", [property.convertProperty.PropertyType]);
                var resultPropValue = convertMethod?.MakeGenericMethod(property.convertProperty.PropertyType).Invoke(null, [propValue]);
                property.convertProperty.SetValue(result, resultPropValue);
            }
        }

        return result;
    }

    public static IEnumerable<T> Map<T>(this IEnumerable<object> values) where T : new()
    {
        return values.Select(c => c.Map<T>());
    }

    private static bool PropertyNameEquals(string name1, string name2)
    {
        return name1.ToLower().Replace("_", "").Equals(name2.Replace("_", "").ToLower());
    }
}
