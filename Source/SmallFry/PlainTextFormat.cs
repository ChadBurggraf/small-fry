//-----------------------------------------------------------------------------
// <copyright file="PlainTextFormat.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    
    /// <summary>
    /// Implements <see cref="IFormat"/> to read and write plain text content.
    /// </summary>
    public class PlainTextFormat : IFormat
    {
        private static readonly string[] Accept = new string[] { "*/*" };

        /// <summary>
        /// Gets a collection of content-type values this instance can deserialize.
        /// </summary>
        public virtual IEnumerable<string> AcceptableFormats
        {
            get { return PlainTextFormat.Accept; }
        }

        /// <summary>
        /// Gets a content-type from the given collection of acceptable types
        /// this instance can serialize. If none of the given types can be serialized by
        /// this instance, returns null.
        /// </summary>
        /// <param name="accept">A collection of accept values.</param>
        /// <returns>A content type value, or null if none of the acceptable types can be serialized..</returns>
        public virtual string ContentType(IEnumerable<string> accept)
        {
            return "text/plain";
        }

        /// <summary>
        /// Deserializes an object of the given type from the given input stream.
        /// </summary>
        /// <param name="accept">The collection of accept values used to choose this format.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="stream">The stream to deserialize the object from.</param>
        /// <returns>A deserialized object of the specified type.</returns>
        public object Deserialize(IEnumerable<string> accept, Type type, Stream stream)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type cannot be null.");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream", "stream cannot be null.");
            }

            if (typeof(string).IsAssignableFrom(type))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }

            return type.Default();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter, otherwise false.</returns>
        public virtual bool Equals(IFormat other)
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
        /// <param name="accept">The collection of accept values used to choose this format.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="stream">The stream to write the serialized object to.</param>
        public void Serialize(IEnumerable<string> accept, object value, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream", "stream cannot be null.");
            }

            if (value != null)
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(value.ToString());
                }
            }
        }
    }
}