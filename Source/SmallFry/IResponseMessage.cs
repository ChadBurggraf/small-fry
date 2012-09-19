//-----------------------------------------------------------------------------
// <copyright file="IResponseMessage.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Specialized;
    using System.Web;

    /// <summary>
    /// Defines the interface for response messages sent by a service endpoint.
    /// </summary>
    public interface IResponseMessage
    {
        /// <summary>
        /// Gets the collection of cookies to send with the response.
        /// </summary>
        HttpCookieCollection Cookies { get; }

        /// <summary>
        /// Gets the collection of headers to send with the response.
        /// </summary>
        NameValueCollection Headers { get; }

        /// <summary>
        /// Gets or sets the object to serialize to the content body of the response.
        /// </summary>
        object Response { get; set; }

        /// <summary>
        /// Gets or sets the status code to send with the response.
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status description to send with the response.
        /// </summary>
        string StatusDescription { get; set; }
    }
}