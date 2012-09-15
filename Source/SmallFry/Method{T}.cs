namespace SmallFry
{
    using System;

    internal sealed class Method<T> : Method
    {
        private Action<T> action;
        private Action<IRequestMessage<T>> requestAction;
        private Action<IRequestMessage<T>, IResponseMessage> requestResponseAction;

        public Method(MethodType methodType, Endpoint endpoint, IMethodCollection methodCollection, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.Initialize(methodType, endpoint, methodCollection);
            this.action = action;
        }

        public Method(MethodType methodType, Endpoint endpoint, IMethodCollection methodCollection, Action<IRequestMessage<T>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.Initialize(methodType, endpoint, methodCollection);
            this.requestAction = action;
        }

        public Method(MethodType methodType, Endpoint endpoint, IMethodCollection methodCollection, Action<IRequestMessage<T>, IResponseMessage> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.Initialize(methodType, endpoint, methodCollection);
            this.requestResponseAction = action;
        }

        public MethodResult Invoke(IRequestMessage<T> request, IResponseMessage response)
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
                    this.action(request.RequestObject);
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
    }
}