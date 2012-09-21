//-----------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Extensions
    {
        public static bool Accept(this IEnumerable<EncodingType> encodingTypes, EncodingType contentEncoding)
        {
            return encodingTypes == null
                || !encodingTypes.Any()
                || encodingTypes.Any(e => e.Accepts(contentEncoding));
        }

        public static bool Accept(this IEnumerable<EncodingType> encodingTypes, string contentEncoding)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(contentEncoding))
            {
                EncodingType contentEncodingType;

                if (EncodingType.TryParse(contentEncoding, out contentEncodingType))
                {
                    result = encodingTypes.Accept(contentEncoding);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool Accept(this IEnumerable<MediaType> mediaTypes, MediaType contentType)
        {
            return mediaTypes == null
                || !mediaTypes.Any()
                || mediaTypes.Any(m => m.Accepts(contentType));
        }

        public static bool Accept(this IEnumerable<MediaType> mediaTypes, string contentType)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(contentType))
            {
                MediaType contentMediaType;

                if (MediaType.TryParse(contentType, out contentMediaType))
                {
                    result = mediaTypes.Accept(contentMediaType);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public static void AddDynamic(this IDictionary<string, object> dictionary, object values)
        {
            dictionary.AddDynamic<object>(values);
        }

        public static void AddDynamic<T>(this IDictionary<string, T> dictionary, object values)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary", "dictionary cannot be null.");
            }

            if (values != null)
            {
                Type type = typeof(T);

                foreach (var p in values.GetType().GetProperties())
                {
                    try
                    {
                        object value = p.GetValue(values, null);

                        if (value == null)
                        {
                            value = type.Default();
                        }

                        if (value == null || type.IsAssignableFrom(value.GetType()))
                        {
                            dictionary.Add(p.Name, (T)value);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static string AppendUrlPath(this string url, string path)
        {
            url = (url ?? string.Empty).Trim();
            path = (path ?? string.Empty).Trim();

            string result = url;

            if (!string.IsNullOrEmpty(path))
            {
                while (url.EndsWith("/", StringComparison.Ordinal))
                {
                    url = url.Substring(0, url.Length - 1);
                }

                while (path.StartsWith("/", StringComparison.Ordinal))
                {
                    path = path.Substring(1);
                }

                bool urlEmpty = string.IsNullOrEmpty(url);
                bool pathEmpty = string.IsNullOrEmpty(path);

                if (!urlEmpty && !pathEmpty)
                {
                    result = url + "/" + path;
                }
                else if (!urlEmpty)
                {
                    result = url;
                }
                else if (!pathEmpty)
                {
                    result = path;
                }
                else
                {
                    result = string.Empty;
                }
            }

            return result;
        }

        public static IEnumerable<EncodingType> AsEncodingTypes(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                value = "*";
            }

            return value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => EncodingType.Parse(s))
                .Distinct()
                .OrderByDescending(e => e.QValue)
                .ThenBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        public static IEnumerable<MediaType> AsMediaTypes(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                value = "*/*";
            }

            return value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => MediaType.Parse(s))
                .Distinct()
                .OrderByDescending(m => m.AcceptParams.Value)
                .ThenBy(m => m.RootType)
                .ThenBy(m => m.SubType)
                .ToArray();
        }

        public static MethodType AsMethodType(this string value)
        {
            switch ((value ?? string.Empty).ToUpperInvariant())
            {
                case "DELETE":
                    return MethodType.Delete;
                case "POST":
                    return MethodType.Post;
                case "PUT":
                    return MethodType.Put;
                default:
                    return MethodType.Get;
            }
        }

        public static string Coalesce(this string value, string fallback, bool includeWhitespace = true)
        {
            string result;

            if (includeWhitespace)
            {
                result = string.IsNullOrWhiteSpace(value) ? fallback : value;
            }
            else
            {
                result = value ?? fallback;
            }

            return result;
        }

        public static bool CollectionEquals<T>(this ICollection<T> value, ICollection<T> compareValue, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer", "comparer cannot be null.");
            }

            bool result;

            if (value != null && compareValue == null)
            {
                result = false;
            }
            else if (value == null && compareValue != null)
            {
                result = false;
            }
            else if (value == null && compareValue == null)
            {
                result = true;
            }
            else if (value.Count != compareValue.Count)
            {
                result = false;
            }
            else
            {
                result = value.OrderBy(v => v, comparer).SequenceEqual(compareValue.OrderBy(v => v, comparer));
            }

            return result;
        }

        public static object Default(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type cannot be null.");
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool EqualsFloat(this float left, float right, float margin)
        {
            float l = Math.Abs(left);
            float r = Math.Abs(right);
            float diff = Math.Abs(l - r);

            if (l * r == 0)
            {
                return diff < (margin * margin);
            }
            else
            {
                return diff / (l + r) < margin;
            }
        }

        public static bool EqualsOperator<T>(T left, T right) where T : IEquatable<T>
        {
            if (object.ReferenceEquals(left, right))
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

        public static IEnumerable<string> ToAcceptEncodings(this IEnumerable<EncodingType> encodingTypes)
        {
            List<string> result = new List<string>();

            if (encodingTypes != null)
            {
                result.AddRange(encodingTypes.Select(e => e.Name));
            }

            if (result.Count == 0)
            {
                result.Add("*");
            }

            return result;
        }

        public static IEnumerable<string> ToAcceptFormats(this IEnumerable<MediaType> mediaTypes)
        {
            List<string> result = new List<string>();

            if (mediaTypes != null)
            {
                result.AddRange(mediaTypes.Select(m => m.RootType + "/" + m.SubType));
            }

            if (result.Count == 0)
            {
                result.Add("*/*");
            }

            return result;
        }
    }
}