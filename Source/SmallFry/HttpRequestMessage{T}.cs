//-----------------------------------------------------------------------------
// <copyright file="HttpRequestMessage{T}.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Web;

    internal sealed class HttpRequestMessage<T> : HttpRequestMessage, IRequestMessage<T>
    {
        private bool disposed;

        public HttpRequestMessage(string serviceName, HttpRequestBase httpRequest)
            : base(serviceName, httpRequest)
        {
        }

        public HttpRequestMessage(string serviceName, HttpRequestBase httpRequest, T requestObject)
            : base(serviceName, httpRequest)
        {
            this.RequestObject = requestObject;
        }

        ~HttpRequestMessage()
        {
            this.Dispose(false);
        }

        public T RequestObject { get; internal set; }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    IDisposable d = this.RequestObject as IDisposable;

                    if (d != null)
                    {
                        d.Dispose();
                    }

                    this.RequestObject = default(T);
                }

                this.disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
