//-----------------------------------------------------------------------------
// <copyright file="PrimitiveRouteParameterParser.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements <see cref="IRouteParameterParser"/> to parse primitive
    /// .NET value types.
    /// </summary>
    public sealed class PrimitiveRouteParameterParser : IRouteParameterParser
    {
        /// <summary>
        /// Gets a collection of types this parser can parse.
        /// </summary>
        public IEnumerable<Type> CanParseTypes
        {
            get { return Extensions.PrimitiveTypes; }
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
            return value.ConvertTo(type);
        }
    }
}