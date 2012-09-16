//-----------------------------------------------------------------------------
// <copyright file="IFormat.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.IO;

    public interface IFormat : IEquatable<IFormat>
    {
        object Deserialize(Type type, Stream stream);

        void Serialize(object value, Stream stream);
    }
}