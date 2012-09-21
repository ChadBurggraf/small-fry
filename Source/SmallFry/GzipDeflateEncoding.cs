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
    using System.Linq;

    /// <summary>
    /// Implements <see cref="IEncoding"/> to perform GZip and Deflate 
    /// encoding and decoding.
    /// </summary>
    public class GzipDeflateEncoding : IEncoding
    {
        private static readonly string[] Accept = new string[] { "gzip", "deflate" };

        /// <summary>
        /// Gets a collection of content-encoding values this instance can decode.
        /// </summary>
        public virtual IEnumerable<string> AcceptableEncodings
        {
            get { return GzipDeflateEncoding.Accept; }
        }

        /// <summary>
        /// Gets a content-encoding from the given collection of acceptable encodings
        /// this instance can encode. If none of the given encodings can be encoded by
        /// this instance, returns null.
        /// </summary>
        /// <param name="acceptEncodings">A collection of acceptable encoding values.</param>
        /// <returns>A content encoding value, or null if none of the acceptable encodings can be encoded.</returns>
        public virtual string ContentEncoding(IEnumerable<string> acceptEncodings)
        {
            string result = null;

            if (acceptEncodings != null)
            {
                if (acceptEncodings.Any(e => "gzip".Equals(e, StringComparison.OrdinalIgnoreCase)))
                {
                    result = "gzip";
                }
                else if (acceptEncodings.Any(e => "deflate".Equals(e, StringComparison.OrdinalIgnoreCase)))
                {
                    result = "deflate";
                }
            }

            return result;
        }

        /// <summary>
        /// Decodes an input stream and writes the decoded content to the
        /// given output stream.
        /// </summary>
        /// <param name="acceptEncodings">The collection of acceptable encoding values used
        /// to choose this encoding.</param>
        /// <param name="inputStream">The stream to read encoded content from.</param>
        /// <param name="outputStream">The stream to write decoded content to.</param>
        public void Decode(IEnumerable<string> acceptEncodings, Stream inputStream, Stream outputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream", "inputStream cannot be null.");
            }

            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream", "outputStream cannot be null.");
            }

            using (Stream compressionStream = this.GetCompressionStream(acceptEncodings, inputStream, CompressionMode.Decompress))
            {
                compressionStream.CopyTo(outputStream);
            }
        }

        /// <summary>
        /// Encodes an input stream and writes the encoded content to the
        /// given output stream.
        /// </summary>
        /// <param name="acceptEncodings">The collection of acceptable encoding values used
        /// to choose this encoding.</param>
        /// <param name="inputStream">The input stream to read content from.</param>
        /// <param name="outputStream">The output stream to write encoded content to.</param>
        public void Encode(IEnumerable<string> acceptEncodings, Stream inputStream, Stream outputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream", "inputStream cannot be null.");
            }

            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream", "outputStream cannot be null.");
            }

            using (Stream compressionStream = this.GetCompressionStream(acceptEncodings, outputStream, CompressionMode.Compress))
            {
                inputStream.CopyTo(compressionStream);
            }
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
        /// Gets one of <see cref="GZipStream"/> or <see cref="DeflateStream"/> for the given collection of accept-encodings.
        /// This method should call <see cref="ContentEncoding(IEnumerable<strong>)"/> to determine the accept-encoding to pick.
        /// </summary>
        /// <param name="acceptEncodings">The collection of accept-encoding values used to choose the encoding.</param>
        /// <param name="inputStream">The input stream to read from.</param>
        /// <param name="mode">The compression mode to use.</param>
        /// <returns>A stream to use for compression.</returns>
        protected virtual Stream GetCompressionStream(IEnumerable<string> acceptEncodings, Stream inputStream, CompressionMode mode)
        {
            return "deflate".Equals(this.ContentEncoding(acceptEncodings), StringComparison.OrdinalIgnoreCase)
                ? new DeflateStream(inputStream, mode) as Stream
                : new GZipStream(inputStream, mode) as Stream;
        }
    }
}