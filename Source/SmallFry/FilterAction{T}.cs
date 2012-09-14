namespace SmallFry
{
    using System;

    internal sealed class FilterAction<T> : FilterAction
    {
        private Func<IRequestMessage<T>, IResponseMessage, bool> action;
        private Func<Exception, IRequestMessage<T>, IResponseMessage, bool> exceptionAction;

        public FilterAction(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.action = action;
        }

        public FilterAction(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.exceptionAction = action;
        }

        public FilterActionResult Invoke(IRequestMessage<T> request, IResponseMessage response, Exception exception)
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
                if (this.action != null)
                {
                    result.Continue = this.action(request, response);
                }
                else if (this.exceptionAction != null)
                {
                    if (exception == null)
                    {
                        throw new ArgumentNullException("exception", "exception cannot be null when invoking an error action.");
                    }

                    result.Continue = this.exceptionAction(exception, request, response);
                }
                else
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