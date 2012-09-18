namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    internal sealed class RouteValueBinder
    {
        private Dictionary<Type, IRouteParameterParser> parserLookup;

        public RouteValueBinder()
        {
            this.parserLookup = new Dictionary<Type, IRouteParameterParser>();
            RouteValueBinder.AddDefaultParsers(this.parserLookup);
        }

        public void AddParser(IRouteParameterParser parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException("parser", "parser cannot be null.");
            }

            Type[] types = (parser.CanParseTypes ?? new Type[0]).ToArray();

            if (types.Length == 0)
            {
                throw new ArgumentException("parser", "parser does not identify any parse-able types."); 
            }

            foreach (Type type in types)
            {
                this.parserLookup[type] = parser;
            }
        }

        public void Bind(IDictionary<string, object> routeValues, IDictionary<string, Type> types)
        {
            if (routeValues == null)
            {
                throw new ArgumentNullException("routeValues", "routeValues cannot be null.");
            }

            if (types != null)
            {
                foreach (KeyValuePair<string, Type> pair in types)
                {
                    if (routeValues.ContainsKey(pair.Key) && pair.Value != null)
                    {
                        string routeValue = routeValues[pair.Key] as string;

                        if (!string.IsNullOrEmpty(routeValue))
                        {
                            if (this.parserLookup.ContainsKey(pair.Value))
                            {
                                try
                                {
                                    //routeValues[pair.Key] = this.Parsers[pair.Value].
                                }
                                catch (Exception ex)
                                {
                                    throw new InvalidOperationException(
                                        string.Format(
                                            CultureInfo.InvariantCulture, 
                                            "Failed to parse route parameter \"{0}\" ({1}) with parser {3} into {4}.", 
                                            pair.Key, 
                                            routeValues[pair.Key],
                                            this.parserLookup[pair.Value], 
                                            pair.Value), 
                                        ex);
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException(
                                    string.Format(
                                        CultureInfo.InvariantCulture, 
                                        "An IRouteParameterParser could not be found for type {0}, which is needed for route parameter \"{1}\".", 
                                        pair.Value, 
                                        pair.Key));
                            }
                        }
                    }
                }
            }
        }

        public bool HasParserForType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type cannot be null.");
            }

            return this.parserLookup.ContainsKey(type);
        }

        private static void AddDefaultParsers(IDictionary<Type, IRouteParameterParser> parserLookup)
        {
            IRouteParameterParser parser = new PrimitiveRouteParameterParser();

            foreach (Type type in parser.CanParseTypes)
            {
                parserLookup[type] = parser;
            }
        }
    }
}
