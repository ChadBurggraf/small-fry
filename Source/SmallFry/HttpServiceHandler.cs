//-----------------------------------------------------------------------------
// <copyright file="HttpServiceHandler.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
        /// <param name="context">An <see cref="HttpContext"/> object that provides references to the intrinsic server objects 
        /// (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            this.ProcessRequest(new HttpContextWrapper(context));
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="HttpContextBase"/> object that provides references to the intrinsic server objects 
        /// (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Overload of an interface method.")]
        public void ProcessRequest(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "context cannot be null.");
            }

            try
            {
                string url = context.Request.AppRelativeCurrentExecutionFilePath.Substring(1) + context.Request.PathInfo;
                MethodType methodType = context.Request.HttpMethod.AsMethodType();
                ResolvedService service = ServiceHost.Instance.ServiceResolver.Find(methodType, url);

                if (service != null)
                {
                    using (HttpRequestMessage request = HttpRequestMessage.Create(service.Name, service.RouteValues, context.Request, service.RequestType))
                    {
                        using (HttpResponseMessage response = new HttpResponseMessage(context.Response))
                        {
                            ReadRequestResult readResult = service.ReadRequest(
                                request,
                                context.Request.ContentLength,
                                context.Request.Headers["Content-Encoding"],
                                context.Request.ContentType,
                                context.Request.InputStream);

                            request.SetRequestObject(readResult.RequestObject);

                            List<Exception> exceptions = new List<Exception>();
                            InvokeActionsResult invokeResult;
                            bool success = readResult.Success, cont = true;

                            if (success)
                            {
                                invokeResult = service.InvokeBeforeActions(request, response);
                                success = invokeResult.Success;
                                cont = invokeResult.Continue;

                                if (!success)
                                {
                                    exceptions.AddRange(invokeResult.Results.Where(r => !r.Success && r.Exception != null).Select(r => r.Exception));
                                }

                                if (success && cont)
                                {
                                    MethodResult methodResult = service.Method.Invoke(request, response);
                                    success = methodResult.Success;

                                    if (!success && methodResult.Exception != null)
                                    {
                                        exceptions.Add(methodResult.Exception);
                                    }

                                    if (success)
                                    {
                                        invokeResult = service.InvokeAfterActions(request, response);
                                        success = invokeResult.Success;
                                        cont = invokeResult.Continue;

                                        if (!success)
                                        {
                                            exceptions.AddRange(invokeResult.Results.Where(r => !r.Success && r.Exception != null).Select(r => r.Exception));
                                        }
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

                            WriteResponseResult writeResult = service.WriteResponse(
                                response,
                                context.Request.Headers["Accept-Encoding"],
                                context.Request.Headers["Accept"]);

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
                else
                {
                    if (ServiceHost.Instance.ServiceResolver.ExistsForAnyMethodType(url))
                    {
                        context.Response.SetStatus(StatusCode.MethodNotAllowed);
                    }
                    else
                    {
                        context.Response.SetStatus(StatusCode.NotFound);
                    }
                }
            }
            finally
            {
                context.Response.End();
            }
        }
    }
}