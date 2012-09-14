namespace SmallFry
{
    using System;
    using System.Web;

    public sealed class HttpServiceHost : IHttpHandler, IServiceHost
    {
        private IServiceCollection services = new ServiceCollection();

        public bool IsReusable
        {
            get { return true; }
        }

        public IServiceCollection Services
        {
            get { return this.services; }
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            this.ProcessRequest(new HttpContextWrapper(httpContext));
        }

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