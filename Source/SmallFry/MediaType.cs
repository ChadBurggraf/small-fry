//-----------------------------------------------------------------------------
// <copyright file="MediaType.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a Content-Type.
    /// </summary>
    public sealed class MediaType : IEquatable<MediaType>, IComparable<MediaType>
    {
        private static readonly Regex AcceptParamsStartExpression = new Regex(@"^\s*q\s*=.*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex ParseExpression = new Regex(@"^(\*/\*|[a-z0-9]+/\*|[a-z0-9]+/[a-z0-9]+)(.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly MediaType EmptyType = MediaType.Parse(null);
        
        private MediaType()
        {
        }

        /// <summary>
        /// Gets the empty <see cref="MediaType"/> instance.
        /// </summary>
        public static MediaType Empty
        {
            get { return MediaType.EmptyType; }
        }

        /// <summary>
        /// Gets the <see cref="AcceptParameters"/> identified by this instance.
        /// </summary>
        public AcceptParameters AcceptParams { get; private set; }

        /// <summary>
        /// Gets the range parameters identified by this instance.
        /// </summary>
        public IEnumerable<string> RangeParams { get; private set; }

        /// <summary>
        /// Gets the root content type.
        /// </summary>
        public string RootType { get; private set; }

        /// <summary>
        /// Gets the sub content type.
        /// </summary>
        public string SubType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether two <see cref="MediaType"/>s are equal.
        /// </summary>
        /// <param name="left">The left <see cref="MediaType"/> to compare.</param>
        /// <param name="right">The right <see cref="MediaType"/> to compare.</param>
        /// <returns>True if the <see cref="MediaType"/>s are equal, false otherwise.</returns>
        public static bool operator ==(MediaType left, MediaType right)
        {
            return Extensions.EqualsOperator(left, right);
        }

        /// <summary>
        /// Gets a value indicating whether two <see cref="MediaType"/>s are not equal.
        /// </summary>
        /// <param name="left">The left <see cref="MediaType"/> to compare.</param>
        /// <param name="right">The right <see cref="MediaType"/> to compare.</param>
        /// <returns>True if the <see cref="MediaType"/>s are not equal, false otherwise.</returns>
        public static bool operator !=(MediaType left, MediaType right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Parses an Content-Type value into a <see cref="MediaType"/> instance.
        /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
        /// </summary>
        /// <param name="value">The Content-Type value to parse.</param>
        /// <returns>The parsed <see cref="MediaType"/>.</returns>
        public static MediaType Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                value = "*/*";
            }

            Match match = MediaType.ParseExpression.Match(value);

            if (match.Success)
            {
                string[] typeParts = match.Groups[1].Value.Split('/');

                MediaType result = new MediaType()
                {
                    RootType = typeParts[0].ToLowerInvariant(),
                    SubType = typeParts[1].ToLowerInvariant()
                };

                List<string> parameters = new List<string>();
                AcceptParameters acceptParams = null;

                if (match.Groups[2].Success && !string.IsNullOrWhiteSpace(match.Groups[2].Value))
                {
                    string[] paramParts = match.Groups[2].Value.Split(new char[] { ';' });
                    int acceptIndex = -1;

                    for (int i = 0; i < paramParts.Length; i++)
                    {
                        string part = paramParts[i];

                        if (!string.IsNullOrWhiteSpace(part))
                        {
                            if (!MediaType.AcceptParamsStartExpression.IsMatch(part))
                            {
                                parameters.Add(part.Trim().ToLowerInvariant());
                            }
                            else
                            {
                                acceptIndex = i;
                                break;
                            }
                        }
                    }

                    if (acceptIndex > -1)
                    {
                        acceptParams = AcceptParameters.Parse(string.Join(";", paramParts.Skip(acceptIndex)));
                    }
                }

                result.RangeParams = parameters.ToArray();
                result.AcceptParams = acceptParams ?? AcceptParameters.Empty;
                return result;
            }
            else
            {
                throw new FormatException("Invalid media type format. Format must be: ( ( \"*/*\" | ( type \"/\" \"*\" ) | ( type \"/\" subtype ) ) *( \";\" parameter ) [ accept-params ] ). See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html");
            }
        }

        /// <summary>
        /// Attempts to parse the given Content-Encoding value into an <see cref="MediaType"/> instance.
        /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
        /// </summary>
        /// <param name="value">The Content-Type value to parse.</param>
        /// <param name="result">The parsed <see cref="MediaType"/>.</param>
        /// <returns>True if the value was successfully parsed, otherwise false.</returns>
        public static bool TryParse(string value, out MediaType result)
        {
            result = null;

            try
            {
                result = MediaType.Parse(value);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance represents a superset of the given instance
        /// (i.e., the given instance is less specific, or this instance represents the wildcard type, etc.).
        /// </summary>
        /// <param name="other">The <see cref="MediaType"/> to compare with this instance.</param>
        /// <returns>True if this instance accepts the given instance, false otherwise.</returns>
        public bool Accepts(MediaType other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other", "other cannot be null.");
            }

            if (this.RootType == "*" || this.RootType.Equals(other.RootType, StringComparison.OrdinalIgnoreCase))
            {
                if (this.SubType == "*" || this.SubType.Equals(other.SubType, StringComparison.OrdinalIgnoreCase))
                {
                    if (other.RangeParams.Any())
                    {
                    }
                }
            }
            return other == null
                || other.Equals(MediaType.Empty)
                || this.Equals(MediaType.Empty)
                || ((this.RootType == "*"
                || other.RootType == "*"
                || this.RootType.Equals(other.RootType, StringComparison.OrdinalIgnoreCase))
                && (this.SubType == "*"
                || other.SubType == "*"
                || this.SubType.Equals(other.SubType, StringComparison.OrdinalIgnoreCase)));
        }

        public int CompareTo(MediaType other)
        {
            int result = 1;

            if (other != null)
            {
                result = this.AcceptParams.Value.CompareTo(other.AcceptParams.Value);

                if (result == 0)
                {
                    int rangeCount = this.RangeParams.Count();
                    int otherRangeCount = other.RangeParams.Count();

                    if (rangeCount > otherRangeCount)
                    {
                        result = 1;
                    }
                    else if (rangeCount < otherRangeCount)
                    {
                        result = -1;
                    }
                    else
                    {
                        if (this.SubType != "*" && other.SubType == "*")
                        {
                            result = 1;
                        }
                        else if (this.SubType == "*" && other.SubType != "*")
                        {
                            result = -1;
                        }
                        else
                        {
                            if (this.RootType != "*" && other.SubType == "*")
                            {
                                result = 1;
                            }
                            else if (this.SubType == "*" && other.SubType != "*")
                            {
                                result = -1;
                            }
                            else
                            {
                                result = this.ToString().CompareTo(other.ToString());
                            }
                        }
                    }
                }
            }

            return result;
        }

        public bool Equals(MediaType other)
        {
            if ((object)other != null)
            {
                return this.AcceptParams.Equals(other.AcceptParams)
                    && this.RangeParams.SequenceEqual(other.RangeParams)
                    && this.RootType.Equals(other.RootType, StringComparison.OrdinalIgnoreCase)
                    && this.SubType.Equals(other.SubType, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as MediaType);
        }

        public override int GetHashCode()
        {
            return this.AcceptParams.GetHashCode()
                ^ this.RangeParams.GetHashCode()
                ^ this.RootType.GetHashCode()
                ^ this.SubType.GetHashCode();
        }

        public override string ToString()
        {
            string result = this.RootType + "/" + this.SubType;

            if (this.RangeParams.Any())
            {
                result += ";" + string.Join(";", this.RangeParams);
            }

            if (this.AcceptParams != AcceptParameters.Empty)
            {
                result += ";" + this.AcceptParams.ToString();
            }

            return result;
        }

        #region Extension Class

        public sealed class Extension : IEquatable<Extension>
        {
            private static readonly Regex ParseExpression = new Regex(@"^([^=]+)(\s*=\s*(.*))?$", RegexOptions.Compiled);
            private static readonly Extension EmptyExtension = Extension.Parse(null);
            
            private Extension()
            {
            }

            public static Extension Empty
            {
                get { return Extension.EmptyExtension; }
            }

            public string Key { get; private set; }

            public string Value { get; private set; }

            public static bool operator ==(Extension left, Extension right)
            {
                return Extensions.EqualsOperator(left, right);
            }

            public static bool operator !=(Extension left, Extension right)
            {
                return !(left == right);
            }

            public static Extension Parse(string value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
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
                else
                {
                    return new Extension() { Key = string.Empty, Value = string.Empty };
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
                string result = string.Empty;

                if (this != Extension.Empty)
                {
                    result = this.Key;

                    if (!string.IsNullOrEmpty(this.Value))
                    {
                        result += "=" + this.Value;
                    }
                }

                return result;
            }
        }

        #endregion

        #region AcceptParameters Class

        public sealed class AcceptParameters : IEquatable<AcceptParameters>
        {
            private static readonly Regex ParseExpression = new Regex(@"^q\s*=\s*(\d(\.\d+)?)(.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            private static readonly AcceptParameters EmptyParameters = AcceptParameters.Parse(null);
            
            private AcceptParameters()
            {
            }

            public static AcceptParameters Empty
            {
                get { return AcceptParameters.EmptyParameters; }
            }

            public IEnumerable<Extension> Extensions { get; private set; }

            public float Value { get; private set; }

            public static bool operator ==(AcceptParameters left, AcceptParameters right)
            {
                return SmallFry.Extensions.EqualsOperator(left, right);
            }

            public static bool operator !=(AcceptParameters left, AcceptParameters right)
            {
                return !(left == right);
            }

            public static AcceptParameters Parse(string value)
            {
                const string FormatExceptionMessage = "Invalid params format. Format must be: \"q\" \"=\" qvalue *( accept-extension ). See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    Match match = AcceptParameters.ParseExpression.Match(value.Trim());

                    if (match.Success)
                    {
                        float floatValue;

                        if (float.TryParse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out floatValue))
                        {
                            AcceptParameters result = new AcceptParameters()
                            {
                                Value = floatValue > 1 ? 1 : (floatValue < 0 ? 0 : floatValue)
                            };

                            if (match.Groups[3].Success && !string.IsNullOrEmpty(match.Groups[3].Value))
                            {
                                result.Extensions = match.Groups[3].Value.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
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
                            throw new FormatException(FormatExceptionMessage);
                        }
                    }
                    else
                    {
                        throw new FormatException(FormatExceptionMessage);
                    }
                }
                else
                {
                    return new AcceptParameters() { Extensions = new Extension[0], Value = 1 };
                }
            }

            public static bool TryParse(string value, out AcceptParameters result)
            {
                result = null;

                try
                {
                    result = AcceptParameters.Parse(value);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            public bool Equals(AcceptParameters other)
            {
                if ((object)other != null)
                {
                    return this.Extensions.SequenceEqual(other.Extensions)
                        && this.Value.EqualsFloat(other.Value, .001f);
                }

                return false;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as AcceptParameters);
            }

            public override int GetHashCode()
            {
                return this.Extensions.GetHashCode()
                    ^ this.Value.GetHashCode();
            }

            public override string ToString()
            {
                string result = string.Empty;

                if (this != AcceptParameters.Empty)
                {
                    result = "q=" + this.Value.ToString("0.###", CultureInfo.InvariantCulture);

                    if (this.Extensions.Any())
                    {
                        result += ";" + string.Join(";", this.Extensions);
                    }
                }

                return result;
            }
        }

        #endregion
    }
}