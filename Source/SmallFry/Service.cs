//-----------------------------------------------------------------------------
// <copyright file="Service.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal sealed class Service
    {
        public Service(string name, string baseUrl, IServiceCollection serviceCollection)
        {
            if (name.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("name", "name must contain a value.");
            }

            if (serviceCollection == null)
            {
                throw new ArgumentNullException("serviceCollection", "serviceCollection cannot be null.");
            }

            this.Name = name;
            this.BaseUrl = baseUrl.Coalesce("/");
            this.ServiceCollection = serviceCollection;
            this.Endpoints = new EndpointCollection(this);
            this.Pipeline = new Pipeline();
        }

        public string BaseUrl { get; private set; }

        public IEndpointCollection Endpoints { get; private set; }

        public string Name { get; private set; }

        public Pipeline Pipeline { get; private set; }

        public IServiceCollection ServiceCollection { get; private set; }
    }
}