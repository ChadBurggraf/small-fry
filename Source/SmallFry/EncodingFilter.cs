namespace SmallFry
{
    using System;

    internal sealed class EncodingFilter
    {
        public EncodingFilter(string names, IEncoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding", "encoding cannot be null.");
            }

            this.Names = (names ?? string.Empty).Trim();
            this.Encoding = encoding;
        }

        public IEncoding Encoding { get; private set; }

        public string Names { get; private set; }
    }
}