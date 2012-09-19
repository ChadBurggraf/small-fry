//-----------------------------------------------------------------------------
// <copyright file="HttpServiceHandler.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Web;

    /// <summary>
    /// Handles service I/O over HTTP.
    /// </summary>
    public sealed class HttpServiceHandler : IHttpHandler
    {
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="IHttpHandler"/> interface.
        /// </summary>
        /// <param name="httpContext">An <see cref="HttpContext"/> object that provides references to the intrinsic server objects 
        /// (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext httpContext)
        {
            this.ProcessRequest(new HttpContextWrapper(httpContext));
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="IHttpHandler"/> interface.
        /// </summary>
        /// <param name="httpContext">An <see cref="HttpContextBase"/> object that provides references to the intrinsic server objects 
        /// (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext", "httpContext cannot be null.");
            }

            string url = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(1);
            MethodType methodType = httpContext.Request.HttpMethod.AsMethodType();
            ResolvedService service = ServiceHost.Instance.ServiceResolver.Find(methodType, url);

            if (service != null)
            {
                if ((methodType == MethodType.Post
                    || methodType == MethodType.Put)
                    && httpContext.Request.ContentLength > 0)
                {
                    IEncoding requestEncoding = ServiceHost.GetEncoding(service, httpContext.Request.Headers["Content-Encoding"]) ?? new IdentityEncoding();
                    IFormat requestFormat = ServiceHost.GetFormat(service, httpContext.Request.ContentType) ?? new PlainTextFormat();

                    throw new NotImplementedException();
                }
            }
            else
            {
                httpContext.Response.StatusCode = 404;
                httpContext.Response.StatusDescription = "Not Found";
            }

            httpContext.Response.End();
        }
    }
}