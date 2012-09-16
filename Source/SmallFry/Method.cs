﻿namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal class Method
    {
        private Action action;
        private Action<IRequestMessage> requestAction;
        private Action<IRequestMessage, IResponseMessage> requestResponseAction;

        protected Method()
        {
        }

        public Method(MethodType methodType, Endpoint endpoint, IMethodCollection methodCollection, Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.Initialize(methodType, endpoint, methodCollection);
            this.action = action;
        }

        public Method(MethodType methodType, Endpoint endpoint, IMethodCollection methodCollection, Action<IRequestMessage> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.Initialize(methodType, endpoint, methodCollection);
            this.requestAction = action;
        }

        public Method(MethodType methodType, Endpoint endpoint, IMethodCollection methodCollection, Action<IRequestMessage, IResponseMessage> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.Initialize(methodType, endpoint, methodCollection);
            this.requestResponseAction = action;
        }

        public Endpoint Endpoint { get; protected set; }

        public IMethodCollection MethodCollection { get; protected set; }

        public MethodType MethodType { get; protected set; }

        public Pipeline Pipeline { get; protected set; }

        public MethodResult Invoke(IRequestMessage request, IResponseMessage response)
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

        protected void Initialize(MethodType methodType, Endpoint endpoint, IMethodCollection methodCollection)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint", "endpoint cannot be null.");
            }

            if (methodCollection == null)
            {
                throw new ArgumentNullException("methodCollection", "methodCollection cannot be null.");
            }

            this.MethodType = methodType;
            this.Endpoint = endpoint;
            this.MethodCollection = methodCollection;
            this.Pipeline = new Pipeline();
        }
    }
}