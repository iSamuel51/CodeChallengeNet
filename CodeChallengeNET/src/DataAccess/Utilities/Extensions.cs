using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Utilities
{
    /// <summary>
    /// Utility Extension methods for value retrieval and/or conversion
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the value from the specified DataRecord column and convert it to the specified Type.
        /// </summary>
        /// <typeparam name="T">Type for conversion.</typeparam>
        /// <param name="row">The DataRecord row.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="defaultValue">The default Type value.</param>
        /// <returns></returns>
        public static T GetValueOrDefault<T>(this IDataRecord row, string fieldName, T defaultValue)
        {
            int ordinal = row.GetOrdinal(fieldName);
            return row.GetValueOrDefault<T>(ordinal, defaultValue);
        }

        /// <summary>
        /// Not inteded for direct use, use String version instead.
        /// </summary>
        public static T GetValueOrDefault<T>(this IDataRecord row, int ordinal, T defaultValue)
        {
            try
            {
                T result;
                if (row.IsDBNull(ordinal))
                {
                    result = defaultValue;
                }
                else
                {
                    result = (T)row.GetValue(ordinal).GetValue<T>(default(T));

                    if (result is string)
                    {
                        var trimResult = result.ToString().Trim();
                        result = (T)Convert.ChangeType(trimResult, typeof(T));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //possible format or type cast exceptions, we want default value
                string message = ex.Message;
                return defaultValue;
            }
        }

        /// <summary>
        /// Converts a raw object to the specified type.
        /// </summary>
        /// <typeparam name="T">Type for conversion.</typeparam>
        /// <param name="rawValue">The raw object value.</param>
        /// <param name="defaultValue">The default Type value.</param>
        /// <returns></returns>
        static public T GetValue<T>(this object rawValue, T defaultValue)
        {
            try
            {
                return rawValue == null ? default(T) :
                    (T)Convert.ChangeType(rawValue, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
