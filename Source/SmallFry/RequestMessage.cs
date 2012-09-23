//-----------------------------------------------------------------------------
// <copyright file="RequestMessage.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web;

    internal class RequestMessage : IRequestMessage
    {
        private IDictionary<string, object> routeValues;
        private bool disposed;

        public RequestMessage(string serviceName, IDictionary<string, object> routeValues, Uri requestUri)
        {
            if (routeValues == null)
            {
                throw new ArgumentNullException("routeValues", "routeValues cannot be null.");
            }

            if (requestUri == null)
            {
                throw new ArgumentNullException("requestUri", "requestUri cannot be null.");
            }

            this.ServiceName = serviceName ?? string.Empty;
            this.routeValues = routeValues;
            this.RequestUri = requestUri;
            this.Cookies = new HttpCookieCollection();
            this.Headers = new NameValueCollection();
            this.Properties = new Dictionary<string, object>();
        }

        ~RequestMessage()
        {
            this.Dispose(false);
        }

        public HttpCookieCollection Cookies { get; private set; }

        public NameValueCollection Headers { get; private set; }

        public IDictionary<string, object> Properties { get; private set; }

        public Uri RequestUri { get; private set; }

        public string ServiceName { get; private set; }

        public static RequestMessage Create(string serviceName, IDictionary<string, object> routeValues, Uri requestUri, Type requestType, object requestObject)
        {
            if (requestType != null)
            {
                return (RequestMessage)Activator.CreateInstance(
                    typeof(RequestMessage<>).MakeGenericType(requestType),
                    serviceName,
                    routeValues,
                    requestUri,
                    requestObject);
            }
            else
            {
                return new RequestMessage(serviceName, routeValues, requestUri);
            }
        }

        public T RouteValue<T>(string name)
        {
            return (T)this.routeValues[name];
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    foreach (object value in this.Properties.Values)
                    {
                        value.DisposeIfPossible();
                    }

                    this.Properties.Clear();
                }

                this.disposed = true;
            }
        }
    }
}