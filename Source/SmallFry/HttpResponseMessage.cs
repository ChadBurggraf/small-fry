//-----------------------------------------------------------------------------
// <copyright file="HttpResponseMessage.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
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

        public void SetEncodingFilter(EncodingType encodingType, IEncoding encoding)
        {
            if (encodingType != null && encoding != null)
            {
                string contentEncoding = encoding.ContentEncoding(encodingType);

                if (!"*".Equals(contentEncoding, StringComparison.Ordinal))
                {
                    this.Headers["Content-Encoding"] = contentEncoding;
                    this.httpResponse.Filter = encoding.Encode(encodingType, this.httpResponse.Filter);
                }
            }
        }

        public void SetStatus(StatusCode statusCode)
        {
            this.StatusCode = (int)statusCode;
            this.StatusDescription = statusCode.Description();
        }

        public void WriteOutputContent(MediaType mediaType, IFormat format)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType", "mediaType cannot be null.");
            }

            if (format == null)
            {
                throw new ArgumentNullException("format", "format cannot be null.");
            }

            string contentType = format.ContentType(mediaType);

            if (contentType.Contains("*"))
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Format {0} returned a Content-Type of {1} for the chosen media type of {2}. Writing a wildcard Content-Type to the response is not supported.",
                        format.GetType(),
                        contentType,
                        mediaType));
            }

            object resp = this.ResponseObject;
            this.httpResponse.ContentType = contentType;

            if (resp != null)
            {
                format.Serialize(mediaType, resp, this.httpResponse.OutputStream);
            }
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