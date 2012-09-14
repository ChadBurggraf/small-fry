namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal sealed class MediaType : IEquatable<MediaType>
    {
        private MediaType()
        {
        }

        public IEnumerable<Params> AcceptParameters { get; private set; }

        public IEnumerable<string> RangeParameters { get; private set; }

        public string RootType { get; private set; }

        public string SubType { get; private set; }

        public bool Equals(MediaType other)
        {
            throw new NotImplementedException();
        }

        #region Extension Class

        public sealed class Extension : IEquatable<Extension>
        {
            private static readonly Regex ParseExpression = new Regex(@"^([^=]+)(\s*=\s*(.*))?$", RegexOptions.Compiled);

            private Extension()
            {
            }

            public string Key { get; private set; }

            public string Value { get; private set; }

            public static bool operator ==(Extension left, Extension right)
            {
                if (Object.ReferenceEquals(left, right))
                {
                    return true;
                }

                object l = (object)left;
                object r = (object)right;

                if ((l != null && r == null)
                    || (l == null && r != null))
                {
                    return false;
                }
                else if (l == null && r == null)
                {
                    return true;
                }
                else
                {
                    return left.Equals(right);
                }
            }

            public static bool operator !=(Extension left, Extension right)
            {
                return !(left == right);
            }

            public static Extension Parse(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value", "value must contain a value.");
                }

                Match match = Extension.ParseExpression.Match(value.Trim());

                if (match.Success)
                {
                    return new Extension()
                    {
                        Key = match.Groups[1].Value.ToLowerInvariant(),
                        Value = match.Groups[3].Value.Coalesce(string.Empty).ToLowerInvariant()
                    };
                }
                else
                {
                    throw new FormatException("Invalid extension format. Format must be: token [ \"=\" ( token | quoted-string ) ]. See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html");
                }
            }

            public static bool TryParse(string value, out Extension result)
            {
                result = null;

                try
                {
                    result = Extension.Parse(value);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            public bool Equals(Extension other)
            {
                if ((object)other != null)
                {
                    return this.Key.Equals(other.Key, StringComparison.OrdinalIgnoreCase)
                        && this.Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as Extension);
            }

            public override int GetHashCode()
            {
                return this.Key.GetHashCode()
                    ^ this.Value.GetHashCode();
            }

            public override string ToString()
            {
                string result = this.Key;

                if (!string.IsNullOrEmpty(this.Value))
                {
                    result += "=" + this.Value;
                }

                return result;
            }
        }

        #endregion

        #region Params Class

        public sealed class Params : IEquatable<Params>
        {
            private static Regex ParseExpression = new Regex(@"^q\s*=\s*([^;]+)(.*)$", RegexOptions.Compiled);

            private Params()
            {
            }

            public IEnumerable<Extension> Extensions { get; private set; }

            public string Value { get; private set; }

            public static bool operator ==(Params left, Params right)
            {
                if (Object.ReferenceEquals(left, right))
                {
                    return true;
                }

                object l = (object)left;
                object r = (object)right;

                if ((l != null && r == null)
                    || (l == null && r != null))
                {
                    return false;
                }
                else if (l == null && r == null)
                {
                    return true;
                }
                else
                {
                    return left.Equals(right);
                }
            }

            public static bool operator !=(Params left, Params right)
            {
                return !(left == right);
            }

            public static Params Parse(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value", "value must contain a value.");
                }

                Match match = Params.ParseExpression.Match(value.Trim());

                if (match.Success)
                {
                    Params result = new Params()
                    {
                        Value = match.Groups[1].Value.Trim().ToLowerInvariant()
                    };

                    if (match.Groups[2].Success && !string.IsNullOrEmpty(match.Groups[2].Value))
                    {
                        result.Extensions = match.Groups[2].Value.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => Extension.Parse(s))
                            .ToArray();
                    }
                    else
                    {
                        result.Extensions = new Extension[0];
                    }

                    return result;
                }
                else
                {
                    throw new FormatException("Invalid params format. Format must be: \"q\" \"=\" qvalue *( accept-extension ). See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html");
                }
            }

            public static bool TryParse(string value, out Params result)
            {
                result = null;

                try
                {
                    result = Params.Parse(value);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            public bool Equals(Params other)
            {
                if ((object)other != null)
                {
                    return this.Extensions.SequenceEqual(other.Extensions)
                        && this.Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as Params);
            }

            public override int GetHashCode()
            {
                return this.Extensions.GetHashCode()
                    ^ this.Value.GetHashCode();
            }

            public override string ToString()
            {
                string result = "q=" + this.Value;

                if (this.Extensions.Any())
                {
                    result += ";" + string.Join(";", this.Extensions);
                }

                return result;
            }
        }

        #endregion
    }
}