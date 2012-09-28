//-----------------------------------------------------------------------------
// <copyright file="MediaTypeAcceptParameters.cs" company="Tasty Codes">
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
    /// Represents an Accept-Params field of a Content-Type.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public sealed class MediaTypeAcceptParameters : IEquatable<MediaTypeAcceptParameters>
    {
        private static readonly Regex ParseExpression = new Regex(@"^q\s*=\s*(\d(\.\d+)?)(.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly MediaTypeAcceptParameters EmptyParameters = MediaTypeAcceptParameters.Parse(null);

        private MediaTypeAcceptParameters()
        {
        }

        /// <summary>
        /// Gets the empty <see cref="MediaTypeAcceptParameters"/> instance.
        /// </summary>
        public static MediaTypeAcceptParameters Empty
        {
            get { return MediaTypeAcceptParameters.EmptyParameters; }
        }

        /// <summary>
        /// Gets the parameters' <see cref="MediaTypeExtension"/> collection.
        /// </summary>
        public IEnumerable<MediaTypeExtension> Extensions { get; private set; }

        /// <summary>
        /// Gets the parameters' q-value.
        /// </summary>
        public float QValue { get; private set; }

        /// <summary>
        /// Gets a value indicating whether two <see cref="MediaTypeAcceptParameters"/>s are equal.
        /// </summary>
        /// <param name="left">The left <see cref="MediaTypeAcceptParameters"/> to compare.</param>
        /// <param name="right">The right <see cref="MediaTypeAcceptParameters"/> to compare.</param>
        /// <returns>True if the <see cref="MediaTypeAcceptParameters"/>s are equal, false otherwise.</returns>
        public static bool operator ==(MediaTypeAcceptParameters left, MediaTypeAcceptParameters right)
        {
            return SmallFry.InternalExtensions.EqualsOperator(left, right);
        }

        /// <summary>
        /// Gets a value indicating whether two <see cref="MediaTypeAcceptParameters"/>s are not equal.
        /// </summary>
        /// <param name="left">The left <see cref="MediaTypeAcceptParameters"/> to compare.</param>
        /// <param name="right">The right <see cref="MediaTypeAcceptParameters"/> to compare.</param>
        /// <returns>True if the <see cref="MediaTypeAcceptParameters"/>s are not equal, false otherwise.</returns>
        public static bool operator !=(MediaTypeAcceptParameters left, MediaTypeAcceptParameters right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Parses an Accept-Params value into an <see cref="MediaTypeAcceptParameters"/> instance.
        /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
        /// </summary>
        /// <param name="value">The Accept-Params value to parse.</param>
        /// <returns>The parsed <see cref="MediaTypeAcceptParameters"/>.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "params", Justification = "Reviewed.")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qvalue", Justification = "Reviewed.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "False positive.")]
        public static MediaTypeAcceptParameters Parse(string value)
        {
            const string FormatExceptionMessage = "Invalid params format. Format must be: \"q\" \"=\" qvalue *( accept-extension ). See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html";

            if (!value.IsNullOrWhiteSpace())
            {
                Match match = MediaTypeAcceptParameters.ParseExpression.Match(value.Trim());

                if (match.Success)
                {
                    float floatValue;

                    if (float.TryParse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out floatValue))
                    {
                        MediaTypeAcceptParameters result = new MediaTypeAcceptParameters()
                        {
                            QValue = floatValue > 1 ? 1 : (floatValue < 0 ? 0 : floatValue)
                        };

                        if (match.Groups[3].Success && !string.IsNullOrEmpty(match.Groups[3].Value))
                        {
                            result.Extensions = match.Groups[3].Value.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => MediaTypeExtension.Parse(s))
                                .ToArray();
                        }
                        else
                        {
                            result.Extensions = new MediaTypeExtension[0];
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
                return new MediaTypeAcceptParameters() { Extensions = new MediaTypeExtension[0], QValue = 1 };
            }
        }

        /// <summary>
        /// Attempts to parse the given Accept-Params value into an <see cref="MediaTypeAcceptParameters"/> instance.
        /// See http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html for formatting information.
        /// </summary>
        /// <param name="value">The Accept-Params value to parse.</param>
        /// <param name="result">The parsed <see cref="MediaTypeAcceptParameters"/>.</param>
        /// <returns>True if the value was successfully parsed, otherwise false.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        public static bool TryParse(string value, out MediaTypeAcceptParameters result)
        {
            result = null;

            try
            {
                result = MediaTypeAcceptParameters.Parse(value);
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
        public bool Equals(MediaTypeAcceptParameters other)
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
            return this.Equals(obj as MediaTypeAcceptParameters);
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

            if (this != MediaTypeAcceptParameters.Empty)
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
}
