//-----------------------------------------------------------------------------
// <copyright file="EncodingType.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a Content-Encoding type.
    /// </summary>
    public sealed class EncodingType : IEquatable<EncodingType>, IComparable<EncodingType>
    {
        private static readonly Regex ParseExpression = new Regex(@"^\s*([^;]+)(\s*;\s*q\s*=\s*(\d(\.\d*)?))?\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly EncodingType EmptyType = EncodingType.Parse(null);
        
        private EncodingType()
        {
        }

        /// <summary>
        /// Gets the empty <see cref="EncodingType"/> instance.
        /// </summary>
        public static EncodingType Empty
        {
            get { return EncodingType.EmptyType; }
        }

        /// <summary>
        /// Gets the encoding type's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the encoding type's q-value.
        /// </summary>
        public float QValue { get; private set; }

        /// <summary>
        /// Gets a value indicating whether two <see cref="EncodingType"/>s are equal.
        /// </summary>
        /// <param name="left">The left <see cref="EncodingType"/> to compare.</param>
        /// <param name="right">The right <see cref="EncodingType"/> to compare.</param>
        /// <returns>True if the <see cref="EncodingType"/>s are equal, false otherwise.</returns>
        public static bool operator ==(EncodingType left, EncodingType right)
        {
            return Extensions.EqualsOperator(left, right);
        }

        /// <summary>
        /// Gets a value indicating whether two <see cref="EncodingType"/>s are not equal.
        /// </summary>
        /// <param name="left">The left <see cref="EncodingType"/> to compare.</param>
        /// <param name="right">The right <see cref="EncodingType"/> to compare.</param>
        /// <returns>True if the <see cref="EncodingType"/>s are not equal, false otherwise.</returns>
        public static bool operator !=(EncodingType left, EncodingType right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Parses an Content-Encoding value into an <see cref="EncodingType"/> instance.
        /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
        /// </summary>
        /// <param name="value">The Content-Encoding value to parse.</param>
        /// <returns>The parsed <see cref="EncodingType"/>.</returns>
        public static EncodingType Parse(string value)
        {
            const string FormatExceptionMessage = "Invalid encoding format. Format must be: ( ( content-coding | \"*\" ) [ \";\" \"q\" \"=\" qvalue ] ). See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html";

            if (string.IsNullOrWhiteSpace(value))
            {
                value = "*";
            }

            Match match = EncodingType.ParseExpression.Match(value);

            if (match.Success)
            {
                string name = match.Groups[1].Value.Trim();

                EncodingType result = new EncodingType() 
                { 
                    Name = (!string.IsNullOrEmpty(name) ? name : "*").ToLowerInvariant(),
                    QValue = 1
                };

                if (match.Groups[3].Success)
                {
                    float q;

                    if (float.TryParse(match.Groups[3].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out q))
                    {
                        result.QValue = q > 1 ? 1 : (q < 0 ? 0 : q);
                    }
                    else
                    {
                        throw new FormatException(FormatExceptionMessage);
                    }
                }

                return result;
            }
            else
            {
                throw new FormatException(FormatExceptionMessage);
            }
        }

        /// <summary>
        /// Attempts to parse the given Content-Encoding value into an <see cref="EncodingType"/> instance.
        /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
        /// </summary>
        /// <param name="value">The Content-Encoding value to parse.</param>
        /// <param name="result">The parsed <see cref="EncodingType"/>.</param>
        /// <returns>True if the value was successfully parsed, otherwise false.</returns>
        public static bool TryParse(string value, out EncodingType result)
        {
            result = null;

            try
            {
                result = EncodingType.Parse(value);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance represents a superset of the given instance
        /// (i.e., their names are equal, or this instance represents the wildcard encoding).
        /// </summary>
        /// <param name="other">The <see cref="EncodingType"/> to compare with this instance.</param>
        /// <returns>True if this instance accepts the given instance, false otherwise.</returns>
        public bool Accepts(EncodingType other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other", "other cannot be null.");
            }

            return this.Name == "*" || this.Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(EncodingType other)
        {
            int result = 1;

            if (other != null)
            {
                result = this.QValue.CompareTo(other.QValue);

                if (result == 0)
                {
                    if (this.Name != "*" && other.Name == "*")
                    {
                        result = 1;
                    }
                    else if (this.Name == "*" && other.Name == "*")
                    {
                        result = -1;
                    }
                    else
                    {
                        result = this.ToString().CompareTo(other.ToString());
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
        public bool Equals(EncodingType other)
        {
            if ((object)other != null)
            {
                return this.Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase)
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
            return this.Equals(obj as EncodingType);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode()
                ^ this.QValue.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string result = this.Name;

            if (this.QValue < 1)
            {
                result += string.Format(CultureInfo.InvariantCulture, ";q={0:0.###}", this.QValue);
            }

            return result;
        }
    }
}