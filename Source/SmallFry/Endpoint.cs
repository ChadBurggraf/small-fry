namespace SmallFry
{
    using System;

    internal sealed class Endpoint
    {
        public Endpoint(string route, Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service", "service cannot be null.");
            }

            this.Route = (route ?? string.Empty).Trim();
            this.Service = service;
        }

        public string Route { get; private set; }

        public Service Service { get; private set; }
    }
}