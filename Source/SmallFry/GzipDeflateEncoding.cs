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
    }
}