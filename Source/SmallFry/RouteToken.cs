//-----------------------------------------------------------------------------
// <copyright file="RouteToken.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    internal sealed class RouteToken
    {
        public RouteToken(RouteTokenType tokenType, string value, bool isOptional)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value", "value must contain a value.");
            }

            if (tokenType != RouteTokenType.Named && isOptional)
            {
                throw new ArgumentException("tokenType must be Named when isOptional is true.", "tokenType");
            }

            this.TokenType = tokenType;
            this.Value = value;
            this.IsOptional = isOptional;
        }

        public bool IsOptional { get; private set; }

        public RouteTokenType TokenType { get; private set; }

        public string Value { get; private set; }

        public override string ToString()
        {
            bool brackets = this.TokenType == RouteTokenType.Named || this.TokenType == RouteTokenType.Wildcard;
            string result = string.Empty;
            
            if (brackets)
            {
                result += "{";

                if (this.IsOptional)
                {
                    result += "?";
                }
                else if (this.TokenType == RouteTokenType.Wildcard)
                {
                    result += "*";
                }
            }

            result += this.Value;

            if (brackets)
            {
                result += "}";
            }

            return result;
        }
    }
}