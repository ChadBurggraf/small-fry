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
        public static bool Accepts(this IEnumerable<EncodingType> encodingTypes, string contentEncoding)
        {
            bool result = true;

            if (encodingTypes != null
                && encodingTypes.Any()
                && !encodingTypes.All(e => e.Equals(EncodingType.Empty))
                && !string.IsNullOrEmpty(contentEncoding))
            {
                EncodingType contentEncodingType;

                if (EncodingType.TryParse(contentEncoding, out contentEncodingType))
                {
                    result = encodingTypes.Any(e => e.Accepts(contentEncodingType));
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool Accepts(this IEnumerable<MediaType> mediaTypes, string contentType)
        {
            bool result = true;

            if (mediaTypes != null 
                && mediaTypes.Any() 
                && !mediaTypes.All(m => m.Equals(MediaType.Empty)) 
                && !string.IsNullOrEmpty(contentType))
            {
                MediaType contentMediaType;

                if (MediaType.TryParse(contentType, out contentMediaType))
                {
                    result = mediaTypes.Any(m => m.Accepts(contentMediaType));
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