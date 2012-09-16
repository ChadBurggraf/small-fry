//-----------------------------------------------------------------------------
// <copyright file="Endpoint.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

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

            this.Route = (route ?? string.Empty).Trim();
            this.Service = service;
            this.EndpointCollection = endpointCollection;
            this.MethodCollection = new MethodCollection(this);
            this.Pipeline = new Pipeline();
        }

        public IEndpointCollection EndpointCollection { get; private set; }

        public IMethodCollection MethodCollection { get; private set; }

        public Pipeline Pipeline { get; private set; }

        public string Route { get; private set; }

        public Service Service { get; private set; }
    }
}