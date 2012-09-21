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
    using System.Web;

    internal class HttpRequestMessage : IRequestMessage
    {
        private HttpRequestBase httpRequest;
        private bool disposed;

        public HttpRequestMessage(string serviceName, HttpRequestBase httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException("httpRequest", "httpRequest cannot be null.");
            }

            this.ServiceName = serviceName ?? string.Empty;
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

        public static HttpRequestMessage Create(string serviceName, HttpRequestBase httpRequest, Type requestType, object requestObject)
        {
            if (requestType != null)
            {
                return (HttpRequestMessage)Activator.CreateInstance(
                    typeof(HttpRequestMessage<>).MakeGenericType(requestType), 
                    serviceName, 
                    httpRequest, 
                    requestObject);
            }
            else
            {
                return new HttpRequestMessage(serviceName, httpRequest);
            }
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
                        IDisposable disposableValue = value as IDisposable;

                        if (disposableValue != null)
                        {
                            disposableValue.Dispose();
                        }
                    }

                    this.Properties.Clear();
                }

                this.disposed = true;
            }
        }
    }
}