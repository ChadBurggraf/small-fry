//-----------------------------------------------------------------------------
// <copyright file="IdentityEncoding.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Implements <see cref="IEncoding"/> for the identity (empty) encoding.
    /// </summary>
    public class IdentityEncoding : IEncoding
    {
        private static readonly string[] Accept = new string[] { "*" };

        /// <summary>
        /// Gets a collection of content-encoding values this instance can decode.
        /// </summary>
        public virtual IEnumerable<string> AcceptableEncodings
        {
            get { return IdentityEncoding.Accept; }
        }

        /// <summary>
        /// Gets a content-encoding from the given collection of acceptable encodings
        /// this instance can encode. If none of the given encodings can be encoded by
        /// this instance, return null.
        /// </summary>
        /// <param name="acceptEncodings">A collection of acceptable encoding values.</param>
        /// <returns>A content encoding value, or null if none of the acceptable encodings can be encoded.</returns>
        public virtual string ContentEncoding(IEnumerable<string> acceptEncodings)
        {
            return acceptEncodings != null ? acceptEncodings.FirstOrDefault() : null;
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
    }
}