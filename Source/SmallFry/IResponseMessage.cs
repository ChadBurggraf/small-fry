//-----------------------------------------------------------------------------
// <copyright file="IResponseMessage.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;

    /// <summary>
    /// Defines the interface for response messages sent by a service endpoint.
    /// </summary>
    public interface IResponseMessage : IDisposable
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
        object ResponseObject { get; set; }

        /// <summary>
        /// Gets or sets the status code to send with the response.
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status description to send with the response.
        /// </summary>
        string StatusDescription { get; set; }

        /// <summary>
        /// Sets an encoding filter on the response's output to provide
        /// encoding services.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> the encoding is being set for.</param>
        /// <param name="encoding">The encoding to set.</param>
        void SetEncodingFilter(EncodingType encodingType, IEncoding encoding);

        /// <summary>
        /// Sets the status code and status description of this instance
        /// from the given <see cref="SmallFry.StatusCode"/> value.
        /// </summary>
        /// <param name="statusCode">The <see cref="SmallFry.StatusCode"/> to set.</param>
        void SetStatus(StatusCode statusCode);
    }
}