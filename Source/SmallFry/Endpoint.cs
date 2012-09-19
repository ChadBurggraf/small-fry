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
            this.Service = service;
            this.EndpointCollection = endpointCollection;
            this.Methods = new MethodCollection(this);
            this.ParameterTypes = new Dictionary<string, Type>();
            this.Pipeline = new Pipeline();
        }

        public IEndpointCollection EndpointCollection { get; private set; }

        public IMethodCollection Methods { get; private set; }

        public IDictionary<string, Type> ParameterTypes { get; private set; }

        public Pipeline Pipeline { get; private set; }

        public RoutePattern Route { get; private set; }

        public Service Service { get; private set; }
    }
}