using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Core
{
    public static class Converter
    {
        public static T To<T>(this object value, T defaultValue)
        {
            var result = defaultValue;
            try
            {
                if (value == null || value == DBNull.Value) return result;
                if (typeof(T).IsEnum)
                {
                    result = (T)Enum.ToObject(typeof(T), To(value, Convert.ToInt32(defaultValue)));
                }
                else
                {
                    result = (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch (Exception ex)
            {
                //Tracer.Current.LogException(ex);
            }

            return result;
        }

        public static T To<T>(this object value)
        {
            return To(value, default(T));
        }
    }
}
