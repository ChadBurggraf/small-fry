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
        /// <summary>
        /// Gets a value indicating whether this instance can decode the given <see cref="EncodingType"/>.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to decode.</param>
        /// <returns>True if this instance can decode the <see cref="EncodingType"/>, false otherwise.</returns>
        public bool CanDecode(EncodingType encodingType)
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether this instance can encode the given <see cref="EncodingType"/>.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> to encode.</param>
        /// <returns>True if this instance can encode the <see cref="EncodingType"/>, false otherwise.</returns>
        public bool CanEncode(EncodingType encodingType)
        {
            return true;
        }

        /// <summary>
        /// Gets a Content-Encoding value to send in responses when the given <see cref="EncodingType"/>
        /// was used to choose this encoding.
        /// </summary>
        /// <param name="encodingType">The <see cref="EncodingType"/> used to choose this encoding.</param>
        /// <returns>A Content-Encoding value to send.</returns>
        public string ContentEncoding(EncodingType encodingType)
        {
            return encodingType.Name;
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
            return stream;
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
            return stream;
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