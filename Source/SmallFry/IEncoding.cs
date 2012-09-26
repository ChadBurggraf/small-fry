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
        /// Gets a Content-Encoding value to send in responses when the given <see cref="EncodingType"/>
        /// was used to choose this encoding.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> used to choose this encoding.</param>
        /// <returns>A Content-Encoding value to send.</returns>
        string ContentEncoding(EncodingType encodingType);

        /// <summary>
        /// Creates a decoding stream around the given stream and returns
        /// the wrapped stream to use for decoding.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to decode.</param>
        /// <param name="stream">The stream to apply the decoding to.</param>
        /// <returns>A stream providing decoding services to the original stream.</returns>
        Stream Decode(EncodingType encodingType, Stream stream);

        /// <summary>
        /// Creates an encoding stream around the given stream and returns
        /// the wrapped stream to use for encoding.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to encode.</param>
        /// <param name="stream">The stream to apply the encoding to.</param>
        /// <returns>A stream providing encoding services to the original stream.</returns>
        Stream Encode(EncodingType encodingType, Stream stream);
    }
}