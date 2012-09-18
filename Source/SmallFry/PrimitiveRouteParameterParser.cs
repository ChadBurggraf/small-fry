//-----------------------------------------------------------------------------
// <copyright file="PrimitiveRouteParameterParser.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Implements <see cref="IRouteParameterParser"/> to parse primitive
    /// .NET value types.
    /// </summary>
    public sealed class PrimitiveRouteParameterParser : IRouteParameterParser
    {
        private static readonly Type[] PrimitiveTypes = new Type[]
        {
            typeof(bool),
            typeof(byte),
            typeof(char),
            typeof(DateTime),
            typeof(DBNull),
            typeof(decimal),
            typeof(double),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(float),
            typeof(string),
            typeof(ushort),
            typeof(uint),
            typeof(ulong)
        };

        /// <summary>
        /// Gets a collection of types this parser can parse.
        /// </summary>
        public IEnumerable<Type> CanParseTypes
        {
            get { return PrimitiveRouteParameterParser.PrimitiveTypes; }
        }

        /// <summary>
        /// Parses a route parameter with the given name and value
        /// of the specified type.
        /// </summary>
        /// <param name="type">The type to parse the parameter value into.</param>
        /// <param name="name">The name of the parameter to parse.</param>
        /// <param name="value">The value of the parameter to parse.</param>
        /// <returns>The parsed parameter value.</returns>
        public object Parse(Type type, string name, string value)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type cannot be null.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "name must contain a value.");
            }

            object result = type.IsValueType ? Activator.CreateInstance(type) : null;
            value = (value ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(value))
            {
                switch (Type.GetTypeCode(type))
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
                        result = DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
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
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Cannot parse values of type {0} with this instance.", type), "type");
                }
            }

            return result;
        }
    }
}