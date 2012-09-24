﻿//-----------------------------------------------------------------------------
// <copyright file="HttpResponseMessage.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Specialized;
    using System.Web;

    internal sealed class HttpResponseMessage : IResponseMessage
    {
        private HttpResponseBase httpResponse;
        private bool disposed;

        public HttpResponseMessage(HttpResponseBase httpResponse)
        {
            if (httpResponse == null)
            {
                throw new ArgumentNullException("httpResponse", "httpResponse cannot be null.");
            }

            this.httpResponse = httpResponse;
        }

        ~HttpResponseMessage()
        {
            this.Dispose(false);
        }

        public HttpCookieCollection Cookies
        {
            get { return this.httpResponse.Cookies; }
        }

        public NameValueCollection Headers
        {
            get { return this.httpResponse.Headers; }
        }

        public object ResponseObject { get; set; }

        public int StatusCode
        {
            get { return this.httpResponse.StatusCode; }
            set { this.httpResponse.StatusCode = value; }
        }

        public string StatusDescription
        {
            get { return this.httpResponse.StatusDescription; }
            set { this.httpResponse.StatusDescription = value; }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetStatus(StatusCode statusCode)
        {
            this.StatusCode = (int)statusCode;
            this.StatusDescription = statusCode.Description();
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.ResponseObject.DisposeIfPossible();
                    this.ResponseObject = null;
                }

                this.disposed = true;
            }
        }
    }
}