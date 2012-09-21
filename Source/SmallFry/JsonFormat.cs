//-----------------------------------------------------------------------------
// <copyright file="JsonFormat.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ServiceStack.Text;

    /// <summary>
    /// Implements <see cref="IFormat"/> to read and write JSON content.
    /// </summary>
    public class JsonFormat : IFormat
    {
        private static readonly MediaType ApplicationJson = MediaType.Parse("application/json");

        static JsonFormat()
        {
            JsConfig.EmitCamelCaseNames = true;
            JsConfig.IncludeNullValues = false;
        }

        /// <summary>
        /// Gets a value indicating whether this instance can deserialize the given <see cref="MediaType"/>.
        /// </summary>
        /// <param name="mediaType">The <see cref="MediaType"/> to decode.</param>
        /// <returns>True if this instance can deserialize the <see cref="MediaType"/>, false otherwise.</returns>
        public virtual bool CanDeserialize(MediaType mediaType)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType", "mediaType cannot be null.");
            }

            return JsonFormat.ApplicationJson.Accepts(mediaType);
        }

        /// <summary>
        /// Gets a value indicating whether this instance can serialize the given <see cref="MediaType"/>.
        /// </summary>
        /// <param name="encodingType">The <see cref="MediaType"/> to encode.</param>
        /// <returns>True if this instance can serialize the <see cref="MediaType"/>, false otherwise.</returns>
        public virtual bool CanSerialize(MediaType mediaType)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType", "mediaType cannot be null.");
            }

            return mediaType.Accepts(JsonFormat.ApplicationJson);
        }

        /// <summary>
        /// Deserializes an object of the given type from the given input stream.
        /// </summary>
        /// <param name="mediaType">The <see cref="MediaType"/> to deserialize.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="stream">The stream to deserialize the object from.</param>
        /// <returns>A deserialized object of the specified type.</returns>
        public object Deserialize(MediaType mediaType, Type type, Stream stream)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type cannot be null.");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream", "stream cannot be null.");
            }

            return JsonSerializer.DeserializeFromStream(type, stream);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter, otherwise false.</returns>
        public bool Equals(IFormat other)
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
            return this.Equals(obj as IFormat);
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
        /// Serializes an object to the given output stream.
        /// </summary>
        /// <param name="mediaType">The <see cref="MediaType"/> to serialize.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="stream">The stream to write the serialized object to.</param>
        public void Serialize(MediaType mediaType, object value, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream", "stream cannot be null.");
            }

            if (value != null)
            {
                JsonSerializer.SerializeToStream(value, value.GetType(), stream);
            }
        }
    }
}