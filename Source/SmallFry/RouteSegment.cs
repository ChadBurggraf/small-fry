//-----------------------------------------------------------------------------
// <copyright file="RouteSegment.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class RouteSegment
    {
        public RouteSegment(IEnumerable<RouteToken> tokens, bool hasWildcard)
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
            this.HasWildcard = hasWildcard;
        }

        public bool HasWildcard { get; private set; }

        public IEnumerable<RouteToken> Tokens { get; private set; }

        public override string ToString()
        {
            return string.Concat(this.Tokens.Select(t => t.ToString()));
        }
    }
}