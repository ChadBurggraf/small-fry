//-----------------------------------------------------------------------------
// <copyright file="RequestMessage{T}.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    internal sealed class RequestMessage<T> : RequestMessage, IRequestMessage<T>
    {
        private bool disposed;

        public RequestMessage(string serviceName, Uri requestUri)
            : base(serviceName, requestUri)
        {
        }

        public RequestMessage(string serviceName, Uri requestUri, T requestObject)
            : base(serviceName, requestUri)
        {
            this.RequestObject = requestObject;
        }

        ~RequestMessage()
        {
            this.Dispose(false);
        }

        public T RequestObject { get; private set; }

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