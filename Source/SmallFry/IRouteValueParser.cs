//-----------------------------------------------------------------------------
// <copyright file="IRouteValueParser.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    /// <summary>
    /// Defines the interface for route value parsers, parse
    /// named route values into typed .NET values.
    /// </summary>
    public interface IRouteValueParser
    {
        /// <summary>
        /// Gets a value indicating whether this instance can parse a parameter
        /// of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the route parameter to check.</typeparam>
        /// <returns>True if this instance can parse the given type, false otherwise.</returns>
        bool CanParse<T>();

        /// <summary>
        /// Tries to parse a route parameter with the given name and value.
        /// </summary>
        /// <typeparam name="T">The type of the route parameter to attempt to parse.</typeparam>
        /// <param name="name">The name of the route parameter to attempt to parse.</param>
        /// <param name="value">The value of the route parameter to attempt to parse.</param>
        /// <param name="result">The result of the parse attempt. Should be default(T) if the parse attempt failed.</param>
        /// <returns>True if the parse attempt was successful, false otherwise.</returns>
        bool TryParse<T>(string name, string value, out T result);
    }
}