namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal sealed class Service
    {
        public Service(string name, string baseUrl, IServiceCollection serviceCollection)
        {
            if (string.IsNullOrWhiteSpace(name))
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
            this.AfterActions = new List<FilterAction>();
            this.BeforeActions = new List<FilterAction>();
            this.Encodings = new List<EncodingFilter>();
            this.Endpoints = new EndpointCollection(this);
            this.Formats = new List<FormatFilter>();
        }

        public IList<FilterAction> AfterActions { get; private set; }

        public IList<FilterAction> BeforeActions { get; private set; }

        public string BaseUrl { get; private set; }

        public IList<EncodingFilter> Encodings { get; private set; }

        public IEndpointCollection Endpoints { get; private set; }

        public IList<FilterAction> ErrorActions { get; private set; }

        public IList<FormatFilter> Formats { get; private set; }

        public string Name { get; private set; }

        public IServiceCollection ServiceCollection { get; private set; }
    }
}