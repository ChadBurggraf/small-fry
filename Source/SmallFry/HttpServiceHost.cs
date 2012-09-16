//-----------------------------------------------------------------------------
// <copyright file="HttpServiceHost.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Web;

    /// <summary>
    /// Implements <see cref="IServiceHost"/> as an <see cref="IHttpHandler"/>
    /// for hosting HTTP services.
    /// </summary>
    public sealed class HttpServiceHost : IHttpHandler, IServiceHost
    {
        private IServiceCollection services = new ServiceCollection();

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the collection of services hosted by this host.
        /// </summary>
        public IServiceCollection Services
        {
            get { return this.services; }
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

            /*
             *  1. Get handler-relative URL
             *  2. Find service + endpoint + method that matches handler-relative URL
             *  3. Invoke global encoding(s)
             *  4. Invoke global format(s)
             *  5. Invoke preconditions
             *  6. Invoke endpoint method
             *  7. Invoke postconditions
             *  8. Invoke global format(s)
             *  9. Invoke global encoding(s)
             *  10. Write response
             */

            // Find service endpoint + method
            //
            // If found
            //     
            // Else if services error handler
            //
            // Else
            //     Respond with 404 Not Found
        }
    }
}