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

        public RoutePattern(string route)
        {
            route = RoutePattern.NormalizeRouteUrl(route);

            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentNullException("route", "route must contain a value other than / or ~/.");
            }

            this.Route = route;
            this.Segments = RoutePattern.Parse(this.Route);
            this.IsEmpty = !this.Segments.Any();
            this.IsAllLiteral = this.Segments.All(s => s.Tokens.All(t => t.TokenType == RouteTokenType.Literal));
            this.IsAllOptional = this.Segments.All(s => s.Tokens.All(t => t.IsOptional));
            this.IsAllWildcard = this.Segments.All(s => s.Tokens.All(t => t.TokenType == RouteTokenType.Wildcard));
        }

        public bool IsEmpty { get; private set; }

        public bool IsAllLiteral { get; private set; }

        public bool IsAllOptional { get; private set; }

        public bool IsAllWildcard { get; private set; }

        public string Route { get; private set; }

        public IEnumerable<RouteSegment> Segments { get; private set; }

        public bool TryMatch(string url, out IDictionary<string, object> routeValues)
        {
            bool matches = false;
            url = RoutePattern.NormalizeRouteUrl(url);
            Dictionary<string, object> d = new Dictionary<string, object>();
            
            if (string.IsNullOrEmpty(url))
            {
                if (this.IsEmpty || this.IsAllOptional || this.IsAllWildcard)
                {
                    RoutePattern.SeedRouteValues(this.Segments, d);
                    matches = true;
                }
            }
            else
            {
                RouteSegment[] urlSegments = null;

                try
                {
                    urlSegments = RoutePattern.Parse(url).ToArray();
                }
                catch (FormatException)
                {
                }

                if (urlSegments != null)
                {
                    int i = 0, urlLength = urlSegments.Length;

                    if (urlLength > 0)
                    {
                        RouteSegment[] patternSegments = this.Segments.ToArray();
                        RoutePattern.SeedRouteValues(patternSegments, d);
                        matches = true;

                        foreach (RouteSegment segment in patternSegments)
                        {
                            if (urlLength > i)
                            {
                                if (!RoutePattern.MatchSegment(segment, urlSegments[i], d))
                                {
                                    matches = false;
                                }
                            }
                            else
                            {
                                IEnumerable<RouteSegment> tail = patternSegments.Skip(i);

                                matches = tail.All(s => s.Tokens.All(t => t.TokenType == RouteTokenType.Wildcard))
                                    || tail.All(s => s.Tokens.All(t => t.IsOptional));

                                break;
                            }

                            i++;
                        }
                    }
                    else if (this.IsEmpty || this.IsAllOptional || this.IsAllWildcard)
                    {
                        RoutePattern.SeedRouteValues(this.Segments, d);
                        matches = true;
                    }
                }
            }

            routeValues = matches ? d : new Dictionary<string, object>();
            return matches;
        }

        private static bool MatchSegment(RouteSegment patternSegment, RouteSegment urlSegment, IDictionary<string, object> routeValues)
        {
            throw new NotImplementedException();
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

            if (value.Contains('?'))
            {
                value = value.Split('?').First().Trim();
            }

            return value;
        }

        private static IEnumerable<RouteSegment> Parse(string route)
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
            int length = part.Length, start = part.IndexOf('{'), end = part.IndexOf('}'), i = 0;

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

                        tokens.Add(token);
                        lookup[name] = true;
                        hasNamed = true;
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