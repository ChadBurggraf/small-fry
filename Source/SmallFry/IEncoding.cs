//-----------------------------------------------------------------------------
// <copyright file="IEncoding.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.IO;

    public interface IEncoding : IEquatable<IEncoding>
    {
        void Decode(Stream inputStream, Stream outputStream);

        void Encode(Stream inputStream, Stream outputStream);
    }
}