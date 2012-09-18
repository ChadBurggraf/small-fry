namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class RouteSegment
    {
        public RouteSegment(IEnumerable<RouteToken> tokens)
            : this(tokens, tokens.Any(t => t.TokenType == RouteTokenType.Named), tokens.Any(t => t.TokenType == RouteTokenType.Wildcard))
        {
        }

        public RouteSegment(IEnumerable<RouteToken> tokens, bool hasNamed, bool hasWildcard)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens", "tokens cannot be null.");
            }

            if (!tokens.Any())
            {
                throw new ArgumentException("tokens must contain at least one value.", "tokens");
            }

            this.Tokens = tokens;
            this.HasNamed = hasNamed;
            this.HasWildcard = hasWildcard;
        }

        public bool HasNamed { get; private set; }

        public bool HasWildcard { get; private set; }

        public IEnumerable<RouteToken> Tokens { get; private set; }

        public override string ToString()
        {
            return string.Concat(this.Tokens.Select(t => t.ToString()));
        }
    }
}