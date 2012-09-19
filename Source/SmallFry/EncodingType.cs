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

    internal sealed class EncodingType : IEquatable<EncodingType>, IComparable<EncodingType>
    {
        private static readonly Regex ParseExpression = new Regex(@"^\s*([^;]+)(\s*;\s*q\s*=\s*(\d(\.\d*)?))?\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly EncodingType EmptyType = EncodingType.Parse(null);
        
        private EncodingType()
        {
        }

        public static EncodingType Empty
        {
            get { return EncodingType.EmptyType; }
        }

        public string Name { get; private set; }

        public float QValue { get; private set; }

        public static bool operator ==(EncodingType left, EncodingType right)
        {
            return Extensions.EqualsOperator(left, right);
        }

        public static bool operator !=(EncodingType left, EncodingType right)
        {
            return !(left == right);
        }

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

        public bool Accepts(EncodingType other)
        {
            return other == null
                || other.Equals(EncodingType.Empty)
                || this.Equals(EncodingType.Empty)
                || this.Name == "*"
                || other.Name == "*"
                || this.Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }

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

        public bool Equals(EncodingType other)
        {
            if ((object)other != null)
            {
                return this.Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase)
                    && this.QValue.EqualsFloat(other.QValue, .001f);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as EncodingType);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode()
                ^ this.QValue.GetHashCode();
        }

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