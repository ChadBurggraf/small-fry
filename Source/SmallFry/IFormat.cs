//-----------------------------------------------------------------------------
// <copyright file="IFormat.cs" company="Tasty Codes">
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
    /// formats.
    /// </summary>
    public interface IFormat : IEquatable<IFormat>
    {
        /// <summary>
        /// Gets a value indicating whether this instance can deserialize the given <see cref="MediaType"/>.
        /// </summary>
        /// <param name="mediaType">The <see cref="MediaType"/> to decode.</param>
        /// <returns>True if this instance can deserialize the <see cref="MediaType"/>, false otherwise.</returns>
        bool CanDeserialize(MediaType mediaType);

        /// <summary>
        /// Gets a value indicating whether this instance can serialize the given <see cref="MediaType"/>.
        /// </summary>
        /// <param name="encodingType">The <see cref="MediaType"/> to encode.</param>
        /// <returns>True if this instance can serialize the <see cref="MediaType"/>, false otherwise.</returns>
        bool CanSerialize(MediaType mediaType);

        /// <summary>
        /// Deserializes an object of the given type from the given input stream.
        /// </summary>
        /// <param name="mediaType">The <see cref="MediaType"/> to deserialize.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="stream">The stream to deserialize the object from.</param>
        /// <returns>A deserialized object of the specified type.</returns>
        object Deserialize(MediaType mediaType, Type type, Stream stream);

        /// <summary>
        /// Serializes an object to the given output stream.
        /// </summary>
        /// <param name="mediaType">The <see cref="MediaType"/> to serialize.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="stream">The stream to write the serialized object to.</param>
        void Serialize(MediaType mediaType, object value, Stream stream);
    }
}