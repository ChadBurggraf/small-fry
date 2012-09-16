//-----------------------------------------------------------------------------
// <copyright file="IRequestMessage{T}.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    /// <summary>
    /// Defines the interface for request messages received by a service endpoint.
    /// </summary>
    /// <typeparam name="T">The type of the expected content received in the request.</typeparam>
    public interface IRequestMessage<T> : IRequestMessage
    {
        /// <summary>
        /// Gets the deserialized content of the request.
        /// </summary>
        T RequestObject { get; }
    }
}