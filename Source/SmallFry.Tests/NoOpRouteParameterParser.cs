namespace SmallFry.Tests
{
    using System;
    using System.Collections.Generic;

    public sealed class NoOpRouteParameterParser : IRouteParameterParser
    {
        public NoOpRouteParameterParser(IEnumerable<Type> canParseTypes)
        {
            this.CanParseTypes = canParseTypes;
        }

        public IEnumerable<Type> CanParseTypes { get; private set; }

        public object Parse(Type type, string name, string value)
        {
            throw new NotImplementedException();
        }
    }
}