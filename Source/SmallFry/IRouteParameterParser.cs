//-----------------------------------------------------------------------------
// <copyright file="IRouteParameterParser.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the interface for route parameter parsers, which parse
    /// named route values into typed .NET values.
    /// </summary>
    public interface IRouteParameterParser
    {
        /// <summary>
        /// Gets a collection of types this parser can parse.
        /// </summary>
        IEnumerable<Type> CanParseTypes { get; }

        /// <summary>
        /// Parses a route parameter with the given name and value
        /// of the specified type.
        /// </summary>
        /// <param name="type">The type to parse the parameter value into.</param>
        /// <param name="name">The name of the parameter to parse.</param>
        /// <param name="value">The value of the parameter to parse.</param>
        /// <returns>The parsed parameter value.</returns>
        object Parse(Type type, string name, string value);
    }
}