//-----------------------------------------------------------------------------
// <copyright file="RouteValueBinder.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
                throw new ArgumentException("parser does not identify any parse-able types.", "parser"); 
            }

            foreach (Type type in types)
            {
                this.parserLookup[type] = parser;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "A binding failure indicates a non-matching route.")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IRouteParameterParser", Justification = "Reviewed.")]
        public IDictionary<string, object> Bind(IDictionary<string, object> routeValues, IDictionary<string, Type> types)
        {
            if (routeValues == null)
            {
                throw new ArgumentNullException("routeValues", "routeValues cannot be null.");
            }

            Dictionary<string, object> result = new Dictionary<string, object>(routeValues);
            bool success = true;

            if (types != null)
            {
                string name, value;
                Type type;

                foreach (KeyValuePair<string, Type> pair in types)
                {
                    name = pair.Key;
                    type = pair.Value;

                    if (result.ContainsKey(name) && type != null)
                    {
                        value = result[name] as string;

                        if (!string.IsNullOrEmpty(value))
                        {
                            if (this.parserLookup.ContainsKey(type))
                            {
                                try
                                {
                                    result[name] = this.parserLookup[type].Parse(type, name, value);
                                }
                                catch
                                {
                                    success = false;
                                    break;
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

            return success ? result : null;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used for testing.")]
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
