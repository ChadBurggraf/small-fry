//-----------------------------------------------------------------------------
// <copyright file="Endpoint.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal sealed class Endpoint
    {
        public Endpoint(string route, Service service, IEndpointCollection endpointCollection)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service", "service cannot be null.");
            }

            if (endpointCollection == null)
            {
                throw new ArgumentNullException("endpointCollection", "endpointCollection cannot be null.");
            }

            this.Route = RoutePattern.Parse(service.BaseUrl.AppendUrlPath(route));
            this.EndpointCollection = endpointCollection;
            this.Methods = new MethodCollection(this);
            this.ParameterPatterns = new Dictionary<string, Regex>();
            this.ParameterTypes = new Dictionary<string, Type>();
            this.Pipeline = new Pipeline();
        }

        public IEndpointCollection EndpointCollection { get; private set; }

        public IMethodCollection Methods { get; private set; }

        public IDictionary<string, Regex> ParameterPatterns { get; private set; }

        public IDictionary<string, Type> ParameterTypes { get; private set; }

        public Pipeline Pipeline { get; private set; }

        public RoutePattern Route { get; private set; }

        public void SetParameterPatterns(object patternConstraints)
        {
            this.ParameterPatterns.Clear();

            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.AddDynamic(patternConstraints);

            foreach (KeyValuePair<string, object> pair in dict) 
            {
                Regex regex = pair.Value as Regex;

                if (regex != null)
                {
                    this.ParameterPatterns[pair.Key] = regex;
                }
                else
                {
                    string value = pair.Value as string;

                    if (!string.IsNullOrEmpty(value))
                    {
                        this.ParameterPatterns[pair.Key] = new Regex(value, RegexOptions.Compiled);
                    }
                }
            }
        }

        public void SetParameterTypes(object typeConstraints)
        {
            this.ParameterTypes.Clear();
            this.ParameterTypes.AddDynamic(typeConstraints);
        }
    }
}