//-----------------------------------------------------------------------------
// <copyright file="HttpRequestMessage{T}.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    internal sealed class HttpRequestMessage<T> : HttpRequestMessage, IRequestMessage<T>
    {
        private bool disposed;

        public HttpRequestMessage(string serviceName, IDictionary<string, object> routeValues, HttpRequestBase httpRequest)
            : base(serviceName, routeValues, httpRequest)
        {
        }

        public HttpRequestMessage(string serviceName, IDictionary<string, object> routeValues, HttpRequestBase httpRequest, T requestObject)
            : base(serviceName, routeValues, httpRequest)
        {
            this.RequestObject = requestObject;
        }

        ~HttpRequestMessage()
        {
            this.Dispose(false);
        }

        public T RequestObject { get; internal set; }

        internal override void SetRequestObject(object requestObject)
        {
            this.RequestObject = (T)requestObject;
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.RequestObject.DisposeIfPossible();
                    this.RequestObject = default(T);
                }

                this.disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
