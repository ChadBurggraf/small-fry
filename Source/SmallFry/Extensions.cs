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