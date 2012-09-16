//-----------------------------------------------------------------------------
// <copyright file="IRequestMessage.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    /// <summary>
    /// Defines the interface for request messages received by a service endpoint.
    /// </summary>
    public interface IRequestMessage : IDisposable
    {
        /// <summary>
        /// Gets the collection of cookies received in the request.
        /// </summary>
        NameValueCollection Cookies { get; }

        /// <summary>
        /// Gets the collection of headers received in the request.
        /// </summary>
        NameValueCollection Headers { get; }

        /// <summary>
        /// Gets a dictionary of custom properties added to the request during
        /// processing.
        /// </summary>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// Gets the original request URI.
        /// </summary>
        Uri RequestUri { get; }

        /// <summary>
        /// Tries to parse a typed route parameter into the provided object.
        /// </summary>
        /// <typeparam name="T">The type to try to parse the route parameter into.</typeparam>
        /// <param name="name">The name of the route parameter to try to parse.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if parsing was successful, otherwise false.</returns>
        bool TryParseRouteParameter<T>(string name, out T result) where T : struct;
    }
}