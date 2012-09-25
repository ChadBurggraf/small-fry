//-----------------------------------------------------------------------------
// <copyright file="GzipDeflateEncoding.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;

    /// <summary>
    /// Implements <see cref="IEncoding"/> to perform GZip and Deflate 
    /// encoding and decoding.
    /// </summary>
    public class GzipDeflateEncoding : IEncoding
    {
        private static readonly EncodingType Deflate = EncodingType.Parse("deflate");
        private static readonly EncodingType Gzip = EncodingType.Parse("gzip");

        /// <summary>
        /// Gets a value indicating whether this instance can decode the given <see cref="EncodingType"/>.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to decode.</param>
        /// <returns>True if this instance can decode the <see cref="EncodingType"/>, false otherwise.</returns>
        public bool CanDecode(EncodingType encodingType)
        {
            if (encodingType == null)
            {
                throw new ArgumentNullException("encodingType", "encodingType cannot be null.");
            }

            return GzipDeflateEncoding.Deflate.Accepts(encodingType) || GzipDeflateEncoding.Gzip.Accepts(encodingType);
        }

        /// <summary>
        /// Gets a value indicating whether this instance can encode the given <see cref="EncodingType"/>.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to encode.</param>
        /// <returns>True if this instance can encode the <see cref="EncodingType"/>, false otherwise.</returns>
        public bool CanEncode(EncodingType encodingType)
        {
            if (encodingType == null)
            {
                throw new ArgumentNullException("encodingType", "encodingType cannot be null.");
            }

            return encodingType.Accepts(GzipDeflateEncoding.Deflate) || encodingType.Accepts(GzipDeflateEncoding.Gzip);
        }

        /// <summary>
        /// Creates a decoding stream around the given stream and returns
        /// the wrapped stream to use for decoding.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to decode.</param>
        /// <param name="stream">The stream to apply the decoding to.</param>
        /// <returns>A stream providing decoding services to the original stream.</returns>
        public Stream Decode(EncodingType encodingType, Stream stream)
        {
            if (encodingType == null)
            {
                throw new ArgumentNullException("encodingType", "encodingType cannot be null.");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream", "stream cannot be null.");
            }

            return this.GetCompressionStream(encodingType, stream, CompressionMode.Decompress);
        }

        /// <summary>
        /// Creates an encoding stream around the given stream and returns
        /// the wrapped stream to use for encoding.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to encode.</param>
        /// <param name="stream">The stream to apply the encoding to.</param>
        /// <returns>A stream providing encoding services to the original stream.</returns>
        public Stream Encode(EncodingType encodingType, Stream stream)
        {
            if (encodingType == null)
            {
                throw new ArgumentNullException("encodingType", "encodingType cannot be null.");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream", "stream cannot be null.");
            }

            return this.GetCompressionStream(encodingType, stream, CompressionMode.Compress);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter, otherwise false.</returns>
        public virtual bool Equals(IEncoding other)
        {
            if ((object)other != null)
            {
                return this.GetType().Equals(other.GetType());
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as IEncoding);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        /// <summary>
        /// Gets one of <see cref="GZipStream"/> or <see cref="DeflateStream"/> for the given <see cref="EncodingType"/>.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to get the compression stream for..</param>
        /// <param name="stream">The stream to compress.</param>
        /// <param name="mode">The compression mode to use.</param>
        /// <returns>A stream to use for compression.</returns>
        protected virtual Stream GetCompressionStream(EncodingType encodingType, Stream stream, CompressionMode mode)
        {
            if (encodingType == null)
            {
                throw new ArgumentNullException("encodingType", "encodingType cannot be null.");
            }

            return "deflate".Equals(encodingType.Name, StringComparison.OrdinalIgnoreCase)
                ? new DeflateStream(stream, mode) as Stream
                : new GZipStream(stream, mode) as Stream;
        }
    }
}