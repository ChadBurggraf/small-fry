//-----------------------------------------------------------------------------
// <copyright file="GzipDeflateEncoding.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.IO;

    public sealed class GzipDeflateEncoding : IEncoding
    {
        public void Decode(Stream inputStream, Stream outputStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(Stream inputStream, Stream outputStream)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IEncoding other)
        {
            if ((object)other != null)
            {
                return this.GetType().Equals(other.GetType());
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IEncoding);
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }
    }
}