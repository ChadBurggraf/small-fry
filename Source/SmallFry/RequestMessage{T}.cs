//-----------------------------------------------------------------------------
// <copyright file="RequestMessage{T}.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal sealed class RequestMessage<T> : RequestMessage, IRequestMessage<T>
    {
        private bool disposed;

        public RequestMessage(string serviceName, IDictionary<string, object> routeValues, Uri requestUri)
            : base(serviceName, routeValues, requestUri)
        {
        }

        public RequestMessage(string serviceName, IDictionary<string, object> routeValues, Uri requestUri, T requestObject)
            : base(serviceName, routeValues, requestUri)
        {
            this.RequestObject = requestObject;
        }

        ~RequestMessage()
        {
            this.Dispose(false);
        }

        public T RequestObject { get; private set; }

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