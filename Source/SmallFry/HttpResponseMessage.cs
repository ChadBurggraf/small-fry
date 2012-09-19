//-----------------------------------------------------------------------------
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

        public HttpResponseMessage(HttpResponseBase httpResponse)
        {
            if (httpResponse == null)
            {
                throw new ArgumentNullException("httpResponse", "httpResponse cannot be null.");
            }

            this.httpResponse = httpResponse;
        }

        public HttpCookieCollection Cookies
        {
            get { return this.httpResponse.Cookies; }
        }

        public NameValueCollection Headers
        {
            get { return this.httpResponse.Headers; }
        }

        public object Response { get; set; }

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
    }
}