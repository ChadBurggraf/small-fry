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
    using System.IO;
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
        /// Gets the header value identified by the given name
        /// form this instance's <see cref="Headers"/> collection. Returns the specified
        /// type's default value if parsing fails for any reason. For more control over
        /// parsing behavior, use <see cref="NameValueCollection"/> extensions such as
        /// <see cref="Extensions.Get{T}(NameValueCollection,string,name)"/> instead.
        /// </summary>
        /// <typeparam name="T">The type of the header value to get.</typeparam>
        /// <param name="name">The name of the header value to get.</param>
        /// <returns>The header value for the specified name.</returns>
        T HeaderValue<T>(string name);

        /// <summary>
        /// Returns the physical path that corresponds to the given virtual path
        /// relative to the current service host.
        /// </summary>
        /// <param name="path">The virtual path to get the physical path for.</param>
        /// <returns>The physical path that corresponds to the given virtual path.</returns>
        string MapPath(string path);

        /// <summary>
        /// Gets the query string value identified by the given name
        /// from this instance's <see cref="RequestUri"/>. Returns the specified type's default value
        /// if parsing fails for any reason. For more control over parsing behavior,
        /// use <see cref="Uri"/> extensions such as <see cref="Extensions.GetQueryValue{T}(Uri,string,bool)"/>
        /// instead.
        /// </summary>
        /// <typeparam name="T">The type of the query value to get.</typeparam>
        /// <param name="name">The name of the query value to get.</param>
        /// <returns>The query value for the specified name.</returns>
        T QueryValue<T>(string name);

        /// <summary>
        /// Gets the route value parsed when matching the route of the specified name.
        /// To ensure that the value is property parsed, add type constraints to the route.
        /// </summary>
        /// <typeparam name="T">The type of the route value to get.</typeparam>
        /// <param name="name">The name of the route value to get.</param>
        /// <returns>The route value for the specified name.</returns>
        T RouteValue<T>(string name);

        /// <summary>
        /// Sets an encoding filter on the request's input to provide
        /// decoding services.
        /// </summary>
        /// <param name="encodingFilter">The encoding filter to set.</param>
        void SetEncodingFilter(Stream encodingFilter);
    }
}