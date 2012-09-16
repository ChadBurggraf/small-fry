//-----------------------------------------------------------------------------
// <copyright file="IFormat.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.IO;

    /// <summary>
    /// Defines the interface for handling incoming and outgoing content
    /// formats.
    /// </summary>
    public interface IFormat : IEquatable<IFormat>
    {
        /// <summary>
        /// Deserializes an object of the given type from the given input stream.
        /// </summary>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="stream">The stream to deserialize the object from.</param>
        /// <returns>A deserialized object of the specified type.</returns>
        object Deserialize(Type type, Stream stream);

        /// <summary>
        /// Serializes an object to the given output stream.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="stream">The stream to write the serialized object to.</param>
        void Serialize(object value, Stream stream);
    }
}