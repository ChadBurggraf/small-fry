namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class FormatFilter : IEquatable<FormatFilter>
    {
        public FormatFilter(string mediaTypes, IFormat format)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format", "format cannot be null.");
            }

            this.MediaTypes = FormatFilter.ParseMediaTypes(mediaTypes);
            this.Format = format;
        }

        public static bool operator ==(FormatFilter left, FormatFilter right)
        {
            return Extensions.EqualsOperator(left, right);
        }

        public static bool operator !=(FormatFilter left, FormatFilter right)
        {
            return !(left == right);
        }

        public IFormat Format { get; private set; }

        public IEnumerable<MediaType> MediaTypes { get; private set; }

        public bool Equals(FormatFilter other)
        {
            if ((object)other != null)
            {
                return this.Format.Equals(other)
                    && this.MediaTypes.SequenceEqual(other.MediaTypes);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as FormatFilter);
        }

        public override int GetHashCode()
        {
            return this.Format.GetHashCode()
                ^ this.MediaTypes.GetHashCode();
        }

        public static IEnumerable<MediaType> ParseMediaTypes(string mediaTypes)
        {
            if (string.IsNullOrWhiteSpace(mediaTypes))
            {
                mediaTypes = "*/*";
            }

            return mediaTypes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => MediaType.Parse(s))
                .Distinct()
                .OrderByDescending(m => m.AcceptParams.Value)
                .ThenBy(m => m.RootType)
                .ThenBy(m => m.SubType)
                .ToArray();
        }
    }
}