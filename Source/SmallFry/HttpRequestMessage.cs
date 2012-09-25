//-----------------------------------------------------------------------------
// <copyright file="HttpRequestMessage.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;

    internal class HttpRequestMessage : IRequestMessage
    {
        private HttpRequestBase httpRequest;
        private IDictionary<string, object> routeValues;
        private bool disposed;

        public HttpRequestMessage(string serviceName, IDictionary<string, object> routeValues, HttpRequestBase httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException("httpRequest", "httpRequest cannot be null.");
            }

            if (routeValues == null)
            {
                throw new ArgumentNullException("routeValues", "routeValues cannot be null.");
            }

            this.ServiceName = serviceName ?? string.Empty;
            this.routeValues = routeValues;
            this.httpRequest = httpRequest;
            this.Properties = new Dictionary<string, object>();
        }

        ~HttpRequestMessage()
        {
            this.Dispose(false);
        }

        public HttpCookieCollection Cookies
        {
            get { return this.httpRequest.Cookies; }
        }

        public NameValueCollection Headers
        {
            get { return this.httpRequest.Headers; }
        }

        public IDictionary<string, object> Properties { get; private set; }

        public Uri RequestUri
        {
            get { return this.httpRequest.Url; }
        }

        public string ServiceName { get; private set; }

        public static HttpRequestMessage Create(string serviceName, IDictionary<string, object> routeValues, HttpRequestBase httpRequest, Type requestType)
        {
            if (requestType != null)
            {
                return (HttpRequestMessage)Activator.CreateInstance(
                    typeof(HttpRequestMessage<>).MakeGenericType(requestType), 
                    serviceName, 
                    routeValues,
                    httpRequest);
            }
            else
            {
                return new HttpRequestMessage(serviceName, routeValues, httpRequest);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public T HeaderValue<T>(string name)
        {
            return this.Headers.Get<T>(name);
        }

        public string MapPath(string path)
        {
            return this.httpRequest.MapPath(path);
        }

        public T QueryValue<T>(string name)
        {
            return this.RequestUri.GetQueryValue<T>(name);
        }

        public T RouteValue<T>(string name)
        {
            return (T)this.routeValues[name];
        }

        public void SetEncodingFilter(EncodingType encodingType, IEncoding encoding)
        {
            if (encodingType != null && encoding != null)
            {
                this.httpRequest.Filter = encoding.Decode(encodingType, this.httpRequest.Filter);
            }
        }

        internal virtual void SetRequestObject(object requestObject)
        {
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