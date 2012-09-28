//-----------------------------------------------------------------------------
// <copyright file="InternalExtensions.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;

    internal static class InternalExtensions
    {
        public static void AddDynamic(this IDictionary<string, object> dictionary, object values)
        {
            dictionary.AddDynamic<object>(values);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Mimics behavior of RouteValueDictionary.")]
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
            if (value.IsNullOrWhiteSpace())
            {
                value = "*";
            }

            return value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => EncodingType.Parse(s))
                .Distinct()
                .OrderByDescending(e => e)
                .ToArray();
        }

        public static IEnumerable<MediaType> AsMediaTypes(this string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                value = "*/*";
            }

            return value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => MediaType.Parse(s))
                .Distinct()
                .OrderByDescending(m => m)
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

#if NET35
        public static string Coalesce(this string value, string fallback)
        {
            return value.Coalesce(fallback, true);
        }

        public static string Coalesce(this string value, string fallback, bool includeWhitespace)
#else
        public static string Coalesce(this string value, string fallback, bool includeWhitespace = true)
#endif
        {
            string result;

            if (includeWhitespace)
            {
                result = value.IsNullOrWhiteSpace() ? fallback : value;
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

#if NET35
        public static void CopyTo(this Stream source, Stream destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "source cannot be null.");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination", "destination cannot be null.");
            }

            byte[] buffer = new byte[4096];
            int length = buffer.Length, count;

            while (0 < (count = source.Read(buffer, 0, length)))
            {
                destination.Write(buffer, 0, count);
            }
        }
#endif

        public static object Default(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type cannot be null.");
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static string Description(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "value cannot be null.");
            }

            string text = value.ToString();
            string result = text;
            MemberInfo info = value.GetType().GetMember(text).FirstOrDefault();

            if (info != null)
            {
                DescriptionAttribute da = info.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                if (da != null)
                {
                    result = da.Description;
                }
            }

            return result;
        }

        public static void DisposeIfPossible(this object obj)
        {
            IDisposable d = obj as IDisposable;

            if (d != null)
            {
                d.Dispose();
            }
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

        public static bool GreaterThanOperator<T>(T left, T right) where T : IComparable<T>
        {
            if (left != null && right == null)
            {
                return true;
            }
            else if (left == null && right != null)
            {
                return false;
            }
            else if (left == null && right == null)
            {
                return false;
            }
            else
            {
                return left.CompareTo(right) > 0;
            }
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
#if NET35
            return string.IsNullOrEmpty((value ?? string.Empty).Trim());
#else
            return string.IsNullOrWhiteSpace(value);
#endif
        }

        public static bool LessThanOperator<T>(T left, T right) where T : IComparable<T>
        {
            if (left != null && right == null)
            {
                return false;
            }
            else if (left == null && right != null)
            {
                return true;
            }
            else if (left == null && right == null)
            {
                return false;
            }
            else
            {
                return left.CompareTo(right) < 0;
            }
        }

        public static void SetStatus(this HttpResponseBase httpResponse, StatusCode statusCode)
        {
            if (httpResponse == null)
            {
                throw new ArgumentNullException("httpResponse", "httpResponse cannot be null.");
            }

            httpResponse.StatusCode = (int)statusCode;
            httpResponse.StatusDescription = statusCode.Description();
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