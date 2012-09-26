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
    using System.Globalization;
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
        /// Parses an Content-Type value into a <see cref="MediaType"/> instance.
        /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
        /// </summary>
        /// <param name="value">The Content-Type value to parse.</param>
        /// <returns>The parsed <see cref="MediaType"/>.</returns>
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
                AcceptParameters acceptParams = null;

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
                        acceptParams = AcceptParameters.Parse(string.Join(";", paramParts.Skip(acceptIndex).ToArray()));
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
                        Extension[] e = this.AcceptParams.Extensions.ToArray();
                        Extension[] oe = other.AcceptParams.Extensions.ToArray();
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
                                result = this.ToString().CompareTo(other.ToString());
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

            if (this.AcceptParams != AcceptParameters.Empty)
            {
                result += ";" + this.AcceptParams.ToString();
            }

            return result;
        }

        #region Extension Class

        /// <summary>
        /// Represents an Accept-Extension field of a Content-Type.
        /// </summary>
        public sealed class Extension : IEquatable<Extension>
        {
            private static readonly Regex ParseExpression = new Regex(@"^([^=]+)(\s*=\s*(.*))?$", RegexOptions.Compiled);
            private static readonly Extension EmptyExtension = Extension.Parse(null);
            
            private Extension()
            {
            }

            /// <summary>
            /// Gets the empty <see cref="Extension"/> instance.
            /// </summary>
            public static Extension Empty
            {
                get { return Extension.EmptyExtension; }
            }

            /// <summary>
            /// Gets the extension's key value.
            /// </summary>
            public string Key { get; private set; }

            /// <summary>
            /// Gets the extension's value.
            /// </summary>
            public string Value { get; private set; }

            /// <summary>
            /// Gets a value indicating whether two <see cref="Extension"/>s are equal.
            /// </summary>
            /// <param name="left">The left <see cref="Extension"/> to compare.</param>
            /// <param name="right">The right <see cref="Extension"/> to compare.</param>
            /// <returns>True if the <see cref="Extension"/>s are equal, false otherwise.</returns>
            public static bool operator ==(Extension left, Extension right)
            {
                return InternalExtensions.EqualsOperator(left, right);
            }

            /// <summary>
            /// Gets a value indicating whether two <see cref="Extension"/>s are not equal.
            /// </summary>
            /// <param name="left">The left <see cref="Extension"/> to compare.</param>
            /// <param name="right">The right <see cref="Extension"/> to compare.</param>
            /// <returns>True if the <see cref="Extension"/>s are not equal, false otherwise.</returns>
            public static bool operator !=(Extension left, Extension right)
            {
                return !(left == right);
            }

            /// <summary>
            /// Parses a Content-Type Accept-Extension value into an <see cref="Extension"/> instance.
            /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
            /// </summary>
            /// <param name="value">The Accept-Extension value to parse.</param>
            /// <returns>The parsed <see cref="Extension"/>.</returns>
            public static Extension Parse(string value)
            {
                if (!value.IsNullOrWhiteSpace())
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

            /// <summary>
            /// Attempts to parse the given Content-Type Accept-Extension value into an <see cref="Extension"/> instance.
            /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
            /// </summary>
            /// <param name="value">The Accept-Extension value to parse.</param>
            /// <param name="result">The parsed <see cref="Extension"/>.</param>
            /// <returns>True if the value was successfully parsed, otherwise false.</returns>
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

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>True if the current object is equal to the other parameter, false otherwise.</returns>
            public bool Equals(Extension other)
            {
                if ((object)other != null)
                {
                    return this.Key.Equals(other.Key, StringComparison.OrdinalIgnoreCase)
                        && this.Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
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
                return this.Equals(obj as Extension);
            }

            /// <summary>
            /// Serves as a hash function for a particular type.
            /// </summary>
            /// <returns>A hash code for the current object.</returns>
            public override int GetHashCode()
            {
                return this.Key.GetHashCode()
                    ^ this.Value.GetHashCode();
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
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

        /// <summary>
        /// Represents an Accept-Params field of a Content-Type.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        public sealed class AcceptParameters : IEquatable<AcceptParameters>
        {
            private static readonly Regex ParseExpression = new Regex(@"^q\s*=\s*(\d(\.\d+)?)(.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            private static readonly AcceptParameters EmptyParameters = AcceptParameters.Parse(null);
            
            private AcceptParameters()
            {
            }

            /// <summary>
            /// Gets the empty <see cref="AcceptParameters"/> instance.
            /// </summary>
            public static AcceptParameters Empty
            {
                get { return AcceptParameters.EmptyParameters; }
            }

            /// <summary>
            /// Gets the parameters' <see cref="Extension"/> collection.
            /// </summary>
            public IEnumerable<Extension> Extensions { get; private set; }

            /// <summary>
            /// Gets the parameters' q-value.
            /// </summary>
            public float QValue { get; private set; }

            /// <summary>
            /// Gets a value indicating whether two <see cref="AcceptParameters"/>s are equal.
            /// </summary>
            /// <param name="left">The left <see cref="AcceptParameters"/> to compare.</param>
            /// <param name="right">The right <see cref="AcceptParameters"/> to compare.</param>
            /// <returns>True if the <see cref="AcceptParameters"/>s are equal, false otherwise.</returns>
            public static bool operator ==(AcceptParameters left, AcceptParameters right)
            {
                return SmallFry.InternalExtensions.EqualsOperator(left, right);
            }

            /// <summary>
            /// Gets a value indicating whether two <see cref="AcceptParameters"/>s are not equal.
            /// </summary>
            /// <param name="left">The left <see cref="AcceptParameters"/> to compare.</param>
            /// <param name="right">The right <see cref="AcceptParameters"/> to compare.</param>
            /// <returns>True if the <see cref="AcceptParameters"/>s are not equal, false otherwise.</returns>
            public static bool operator !=(AcceptParameters left, AcceptParameters right)
            {
                return !(left == right);
            }

            /// <summary>
            /// Parses an Accept-Params value into an <see cref="AcceptParameters"/> instance.
            /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
            /// </summary>
            /// <param name="value">The Accept-Params value to parse.</param>
            /// <returns>The parsed <see cref="AcceptParameters"/>.</returns>
            [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
            public static AcceptParameters Parse(string value)
            {
                const string FormatExceptionMessage = "Invalid params format. Format must be: \"q\" \"=\" qvalue *( accept-extension ). See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html";

                if (!value.IsNullOrWhiteSpace())
                {
                    Match match = AcceptParameters.ParseExpression.Match(value.Trim());

                    if (match.Success)
                    {
                        float floatValue;

                        if (float.TryParse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out floatValue))
                        {
                            AcceptParameters result = new AcceptParameters()
                            {
                                QValue = floatValue > 1 ? 1 : (floatValue < 0 ? 0 : floatValue)
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
                    return new AcceptParameters() { Extensions = new Extension[0], QValue = 1 };
                }
            }

            /// <summary>
            /// Attempts to parse the given Accept-Params value into an <see cref="AcceptParameters"/> instance.
            /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
            /// </summary>
            /// <param name="value">The Accept-Params value to parse.</param>
            /// <param name="result">The parsed <see cref="AcceptParameters"/>.</param>
            /// <returns>True if the value was successfully parsed, otherwise false.</returns>
            [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
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

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>True if the current object is equal to the other parameter, false otherwise.</returns>
            public bool Equals(AcceptParameters other)
            {
                if ((object)other != null)
                {
                    return this.Extensions.SequenceEqual(other.Extensions)
                        && this.QValue.EqualsFloat(other.QValue, .001f);
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
                return this.Equals(obj as AcceptParameters);
            }

            /// <summary>
            /// Serves as a hash function for a particular type.
            /// </summary>
            /// <returns>A hash code for the current object.</returns>
            public override int GetHashCode()
            {
                return this.Extensions.GetHashCode()
                    ^ this.QValue.GetHashCode();
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                string result = string.Empty;

                if (this != AcceptParameters.Empty)
                {
                    result = "q=" + this.QValue.ToString("0.###", CultureInfo.InvariantCulture);

                    if (this.Extensions.Any())
                    {
                        result += ";" + string.Join(";", this.Extensions.Select(s => s.ToString()).ToArray());
                    }
                }

                return result;
            }
        }

        #endregion
    }
}