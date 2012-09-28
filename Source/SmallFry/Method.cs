//-----------------------------------------------------------------------------
// <copyright file="Method.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal class Method
    {
        private static readonly Type[] Types = new Type[0];
        private Action action;
        private Action<IRequestMessage> requestAction;
        private Action<IRequestMessage, IResponseMessage> requestResponseAction;

        public Method(MethodType methodType, Endpoint endpoint, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.Initialize(methodType, endpoint);
            this.action = action;
        }

        public Method(MethodType methodType, Endpoint endpoint, Action<IRequestMessage> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.Initialize(methodType, endpoint);
            this.requestAction = action;
        }

        public Method(MethodType methodType, Endpoint endpoint, Action<IRequestMessage, IResponseMessage> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.Initialize(methodType, endpoint);
            this.requestResponseAction = action;
        }

        protected Method()
        {
        }

        public Endpoint Endpoint { get; protected set; }

        public MethodType MethodType { get; protected set; }

        public Pipeline Pipeline { get; protected set; }

        public virtual IEnumerable<Type> TypeArguments
        {
            get { return Method.Types; }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Pipeline execution.")]
        public virtual MethodResult Invoke(IRequestMessage request, IResponseMessage response)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", "request cannot be null.");
            }

            if (response == null)
            {
                throw new ArgumentNullException("response", "response cannot be null.");
            }

            MethodResult result = new MethodResult();

            if (this.action != null)
            {
                try
                {
                    this.action();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                }
            }
            else if (this.requestAction != null)
            {
                try
                {
                    this.requestAction(request);
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                }
            }
            else if (this.requestResponseAction != null)
            {
                try
                {
                    this.requestResponseAction(request, response);
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                }
            }
            else
            {
                throw new InvalidOperationException("No action was found to invoke.");
            }

            return result;
        }

        protected void Initialize(MethodType methodType, Endpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint", "endpoint cannot be null.");
            }

            this.MethodType = methodType;
            this.Endpoint = endpoint;
            this.Pipeline = new Pipeline();
        }
    }
}
