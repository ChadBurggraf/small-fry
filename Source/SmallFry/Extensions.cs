namespace SmallFry
{
    using System;

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
    }
}