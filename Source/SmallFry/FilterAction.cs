namespace SmallFry
{
    using System;

    internal class FilterAction
    {
        private Func<bool> simpleAction;
        private Func<Exception, bool> simpleExceptionAction;
        private Func<IRequestMessage, IResponseMessage, bool> requestResponseAction;
        private Func<Exception, IRequestMessage, IResponseMessage, bool> requestResponseExceptionAction;

        protected FilterAction()
        {
        }

        public FilterAction(Func<bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.simpleAction = action;
        }

        public FilterAction(Func<Exception, bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.simpleExceptionAction = action;
        }

        public FilterAction(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.requestResponseAction = action;
        }

        public FilterAction(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.requestResponseExceptionAction = action;
        }

        public FilterActionResult Invoke(IRequestMessage request, IResponseMessage response, Exception exception)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", "request cannot be null.");
            }

            if (response == null)
            {
                throw new ArgumentNullException("response", "response cannot be null.");
            }

            FilterActionResult result = new FilterActionResult();

            try
            {
                bool actionFound = false;

                if (this.requestResponseAction != null)
                {
                    result.Continue = this.requestResponseAction(request, response);
                    actionFound = true;
                }
                else if (this.simpleAction != null)
                {
                    result.Continue = this.simpleAction();
                    actionFound = true;
                }
                else
                {
                    if (exception == null)
                    {
                        throw new ArgumentNullException("exception", "exception cannot be null when invoking an error action.");
                    }

                    if (this.requestResponseExceptionAction != null)
                    {
                        result.Continue = this.requestResponseExceptionAction(exception, request, response);
                        actionFound = true;
                    }
                    else if (this.simpleExceptionAction != null)
                    {
                        result.Continue = this.simpleExceptionAction(exception);
                        actionFound = true;
                    }
                }

                if (!actionFound)
                {
                    throw new InvalidOperationException("No action was found to invoke.");
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }
    }
}
