namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal sealed class FormatFilter : IEquatable<FormatFilter>
    {
        public FormatFilter(string mimeTypes, IFormat format)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format", "format cannot be null.");
            }

            this.MimeTypes = (mimeTypes ?? string.Empty).Trim();
            this.Format = format;
        }

        public IFormat Format { get; private set; }

        public string MimeTypes { get; private set; }

        public bool Equals(FormatFilter other)
        {
            if ((object)other != null)
            {

            }

            return false;
        }

        public static IEnumerable<string> SplitMimeTypes(string mimeTypes)
        {
            mimeTypes = (mimeTypes ?? string.Empty).Trim();
            throw new NotImplementedException();
        }
    }
}