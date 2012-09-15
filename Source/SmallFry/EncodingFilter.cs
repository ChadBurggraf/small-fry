namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class EncodingFilter : IEquatable<EncodingFilter>
    {
        public EncodingFilter(string accept, IEncoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding", "encoding cannot be null.");
            }

            this.AcceptTypes = EncodingFilter.ParseAcceptTypes(accept);
            this.Encoding = encoding;
        }

        public static bool operator ==(EncodingFilter left, EncodingFilter right)
        {
            return Extensions.EqualsOperator(left, right);
        }

        public static bool operator !=(EncodingFilter left, EncodingFilter right)
        {
            return !(left == right);
        }

        public IEnumerable<EncodingType> AcceptTypes { get; private set; }

        public IEncoding Encoding { get; private set; }

        public static IEnumerable<EncodingType> ParseAcceptTypes(string accept)
        {
            if (string.IsNullOrWhiteSpace(accept))
            {
                accept = "*";
            }

            return accept.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => EncodingType.Parse(s))
                .Distinct()
                .OrderByDescending(e => e.QValue)
                .ThenBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        public bool Equals(EncodingFilter other)
        {
            if ((object)other != null)
            {
                return this.AcceptTypes.SequenceEqual(other.AcceptTypes)
                    && this.Encoding.Equals(other.Encoding);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as EncodingFilter);
        }

        public override int GetHashCode()
        {
            return this.AcceptTypes.GetHashCode()
                ^ this.Encoding.GetHashCode();
        }
    }
}