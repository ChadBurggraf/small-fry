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
    using System.Web.SessionState;

    /// <summary>
    /// Handles service I/O over HTTP.
    /// </summary>
    public sealed class HttpServiceHandler : IHttpHandler, IRequiresSessionState
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

            try
            {
                string url = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(1) + httpContext.Request.PathInfo;
                MethodType methodType = httpContext.Request.HttpMethod.AsMethodType();
                ResolvedService service = ServiceHost.Instance.ServiceResolver.Find(methodType, url);

                if (service != null)
                {
                    using (HttpRequestMessage request = HttpRequestMessage.Create(service.Name, service.RouteValues, httpContext.Request, service.RequestType))
                    {
                        using (HttpResponseMessage response = new HttpResponseMessage(httpContext.Response))
                        {
                            ReadRequestResult readResult = service.ReadRequest(
                                request,
                                httpContext.Request.ContentLength,
                                httpContext.Request.Headers["Content-Encoding"],
                                httpContext.Request.ContentType,
                                httpContext.Request.InputStream);

                            request.SetRequestObject(readResult.RequestObject);

                            List<Exception> exceptions = new List<Exception>();
                            InvokeActionsResult invokeResult;
                            bool success = readResult.Success, cont = true;

                            if (success)
                            {
                                invokeResult = service.InvokeBeforeActions(request, response);
                                success = success && invokeResult.Success;
                                cont = cont && invokeResult.Continue;

                                if (!invokeResult.Success)
                                {
                                    exceptions.AddRange(invokeResult.Results.Where(r => !r.Success && r.Exception != null).Select(r => r.Exception));
                                }

                                if (cont)
                                {
                                    MethodResult methodResult = service.Method.Invoke(request, response);
                                    success = success && methodResult.Success;

                                    if (!methodResult.Success && methodResult.Exception != null)
                                    {
                                        exceptions.Add(methodResult.Exception);
                                    }

                                    invokeResult = service.InvokeAfterActions(request, response);
                                    success = success && invokeResult.Success;
                                    cont = cont && invokeResult.Continue;

                                    if (!invokeResult.Success)
                                    {
                                        exceptions.AddRange(invokeResult.Results.Where(r => !r.Success && r.Exception != null).Select(r => r.Exception));
                                    }
                                }
                            }
                            else
                            {
                                if (readResult.Exception != null)
                                {
                                    exceptions.Add(readResult.Exception);
                                }

                                if (readResult.StatusCode != StatusCode.None)
                                {
                                    response.SetStatus(readResult.StatusCode);
                                }
                            }

                            if (!success)
                            {
                                invokeResult = service.InvokeErrorActions(request, response, exceptions);

                                if (!invokeResult.Success)
                                {
                                    throw new PipelineException(
                                        PipelineErrorType.ErrorHandlerThrewException,
                                        service.Name,
                                        service.Method.Endpoint.Route.ToString(),
                                        request.RequestUri,
                                        methodType,
                                        invokeResult.Results.Where(r => !r.Success && r.Exception != null).Select(r => r.Exception).FirstOrDefault());
                                }
                            }

                            if (!success && response.StatusCode < 300)
                            {
                                response.SetStatus(StatusCode.InternalServerError);
                            }

                            if (cont)
                            {
                                WriteResponseResult writeResult = service.WriteResponse(
                                    response,
                                    httpContext.Request.Headers["Accept-Encoding"],
                                    httpContext.Request.Headers["Accept"]);

                                if (!writeResult.Success)
                                {
                                    if (writeResult.StatusCode != StatusCode.None)
                                    {
                                        response.SetStatus(writeResult.StatusCode);
                                    }

                                    if (writeResult.Exception != null)
                                    {
                                        throw new PipelineException(
                                            PipelineErrorType.WriteResponseOutputThrewException,
                                            service.Name,
                                            service.Method.Endpoint.Route.ToString(),
                                            request.RequestUri,
                                            methodType,
                                            writeResult.Exception);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ServiceHost.Instance.ServiceResolver.ExistsForAnyMethodType(url))
                    {
                        httpContext.Response.SetStatus(StatusCode.MethodNotAllowed);
                    }
                    else
                    {
                        httpContext.Response.SetStatus(StatusCode.NotFound);
                    }
                }
            }
            finally
            {
                httpContext.Response.End();
            }
        }
    }
}