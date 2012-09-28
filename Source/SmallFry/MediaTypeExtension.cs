//-----------------------------------------------------------------------------
// <copyright file="MediaTypeExtension.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents an Accept-Extension field of a Content-Type.
    /// </summary>
    public sealed class MediaTypeExtension : IEquatable<MediaTypeExtension>
    {
        private static readonly Regex ParseExpression = new Regex(@"^([^=]+)(\s*=\s*(.*))?$", RegexOptions.Compiled);
        private static readonly MediaTypeExtension EmptyExtension = MediaTypeExtension.Parse(null);

        private MediaTypeExtension()
        {
        }

        /// <summary>
        /// Gets the empty <see cref="MediaTypeExtension"/> instance.
        /// </summary>
        public static MediaTypeExtension Empty
        {
            get { return MediaTypeExtension.EmptyExtension; }
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
        /// Gets a value indicating whether two <see cref="MediaTypeExtension"/>s are equal.
        /// </summary>
        /// <param name="left">The left <see cref="MediaTypeExtension"/> to compare.</param>
        /// <param name="right">The right <see cref="MediaTypeExtension"/> to compare.</param>
        /// <returns>True if the <see cref="MediaTypeExtension"/>s are equal, false otherwise.</returns>
        public static bool operator ==(MediaTypeExtension left, MediaTypeExtension right)
        {
            return InternalExtensions.EqualsOperator(left, right);
        }

        /// <summary>
        /// Gets a value indicating whether two <see cref="MediaTypeExtension"/>s are not equal.
        /// </summary>
        /// <param name="left">The left <see cref="MediaTypeExtension"/> to compare.</param>
        /// <param name="right">The right <see cref="MediaTypeExtension"/> to compare.</param>
        /// <returns>True if the <see cref="MediaTypeExtension"/>s are not equal, false otherwise.</returns>
        public static bool operator !=(MediaTypeExtension left, MediaTypeExtension right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Parses a Content-Type Accept-Extension value into an <see cref="MediaTypeExtension"/> instance.
        /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
        /// </summary>
        /// <param name="value">The Accept-Extension value to parse.</param>
        /// <returns>The parsed <see cref="MediaTypeExtension"/>.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Standard use is lowercase.")]
        public static MediaTypeExtension Parse(string value)
        {
            if (!value.IsNullOrWhiteSpace())
            {
                Match match = MediaTypeExtension.ParseExpression.Match(value.Trim());

                if (match.Success)
                {
                    return new MediaTypeExtension()
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
                return new MediaTypeExtension() { Key = string.Empty, Value = string.Empty };
            }
        }

        /// <summary>
        /// Attempts to parse the given Content-Type Accept-Extension value into an <see cref="MediaTypeExtension"/> instance.
        /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
        /// </summary>
        /// <param name="value">The Accept-Extension value to parse.</param>
        /// <param name="result">The parsed <see cref="MediaTypeExtension"/>.</param>
        /// <returns>True if the value was successfully parsed, otherwise false.</returns>
        public static bool TryParse(string value, out MediaTypeExtension result)
        {
            result = null;

            try
            {
                result = MediaTypeExtension.Parse(value);
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
        public bool Equals(MediaTypeExtension other)
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
            return this.Equals(obj as MediaTypeExtension);
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

            if (this != MediaTypeExtension.Empty)
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
}
