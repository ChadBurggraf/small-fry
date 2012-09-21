//-----------------------------------------------------------------------------
// <copyright file="HttpServiceHandler.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
                object requestObject = null;
                ICollection<FilterActionResult> actionResults = null;
                List<Exception> exceptions = new List<Exception>();
                bool success = true, cont = true;

                try
                {
                    // Read the incoming request.
                    if ((methodType == MethodType.Post
                        || methodType == MethodType.Put)
                        && service.RequestType != null
                        && httpContext.Request.ContentLength > 0)
                    {
                        Exception ex;

                        if (!service.TryReadRequestObject(
                            httpContext.Request.Headers["Content-Encoding"], 
                            httpContext.Request.ContentType, 
                            httpContext.Request.InputStream, 
                            out requestObject,
                            out ex))
                        {
                            if (ex != null)
                            {
                                exceptions.Add(ex);
                            }

                            success = false;
                        }
                    }

                    // Instantiate the request and response objects.
                    using (IRequestMessage request = HttpRequestMessage.Create(service.Name, httpContext.Request, service.RequestType, requestObject))
                    {
                        requestObject = null;

                        using (IResponseMessage response = new HttpResponseMessage(httpContext.Response))
                        {
                            // If we failed to read the request, don't even bother with the main pipeline.
                            if (success)
                            {
                                // Invoke the before actions.
                                actionResults = service.InvokeBeforeActions(request, response);
                                exceptions.AddRange(actionResults.Where(r => !r.Success && r.Exception != null).Select(r => r.Exception));
                                success = success && actionResults.Any(r => !r.Success);
                                cont = cont && !actionResults.Any(r => !r.Continue);

                                // If we didn't bail out.
                                if (cont)
                                {
                                    // Invoke the endpoint method.
                                    MethodResult result = service.Method.Invoke(request, response);
                                    success = success && result.Success;

                                    if (!result.Success && result.Exception != null)
                                    {
                                        exceptions.Add(result.Exception);
                                    }

                                    // Invoke the after actions.
                                    actionResults = service.InvokeAfterActions(request, response);
                                    exceptions.AddRange(actionResults.Where(r => !r.Success && r.Exception != null).Select(r => r.Exception));
                                    success = success && actionResults.Any(r => !r.Success);
                                    cont = cont && !actionResults.Any(r => !r.Continue);
                                }
                            }

                            // If we hit an error and didn't bail out, invoke the error actions.
                            if (!success && cont)
                            {
                                actionResults = service.InvokeErrors(request, response, exceptions);
                            }

                            // Write the outgoing response.
                            if (response.ResponseObject != null)
                            {
                                Exception ex;

                                if (!service.TryWriteResponseObject(
                                    httpContext.Request.Headers["Accept-Encoding"],
                                    string.Join(",", httpContext.Request.AcceptTypes),
                                    response.ResponseObject,
                                    httpContext.Response.OutputStream,
                                    out ex))
                                {
                                    if (ex != null)
                                    {
                                        throw ex;
                                    }

                                    success = false;
                                }
                            }
                        } 
                    }

                    // If there was an error, but no actions updated the response
                    // status code, just send a 500.
                    if (!success && httpContext.Response.StatusCode < 400)
                    {
                        httpContext.Response.StatusCode = 500;
                        httpContext.Response.StatusDescription = "Internal Server Error";
                    }
                }
                finally
                {
                    // Ensure the request object is disposed, if it is disposable
                    // and something went wront.
                    IDisposable d = requestObject as IDisposable;

                    if (d != null)
                    {
                        d.Dispose();
                    }
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