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
    using System.Web;

    /// <summary>
    /// Defines the interface for request messages received by a service endpoint.
    /// </summary>
    public interface IRequestMessage : IDisposable
    {
        /// <summary>
        /// Gets the collection of cookies received in the request.
        /// </summary>
        HttpCookieCollection Cookies { get; }

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
        /// Gets the name of the service hosting the request.
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// Gets the route value parsed when matching the route of the specified name.
        /// To ensure that the value is property parsed, add type constraints to the route.
        /// </summary>
        /// <typeparam name="T">The type of the route value to get.</typeparam>
        /// <param name="name">The name of the route value to get.</param>
        /// <returns>The route value for the specified name.</returns>
        T RouteValue<T>(string name);
    }
}