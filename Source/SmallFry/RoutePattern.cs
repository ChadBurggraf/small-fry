//-----------------------------------------------------------------------------
// <copyright file="RoutePattern.cs" company="Tasty Codes">
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

    internal sealed class RoutePattern
    {
        private const string WildcardFormatMessage = "A wildcard parameter must be the last segment of a route.";

        private RoutePattern()
        {
        }

        public bool IsEmpty { get; private set; }

        public bool IsAllLiteral { get; private set; }

        public bool IsAllOptional { get; private set; }

        public bool IsAllWildcard { get; private set; }

        public string Route { get; private set; }

        public IEnumerable<RouteSegment> Segments { get; private set; }

        public static RoutePattern Parse(string route)
        {
            route = RoutePattern.NormalizeRouteUrl(route);

            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentNullException("route", "route must contain a value other than / or ~/.");
            }

            IEnumerable<RouteSegment> segments = RoutePattern.ParseAll(route);

            return new RoutePattern()
            {
                Route = route,
                Segments = segments,
                IsEmpty = !segments.Any(),
                IsAllLiteral = segments.All(s => s.Tokens.All(t => t.TokenType == RouteTokenType.Literal)),
                IsAllOptional = segments.All(s => s.Tokens.All(t => t.IsOptional)),
                IsAllWildcard = segments.All(s => s.Tokens.All(t => t.TokenType == RouteTokenType.Wildcard))
            };
        }

        public static bool TryParse(string route, out RoutePattern result)
        {
            bool success = false;
            result = null;

            try
            {
                result = RoutePattern.Parse(route);
                success = true;
            }
            catch (FormatException)
            {
            }

            return success;
        }

        public IDictionary<string, object> Match(string url)
        {
            url = RoutePattern.NormalizeRouteUrl(url);

            bool match = true;
            Dictionary<string, object> routeValues = new Dictionary<string, object>();
            RouteSegment[] segments = this.Segments.ToArray();

            if (url.Contains('?'))
            {
                url = url.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
            }

            if (!string.IsNullOrEmpty(url))
            {
                RoutePattern.SeedRouteValues(segments, routeValues);

                string[] urlSegments = url.Split('/');
                int segmentCount = segments.Length, urlSegmentCount = urlSegments.Length;

                // If the URL is longer than the route, the route must either be empty or have a
                // wildcard at the end.
                if (urlSegmentCount <= segmentCount || (segmentCount > 0 && segments[segmentCount - 1].HasWildcard))
                {
                    RouteSegment segment;
                    string urlSegment;

                    for (int i = 0; i < segmentCount; i++)
                    {
                        segment = segments[i];

                        if (urlSegmentCount > i)
                        {
                            urlSegment = urlSegments[i];
                            match = RoutePattern.MatchSegment(urlSegments[i], segment, routeValues);

                            if (match)
                            {
                                // If the matched segment has a wildcard, it means we're going to be at the end
                                // of the route. Add the remainder of the URL to the wildcard route value.
                                if (segment.HasWildcard)
                                {
                                    RouteToken wildcardToken = segment.Tokens.First(t => t.TokenType == RouteTokenType.Wildcard);
                                    string value = routeValues[wildcardToken.Value] as string ?? string.Empty;

                                    if (urlSegmentCount > i + 1)
                                    {
                                        if (!value.EndsWith("/", StringComparison.Ordinal))
                                        {
                                            value += "/";
                                        }

                                        value += string.Join("/", urlSegments.Skip(i + 1).ToArray());
                                        routeValues[wildcardToken.Value] = value;
                                    }

                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            // If we are at the end of the URL, it only matches if we've reached a
                            // wildcard or if all of the remaining tokens in the pattern are optional.
                            match = segment.HasWildcard
                                || segments.Skip(i).All(s => s.Tokens.All(t => t.IsOptional));

                            break;
                        }
                    }
                }
                else
                {
                    match = false;
                }
            }
            else
            {
                // If the URL is empty, we match if the pattern is also empty,
                // the pattern is just a wildcard, or the pattern is all optional.
                match = segments.Length == 0
                    || this.IsAllOptional
                    || this.IsAllWildcard;

                if (match)
                {
                    RoutePattern.SeedRouteValues(segments, routeValues);
                }
            }

            return match ? routeValues : null;
        }

        public override string ToString()
        {
            return this.Route;
        }

        private static bool MatchSegment(string urlSegment, RouteSegment segment, IDictionary<string, object> routeValues)
        {
            bool match = true;
            RouteToken[] tokens = segment.Tokens.ToArray();
            int start = 0, position = 0, tokenCount = tokens.Length, urlSegmentLength = urlSegment.Length;
            RouteToken previousToken = null;

            for (int i = 0; i < tokenCount; i++)
            {
                RouteToken token = tokens[i];

                // Literals must be matched.
                if (token.TokenType == RouteTokenType.Literal)
                {
                    // Find the first occurrance of the literal after our current position.
                    start = urlSegment.IndexOf(token.Value, position, StringComparison.OrdinalIgnoreCase);

                    if (start > -1)
                    {
                        // If the literal exists and we have a previous token, 
                        // then that token is named. If the token is not optional,
                        // it must exist and start must be beyound our current position.
                        if (i > 0)
                        {
                            if (start > position)
                            {
                                routeValues[previousToken.Value] = urlSegment.Substring(position, start - position);
                            }
                            else if (!previousToken.IsOptional)
                            {
                                match = false;
                                break;
                            }
                        }

                        // Move the position to the end of the current token.
                        position = start + token.Value.Length;
                    }
                    else
                    {
                        match = false;
                        break;
                    }
                }
                else if (token.TokenType == RouteTokenType.Named
                    || token.TokenType == RouteTokenType.Wildcard)
                {
                    // If we're on a named or wildcard token and it is the last token,
                    // add the remainder of the URL segment to the token's route value.
                    if (i == tokenCount - 1)
                    {
                        bool hasValue = urlSegmentLength > position;

                        if (hasValue
                            || token.TokenType == RouteTokenType.Wildcard
                            || token.IsOptional)
                        {
                            if (hasValue)
                            {
                                routeValues[token.Value] = urlSegment.Substring(position);
                            }
                        }
                        else
                        {
                            match = false;
                        }

                        break;
                    }
                }

                previousToken = token;
            }

            return match;
        }

        private static string NormalizeRouteUrl(string value)
        {
            value = (value ?? string.Empty).Trim();

            if (value.StartsWith("/", StringComparison.Ordinal))
            {
                value = value.Substring(1);
            }

            if (value.StartsWith("~/", StringComparison.Ordinal))
            {
                value = value.Substring(2);
            }

            if (value.EndsWith("/", StringComparison.Ordinal))
            {
                value = value.Substring(0, value.Length - 1);
            }

            return value;
        }

        private static IEnumerable<RouteSegment> ParseAll(string route)
        {
            List<RouteSegment> segments = new List<RouteSegment>();
            Dictionary<string, bool> lookup = new Dictionary<string, bool>();
            bool hasWildcard = false;

            foreach (string part in route.Split('/'))
            {
                RouteSegment segment = RoutePattern.ParseSegment(part, lookup);

                if (hasWildcard && segment.HasWildcard)
                {
                    throw new FormatException(RoutePattern.WildcardFormatMessage);
                }

                segments.Add(segment);
            }

            return segments;
        }

        private static RouteSegment ParseSegment(string part, Dictionary<string, bool> lookup)
        {
            List<RouteToken> tokens = new List<RouteToken>();
            bool hasWildcard = false, hasNamed = false, optional, wildcard;
            int length = part.Length, start = part.IndexOf('{'), end = part.IndexOf('}'), i = 0, tokenCount = 0;

            if (start > -1)
            {
                if (end < 0)
                {
                    throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The route segment \"{0}\" contains an un-matched opening brace.", part));
                }

                while (i < length)
                {
                    if (hasWildcard)
                    {
                        throw new FormatException(RoutePattern.WildcardFormatMessage);
                    }
                    
                    // Is there a literal before the beginning of the first parameter?
                    if (start - i > 0)
                    {
                        tokens.Add(new RouteToken(RouteTokenType.Literal, part.Substring(i, start - i), false));
                        tokenCount = tokenCount + 1;
                        i = start;
                    }

                    // Move to the character after the opening bracket and check for modifiers.
                    i = i + 1;
                    optional = part[i] == '?';
                    wildcard = !optional && part[i] == '*';
                    hasWildcard = hasWildcard || wildcard;

                    if (optional || wildcard)
                    {
                        i = i + 1;
                    }

                    // Make sure the closing bracket isn't the character after the opening bracket.
                    if (end - i > 0)
                    {
                        RouteToken token = new RouteToken(wildcard ? RouteTokenType.Wildcard : RouteTokenType.Named, part.Substring(i, end - i), optional);
                        string name = token.Value.ToUpperInvariant();

                        if (lookup.ContainsKey(name))
                        {
                            throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The route segment \"{0}\" contains a named parameter \"{1}\" that exists more than once in the route.", part, token.Value));
                        }

                        if (tokenCount > 0)
                        {
                            RouteToken previousToken = tokens[tokenCount - 1];

                            if (previousToken.TokenType != RouteTokenType.Literal)
                            {
                                throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The route segment \"{0}\" contains two named parameters in a row without a literal separator.", part));
                            }
                        }

                        tokens.Add(token);
                        lookup[name] = true;
                        hasNamed = true;
                        tokenCount = tokenCount + 1;
                        i = end + 1;
                    }
                    else
                    {
                        throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The route segment \"{0}\" contains an empty parameter.", part));
                    }

                    start = part.IndexOf('{', i);
                    end = part.IndexOf('}', i);
                }
            }
            else
            {
                if (end < 0)
                {
                    tokens.Add(new RouteToken(RouteTokenType.Literal, part, false));
                    tokenCount = tokenCount + 1;
                }
                else
                {
                    throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The route segment \"{0}\" contains an un-matched closing brace.", part));
                }
            }

            return new RouteSegment(tokens, hasNamed, hasWildcard);
        }

        private static void SeedRouteValues(IEnumerable<RouteSegment> segments, IDictionary<string, object> routeValues)
        {
            foreach (RouteToken token in segments.SelectMany(s => s.Tokens))
            {
                if (token.TokenType != RouteTokenType.Literal)
                {
                    routeValues[token.Value] = null;
                }
            }
        }
    }
}