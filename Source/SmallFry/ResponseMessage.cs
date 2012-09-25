//-----------------------------------------------------------------------------
// <copyright file="ResponseMessage.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;

    internal sealed class ResponseMessage : IResponseMessage
    {
        private bool disposed;

        public ResponseMessage()
        {
            this.Cookies = new HttpCookieCollection();
            this.Headers = new NameValueCollection();
            this.OutputStream = new MemoryStream();
            this.SetStatus(SmallFry.StatusCode.OK);
        }

        ~ResponseMessage()
        {
            this.Dispose(false);
        }

        public HttpCookieCollection Cookies { get; private set; }

        public NameValueCollection Headers { get; private set; }

        public object ResponseObject { get; set; }

        public int StatusCode { get; set; }

        public string StatusDescription { get; set; }

        internal Stream OutputStream { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetEncodingFilter(EncodingType encodingType, IEncoding encoding)
        {
            if (encodingType != null && encoding != null)
            {
                this.OutputStream = encoding.Encode(encodingType, this.OutputStream);
            }
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
                    if (this.OutputStream != null)
                    {
                        this.OutputStream.Dispose();
                        this.OutputStream = null;
                    }

                    this.ResponseObject.DisposeIfPossible();
                    this.ResponseObject = null;
                }

                this.disposed = true;
            }
        }
    }
}