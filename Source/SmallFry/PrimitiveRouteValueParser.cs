//-----------------------------------------------------------------------------
// <copyright file="PrimitiveRouteValueParser.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Implements <see cref="IRouteValueParser"/> to parse primitive
    /// .NET value types.
    /// </summary>
    public sealed class PrimitiveRouteValueParser : IRouteValueParser
    {
        /// <summary>
        /// Gets a value indicating whether this instance can parse a parameter
        /// of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the route parameter to check.</typeparam>
        /// <returns>True if this instance can parse the given type, false otherwise.</returns>
        public bool CanParse<T>()
        {
            return TypeCode.Object != Type.GetTypeCode(typeof(T));
        }

        /// <summary>
        /// Tries to parse a route parameter with the given name and value.
        /// </summary>
        /// <typeparam name="T">The type of the route parameter to attempt to parse.</typeparam>
        /// <param name="name">The name of the route parameter to attempt to parse.</param>
        /// <param name="value">The value of the route parameter to attempt to parse.</param>
        /// <param name="result">The result of the parse attempt. Should be default(T) if the parse attempt failed.</param>
        /// <returns>True if the parse attempt was successful, false otherwise.</returns>
        public bool TryParse<T>(string name, string value, out T result)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "name must contain a value.");
            }

            result = default(T);
            value = (value ?? string.Empty).Trim();
            bool success = true;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    object obj = null;

                    switch (Type.GetTypeCode(typeof(T)))
                    {
                        case TypeCode.Boolean:
                            obj = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Byte:
                            obj = Convert.ToByte(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Char:
                            obj = Convert.ToChar(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.DateTime:
                            obj = DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                            break;
                        case TypeCode.DBNull:
                        case TypeCode.Empty:
                            break;
                        case TypeCode.Decimal:
                            obj = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Double:
                            obj = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Int16:
                            obj = Convert.ToInt16(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Int32:
                            obj = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Int64:
                            obj = Convert.ToInt64(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.SByte:
                            obj = Convert.ToSByte(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Single:
                            obj = Convert.ToSingle(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.String:
                            obj = Convert.ToString(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.UInt16:
                            obj = Convert.ToUInt16(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.UInt32:
                            obj = Convert.ToUInt32(value, CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.UInt64:
                            obj = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
                            break;
                        default:
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Cannot parse values of type {0} with this instance.", typeof(T)), "T");
                    }

                    if (obj != null)
                    {
                        result = (T)obj;
                    }
                }
                catch (FormatException)
                {
                    success = false;
                }
                catch (OverflowException)
                {
                    success = false;
                }
            }

            return success;
        }
    }
}