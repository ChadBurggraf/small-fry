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

    internal sealed class MediaType : IEquatable<MediaType>
    {
        private static readonly Regex AcceptParamsStartExpression = new Regex(@"^\s*q\s*=.*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex ParseExpression = new Regex(@"^(\*/\*|[a-z0-9]+/\*|[a-z0-9]+/[a-z0-9]+)(.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly MediaType EmptyType = MediaType.Parse(null);
        
        private MediaType()
        {
        }

        public static MediaType Empty
        {
            get { return MediaType.EmptyType; }
        }

        public AcceptParameters AcceptParams { get; private set; }

        public IEnumerable<string> RangeParams { get; private set; }

        public string RootType { get; private set; }

        public string SubType { get; private set; }

        public static bool operator ==(MediaType left, MediaType right)
        {
            return Extensions.EqualsOperator(left, right);
        }

        public static bool operator !=(MediaType left, MediaType right)
        {
            return !(left == right);
        }

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

        public bool Accepts(MediaType other)
        {
            return other == null
                || other.Equals(MediaType.Empty)
                || ((this.RootType.Equals(other.RootType, StringComparison.OrdinalIgnoreCase)
                || this.RootType == "*"
                || other.RootType == "*")
                && (this.SubType.Equals(other.SubType, StringComparison.OrdinalIgnoreCase)
                || this.SubType == "*"
                || other.SubType == "*"));
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