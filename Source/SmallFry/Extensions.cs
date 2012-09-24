//-----------------------------------------------------------------------------
// <copyright file="InternalExtensions.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// Provides extension and convenience methods for working with services.
    /// </summary>
    public static class Extensions
    {
        internal static readonly Type[] PrimitiveTypes = new Type[]
        {
            typeof(bool),
            typeof(bool?),
            typeof(byte),
            typeof(byte?),
            typeof(char),
            typeof(char?),
            typeof(DateTime),
            typeof(DateTime?),
            typeof(DBNull),
            typeof(decimal),
            typeof(decimal?),
            typeof(double),
            typeof(double?),
            typeof(Guid),
            typeof(Guid?),
            typeof(short),
            typeof(short?),
            typeof(int),
            typeof(int?),
            typeof(long),
            typeof(long?),
            typeof(sbyte),
            typeof(sbyte?),
            typeof(float),
            typeof(float?),
            typeof(string),
            typeof(ushort),
            typeof(ushort?),
            typeof(uint),
            typeof(uint?),
            typeof(ulong),
            typeof(ulong?)
        };

        private const string HttpDateTimeFormat1036 = "dddd, dd-MMM-yy HH:mm:ss";
        private const string HttpDateTimeFormat1123 = "ddd, dd MMM yyyy HH:mm:ss";
        private const string HttpDateTimeFormatAscTime = "ddd MMM d HH:mm:ss yyyy";

        private static readonly string[] ParseFormats = new string[] 
        { 
            HttpDateTimeFormat1123, 
            HttpDateTimeFormat1036, 
            HttpDateTimeFormatAscTime 
        };

        /// <summary>
        /// Converts a string into a value of the specified type.
        /// In order to succeed, the destination type must be one of the primitive
        /// .NET types, an <see cref="Enum"/>, or a <see cref="Guid"/>.
        /// </summary>
        /// <typeparam name="T">The type to convert the string value into.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static T ConvertTo<T>(this string value)
        {
            return (T)value.ConvertTo(typeof(T));
        }

        /// <summary>
        /// Converts a string into a value of the specified type.
        /// In order to succeed, the destination type must be one of the primitive
        /// .NET types, an <see cref="Enum"/>, or a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="type">The type to convert the string value into.</param>
        /// <returns>The converted value.</returns>
        public static object ConvertTo(this string value, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type cannot be null.");
            }

            object result = type.Default();
            value = (value ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(value))
            {
                Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

                if (underlyingType.IsEnum)
                {
                    result = Enum.Parse(underlyingType, value);
                }
                else if (typeof(Guid).IsAssignableFrom(underlyingType))
                {
                    result = new Guid(value);
                }
                else
                {
                    switch (Type.GetTypeCode(underlyingType))
                    {
                        case TypeCode.Boolean:
                            result = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Byte:
                            result = Convert.ToByte(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Char:
                            result = Convert.ToChar(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.DateTime:
                            result = value.ParseHttpDateTime();
                            break;
                        case TypeCode.DBNull:
                        case TypeCode.Empty:
                            break;
                        case TypeCode.Decimal:
                            result = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Double:
                            result = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Int16:
                            result = Convert.ToInt16(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Int32:
                            result = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Int64:
                            result = Convert.ToInt64(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.SByte:
                            result = Convert.ToSByte(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Single:
                            result = Convert.ToSingle(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.String:
                            result = Convert.ToString(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.UInt16:
                            result = Convert.ToUInt16(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.UInt32:
                            result = Convert.ToUInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.UInt64:
                            result = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
                            break;
                        default:
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Cannot convert values of type {0}.", type), "type");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a typed value from a <see cref="NameValueCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="collection">The collection to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <param name="throwOnError">A value indicating whether to re-throw an exception encountered
        /// durint parsing. If false, the default value for the specified type will be returned
        /// in case of an error.</param>
        /// <returns>A typed value.</returns>
        public static T Get<T>(this NameValueCollection collection, string name, bool throwOnError = false)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection", "collection cannot be null.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "name must contain a value.");
            }

            T result = default(T);

            try
            {
                string value = collection[name];

                if (!string.IsNullOrEmpty(value))
                {
                    result = value.ConvertTo<T>();
                }
            }
            catch (FormatException)
            {
                if (throwOnError)
                {
                    throw;
                }
            }
            catch (OverflowException)
            {
                if (throwOnError)
                {
                    throw;
                }
            }
            catch (InvalidCastException)
            {
                if (throwOnError)
                {
                    throw;
                }
            }
            catch (ArgumentException)
            {
                if (throwOnError)
                {
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the value of the query string parameter with the given key.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> to get the query string value from.</param>
        /// <param name="key">The key to get the value of.</param>
        /// <returns>The query string value for the given key.</returns>
        public static string GetQueryValue(this Uri uri, string key)
        {
            return uri.QueryString()[key];
        }

        /// <summary>
        /// Gets the typed value of the query string parameter with the given key.
        /// </summary>
        /// <typeparam name="T">The type to conver the value into.</typeparam>
        /// <param name="uri">The <see cref="Uri"/> to get the query string value from.</param>
        /// <param name="key">The key to get the value of.</param>
        /// <returns>The query string value for the given key.</returns>
        public static T GetQueryValue<T>(this Uri uri, string key)
        {
            string value = uri.GetQueryValue(key);

            if (!string.IsNullOrEmpty(value))
            {
                return value.ConvertTo<T>();
            }

            return default(T);
        }

        /// <summary>
        /// Parses an HTTP-formatted date string into a <see cref="DateTime"/> instance.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <returns>The parsed <see cref="DateTime"/>.</returns>
        public static DateTime ParseHttpDateTime(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value", "value must contain a value.");
            }

            try
            {
                return DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }
            catch (FormatException)
            {
            }

            return DateTime.ParseExact(
                Regex.Replace(value.Trim(), @"\s+GMT$", string.Empty, RegexOptions.IgnoreCase),
                Extensions.ParseFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal);
        }

        /// <summary>
        /// Parses the query string of a <see cref="Uri"/> into a <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> to parse the query string from.</param>
        /// <returns>A query string represented as a <see cref="NameValueCollection"/>.</returns>
        public static NameValueCollection QueryString(this Uri uri)
        {
            NameValueCollection result = new NameValueCollection();
            string query = uri != null ? uri.Query : string.Empty;

            if (!string.IsNullOrEmpty(query))
            {
                if (query.StartsWith("?", StringComparison.Ordinal))
                {
                    query = query.Substring(1);
                }

                foreach (string part in query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] pair = part.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                    if (!string.IsNullOrEmpty(pair[0]))
                    {
                        string key = HttpUtility.UrlDecode(pair[0]);

                        if (pair.Length > 1)
                        {
                            foreach (string value in HttpUtility.UrlDecode(pair[1]).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                result.Add(key, value);
                            }
                        }
                        else
                        {
                            result.Add(key, string.Empty);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> into an HTTP-formatted date string.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> to convert.</param>
        /// <returns>An HTTP-formatted date string.</returns>
        public static string ToHttpString(this DateTime value)
        {
            return value.ToUniversalTime().ToString(Extensions.HttpDateTimeFormat1123, CultureInfo.InvariantCulture) + " GMT";
        }

        /// <summary>
        /// Tries to get a typed value from a <see cref="NameValueCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="collection">The collection to get the value from.</param>
        /// <param name="name">The name of the value to get.</param>
        /// <param name="result">A typed value.</param>
        /// <returns>True if the conversion succeeded, false otherwise..</returns>
        public static bool TryGet<T>(this NameValueCollection collection, string name, out T result)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection", "collection cannot be null.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "name must contain a value.");
            }

            result = default(T);
            bool success = false;

            try
            {
                string value = collection[name];

                if (!string.IsNullOrEmpty(value))
                {
                    result = value.ConvertTo<T>();
                }

                success = true;
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            catch (InvalidCastException)
            {
            }
            catch (ArgumentException)
            {
            }

            return success;
        }

        /// <summary>
        /// Tries to get the typed query string value with the given key.
        /// </summary>
        /// <typeparam name="T">The type to conver the value into.</typeparam>
        /// <param name="uri">The <see cref="Uri"/> to get the query string value from.</param>
        /// <param name="key">The key to get the value of.</param>
        /// <param name="result">The typed query string value for the given key.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public static bool TryGetQueryValue<T>(this Uri uri, string key, out T result)
        {
            bool success = false;
            result = default(T);

            try
            {
                result = uri.GetQueryValue<T>(key);
                success = true;
            }
            catch (InvalidCastException)
            {
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            catch (ArgumentException)
            {
            }

            return success;
        }

        /// <summary>
        /// Tries to parse an HTTP-formatted date string into a <see cref="DateTime"/> instance.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <param name="result">The parsed <see cref="DateTime"/>.</param>
        /// <returns>True if the parse was successful, false otherwise.</returns>
        public static bool TryParseHttpDateTime(this string value, out DateTime result)
        {
            bool success = false;
            result = DateTime.MinValue;

            try
            {
                result = value.ParseHttpDateTime();
            }
            catch (FormatException)
            {
            }
            catch (ArgumentException)
            {
            }

            return success;
        }
    }
}