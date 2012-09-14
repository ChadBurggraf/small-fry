namespace SmallFry
{
    using System;

    internal sealed class FormatFilter
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
    }
}