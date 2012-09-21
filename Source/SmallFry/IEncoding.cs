//-----------------------------------------------------------------------------
// <copyright file="IEncoding.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Defines the interface for handling incoming and outgoing content
    /// encodings.
    /// </summary>
    public interface IEncoding : IEquatable<IEncoding>
    {
        /// <summary>
        /// Gets a collection of content-encoding values this instance can decode.
        /// </summary>
        IEnumerable<string> AcceptableEncodings { get; }

        /// <summary>
        /// Gets a content-encoding from the given collection of acceptable encodings
        /// this instance can encode. If none of the given encodings can be encoded by
        /// this instance, returns null.
        /// </summary>
        /// <param name="acceptEncodings">A collection of acceptable encoding values.</param>
        /// <returns>A content encoding value, or none if none of the acceble encodings can be encoded.</returns>
        string ContentEncoding(IEnumerable<string> acceptEncodings);

        /// <summary>
        /// Decodes an input stream and writes the decoded content to the
        /// given output stream.
        /// </summary>
        /// <param name="acceptEncodings">The collection of acceptable encoding values used
        /// to choose this encoding.</param>
        /// <param name="inputStream">The stream to read encoded content from.</param>
        /// <param name="outputStream">The stream to write decoded content to.</param>
        void Decode(IEnumerable<string> acceptEncodings, Stream inputStream, Stream outputStream);

        /// <summary>
        /// Encodes an input stream and writes the encoded content to the
        /// given output stream.
        /// </summary>
        /// <param name="acceptEncodings">The collection of acceptable encoding values used
        /// to choose this encoding.</param>
        /// <param name="inputStream">The input stream to read content from.</param>
        /// <param name="outputStream">The output stream to write encoded content to.</param>
        void Encode(IEnumerable<string> acceptEncodings, Stream inputStream, Stream outputStream);
    }
}