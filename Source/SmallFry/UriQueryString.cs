//-----------------------------------------------------------------------------
// <copyright file="UriQueryString.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Provides extension methods for working with <see cref="Uri"/> query strings.
    /// </summary>
    public static class UriQueryString
    {
        private static readonly PrimitiveRouteParameterParser Parser = new PrimitiveRouteParameterParser();

        /// <summary>
        /// Gets the value of the query string parameter with the given key.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> to get the query string value from.</param>
        /// <param name="key">The key to get the value of.</param>
        /// <returns>The query string value for the given key.</returns>
        public static string GetQueryValue(this Uri uri, string key)
        {
            return uri.QueryString()[key];
        }

        /// <summary>
        /// Gets the typed value of the query string parameter with the given key.
        /// </summary>
        /// <typeparam name="T">The type to conver the value into.</typeparam>
        /// <param name="uri">The <see cref="Uri"/> to get the query string value from.</param>
        /// <param name="key">The key to get the value of.</param>
        /// <returns>The query string value for the given key.</returns>
        public static T GetQueryValue<T>(this Uri uri, string key)
        {
            string value = uri.GetQueryValue(key);

            if (!string.IsNullOrEmpty(value))
            {
                Type type = typeof(T);

                if (UriQueryString.Parser.CanParseTypes.Any(t => t.IsAssignableFrom(type)))
                {
                    return (T)UriQueryString.Parser.Parse(type, null, value);
                }
                else
                {
                    throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture, "There is no conversion defined for {0}.", type));
                }
            }

            return default(T);
        }

        /// <summary>
        /// Parses the query string of a <see cref="Uri"/> into a <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> to parse the query string from.</param>
        /// <returns>A query string represented as a <see cref="NameValueCollection"/>.</returns>
        public static NameValueCollection QueryString(this Uri uri)
        {
            NameValueCollection result = new NameValueCollection();
            string query = uri != null ? uri.Query : string.Empty;

            if (!string.IsNullOrEmpty(query))
            {
                if (query.StartsWith("?", StringComparison.Ordinal))
                {
                    query = query.Substring(1);
                }

                foreach (string part in query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] pair = part.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                    if (!string.IsNullOrEmpty(pair[0]))
                    {
                        string key = HttpUtility.UrlDecode(pair[0]);

                        if (pair.Length > 1)
                        {
                            foreach (string value in HttpUtility.UrlDecode(pair[1]).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                result.Add(key, value);
                            }
                        }
                        else
                        {
                            result.Add(key, string.Empty);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Tries to get the typed query string value with the given key.
        /// </summary>
        /// <typeparam name="T">The type to conver the value into.</typeparam>
        /// <param name="uri">The <see cref="Uri"/> to get the query string value from.</param>
        /// <param name="key">The key to get the value of.</param>
        /// <param name="result">The typed query string value for the given key.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public static bool TryGetQueryValue<T>(this Uri uri, string key, out T result)
        {
            bool success = false;
            result = default(T);

            try
            {
                result = uri.GetQueryValue<T>(key);
                success = true;
            }
            catch (InvalidCastException)
            {
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            catch (ArgumentException)
            {
            }

            return success;
        }
    }
}