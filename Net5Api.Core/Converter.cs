using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Core
{
    public static class Converter
    {
        public static T Convert<T>(this object value, T defaultValue)
        {
            var result = defaultValue;
            try
            {
                if (value == null || value == DBNull.Value) return result;
                if (typeof(T).IsEnum)
                {
                    result = (T)Enum.ToObject(typeof(T), Convert(value, System.Convert.ToInt32(defaultValue)));
                }
                else
                {
                    result = (T)System.Convert.ChangeType(value, typeof(T));
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public static T Convert<T>(this object value)
        {
            return Convert(value, default(T));
        }

        public static Boolean TryParse<T>(String source, out T value)
        {

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            try
            {

                value = (T)converter.ConvertFromString(source);
                return true;

            }
            catch
            {

                value = default(T);
                return false;

            }

        }

        public static Boolean TryChangeType<T>(Object source, out T value)
        {

            try
            {

                Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                value = source.Convert<T>();
                return true;

            }
            catch
            {

                value = default(T);
                return false;

            }

        }

        public static T Map<T>(this object value) where T : new()
        {
            T result = new T();
            var valueProperties = value.GetType().GetProperties();
            var convertProperties = typeof(T).GetProperties();
            MethodInfo convertMethod = null;
            foreach (var property in (from c in convertProperties
                                      join v in valueProperties
                                      on 1 equals 1
                                      where propertyNameEquals(c.Name, v.Name)
                                      select new { convertProperty = c, valueProperty = v }))
            {
                var propValue = property.valueProperty.GetValue(value);
                if (property.valueProperty.PropertyType == property.convertProperty.PropertyType)
                    property.convertProperty.SetValue(result, propValue);
                else
                {
                    if (convertMethod == null)
                        typeof(Converter).GetMethod("Convert", new Type[] { typeof(object) });
                    MethodInfo convertMethodExecute = convertMethod.MakeGenericMethod(property.convertProperty.PropertyType);
                    var resultPropValue = convertMethodExecute.Invoke(null, new object[] { propValue });
                    property.convertProperty.SetValue(result, resultPropValue);
                }
            }

            return result;
        }

        private static bool propertyNameEquals(string name1, string name2)
        {
            return name1.ToLower().Replace("_", "").Equals(name2.Replace("_", "").ToLower());
        }
    }
}
