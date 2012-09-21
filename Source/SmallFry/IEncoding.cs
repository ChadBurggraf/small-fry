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
        /// Gets a value indicating whether this instance can decode the given <see cref="EncodingType"/>.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to decode.</param>
        /// <returns>True if this instance can decode the <see cref="EncodingType"/>, false otherwise.</returns>
        bool CanDecode(EncodingType encodingType);

        /// <summary>
        /// Gets a value indicating whether this instance can encode the given <see cref="EncodingType"/>.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to encode.</param>
        /// <returns>True if this instance can encode the <see cref="EncodingType"/>, false otherwise.</returns>
        bool CanEncode(EncodingType encodingType);

        /// <summary>
        /// Decodes an input stream and writes the decoded content to the
        /// given output stream.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to decode.</param>
        /// <param name="inputStream">The stream to read encoded content from.</param>
        /// <param name="outputStream">The stream to write decoded content to.</param>
        void Decode(EncodingType encodingType, Stream inputStream, Stream outputStream);

        /// <summary>
        /// Encodes an input stream and writes the encoded content to the
        /// given output stream.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to encode</param>
        /// <param name="inputStream">The input stream to read content from.</param>
        /// <param name="outputStream">The output stream to write encoded content to.</param>
        void Encode(EncodingType encodingType, Stream inputStream, Stream outputStream);
    }
}