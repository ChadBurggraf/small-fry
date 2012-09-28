//-----------------------------------------------------------------------------
// <copyright file="MediaType.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a Content-Type.
    /// </summary>
    public sealed class MediaType : IEquatable<MediaType>, IComparable<MediaType>
    {
        private static readonly Regex AcceptParamsStartExpression = new Regex(@"^\s*q\s*=.*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex ParseExpression = new Regex(@"^(\*/\*|\*/[a-z-0-9]+|[a-z0-9]+/\*|[a-z0-9]+/[a-z0-9]+)(.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
        /// Gets the <see cref="MediaTypeAcceptParameters"/> identified by this instance.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Params", Justification = "Reviewed.")]
        public MediaTypeAcceptParameters AcceptParams { get; private set; }

        /// <summary>
        /// Gets the range parameters identified by this instance.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Params", Justification = "Reviewed.")]
        public IEnumerable<string> RangeParams { get; private set; }

        /// <summary>
        /// Gets the root content type.
        /// </summary>
        public string RootType { get; private set; }

        /// <summary>
        /// Gets the sub content type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SubType", Justification = "Consistent with RootType.")]
        public string SubType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether two <see cref="MediaType"/>s are equal.
        /// </summary>
        /// <param name="left">The left <see cref="MediaType"/> to compare.</param>
        /// <param name="right">The right <see cref="MediaType"/> to compare.</param>
        /// <returns>True if the <see cref="MediaType"/>s are equal, false otherwise.</returns>
        public static bool operator ==(MediaType left, MediaType right)
        {
            return InternalExtensions.EqualsOperator(left, right);
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
        /// Gets a value indicating whether a <see cref="MediaType"/> is greater
        /// than a comparable <see cref="MediaType"/>.
        /// </summary>
        /// <param name="left">The left <see cref="MediaType"/> to compare.</param>
        /// <param name="right">The right <see cref="MediaType"/> to compare.</param>
        /// <returns>True if the left <see cref="MediaType"/> is greater than the right
        /// <see cref="MediaType"/>, false otherwise.</returns>
        public static bool operator >(MediaType left, MediaType right)
        {
            return InternalExtensions.GreaterThanOperator(left, right);
        }

        /// <summary>
        /// Gets a value indicating whether a <see cref="MediaType"/> is less
        /// than a comparable <see cref="MediaType"/>.
        /// </summary>
        /// <param name="left">The left <see cref="MediaType"/> to compare.</param>
        /// <param name="right">The right <see cref="MediaType"/> to compare.</param>
        /// <returns>True if the left <see cref="MediaType"/> is less than the right
        /// <see cref="MediaType"/>, false otherwise.</returns>
        public static bool operator <(MediaType left, MediaType right)
        {
            return InternalExtensions.LessThanOperator(left, right);
        }

        /// <summary>
        /// Parses an Content-Type value into a <see cref="MediaType"/> instance.
        /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
        /// </summary>
        /// <param name="value">The Content-Type value to parse.</param>
        /// <returns>The parsed <see cref="MediaType"/>.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Standard use is lowercase.")]
        public static MediaType Parse(string value)
        {
            if (value.IsNullOrWhiteSpace())
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
                MediaTypeAcceptParameters acceptParams = null;

                if (match.Groups[2].Success && !match.Groups[2].Value.IsNullOrWhiteSpace())
                {
                    string[] paramParts = match.Groups[2].Value.Split(new char[] { ';' });
                    int acceptIndex = -1;

                    for (int i = 0; i < paramParts.Length; i++)
                    {
                        string part = paramParts[i];

                        if (!part.IsNullOrWhiteSpace())
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
                        acceptParams = MediaTypeAcceptParameters.Parse(string.Join(";", paramParts.Skip(acceptIndex).ToArray()));
                    }
                }

                result.RangeParams = parameters.ToArray();
                result.AcceptParams = acceptParams ?? MediaTypeAcceptParameters.Empty;
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

            bool result = false;

            if (this.RootType == "*" || this.RootType.Equals(other.RootType, StringComparison.OrdinalIgnoreCase))
            {
                if (this.SubType == "*" || this.SubType.Equals(other.SubType, StringComparison.OrdinalIgnoreCase))
                {
                    result = true;

                    string[] r = this.RangeParams.ToArray();
                    string[] or = other.RangeParams.ToArray();
                    int rl = r.Length, ol = or.Length;

                    // This instance accepts anything more specific. So if we have
                    // range params, the other instance must have at least all of the
                    // range params we have. They may have more range params, however.
                    for (int i = 0; i < rl; i++)
                    {
                        if (i < ol)
                        {
                            if (!r[i].Equals(or[i], StringComparison.Ordinal))
                            {
                                result = false;
                                break;
                            }
                        }
                        else
                        {
                            result = false;
                            break;
                        }
                    }

                    // Same as above for extension params.
                    if (result)
                    {
                        MediaTypeExtension[] e = this.AcceptParams.Extensions.ToArray();
                        MediaTypeExtension[] oe = other.AcceptParams.Extensions.ToArray();
                        rl = e.Length;
                        ol = oe.Length;

                        for (int i = 0; i < rl; i++)
                        {
                            if (i < ol)
                            {
                                if (!e[i].Equals(oe[i]))
                                {
                                    result = false;
                                    break;
                                }
                            }
                            else
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(MediaType other)
        {
            int result = 1;

            if (other != null)
            {
                result = this.AcceptParams.QValue.CompareTo(other.AcceptParams.QValue);

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
                                result = string.Compare(this.ToString(), other.ToString(), StringComparison.OrdinalIgnoreCase);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>True if the current object is equal to the other parameter, false otherwise.</returns>
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

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as MediaType);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.AcceptParams.GetHashCode()
                ^ this.RangeParams.GetHashCode()
                ^ this.RootType.GetHashCode()
                ^ this.SubType.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object
        /// as a Content-Type value (i.e., without any <see cref="AcceptParams"/>).
        /// </summary>
        /// <returns>A string that represents this object as a Content-Type.</returns>
        public string ToContentTypeString()
        {
            string result = this.RootType + "/" + this.SubType;

            if (this.RangeParams.Any())
            {
                result += ";" + string.Join(";", this.RangeParams.ToArray());
            }

            return result;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string result = this.RootType + "/" + this.SubType;

            if (this.RangeParams.Any())
            {
                result += ";" + string.Join(";", this.RangeParams.ToArray());
            }

            if (this.AcceptParams != MediaTypeAcceptParameters.Empty)
            {
                result += ";" + this.AcceptParams.ToString();
            }

            return result;
        }
    }
}