namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal class FilterAction : IEquatable<FilterAction>
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

        public static bool operator ==(FilterAction left, FilterAction right)
        {
            if (Object.ReferenceEquals(left, right))
            {
                return true;
            }

            object l = (object)left;
            object r = (object)right;

            if (l != null && r == null)
            {
                return false;
            }
            else if (l == null && r != null)
            {
                return false;
            }
            else if (l == null && r == null)
            {
                return true;
            }
            else
            {
                return left.Equals(right);
            }
        }

        public static bool operator !=(FilterAction left, FilterAction right)
        {
            return !(left == right);
        }

        public virtual bool Equals(FilterAction other)
        {
            if ((object)other != null)
            {
                if (this.requestResponseAction != null)
                {
                    return this.requestResponseAction == other.requestResponseAction;
                }
                else if (this.simpleAction != null)
                {
                    return this.simpleAction == other.simpleAction;
                }
                else if (this.requestResponseExceptionAction != null)
                {
                    return this.requestResponseExceptionAction == other.requestResponseExceptionAction;
                }
                else if (this.simpleExceptionAction != null)
                {
                    return this.simpleExceptionAction == other.simpleExceptionAction;
                }
                else
                {
                    throw new InvalidOperationException("No action was found to compare.");
                }
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                return this.Equals(obj as FilterAction);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.requestResponseAction != null)
            {
                return this.requestResponseAction.GetHashCode();
            }
            else if (this.simpleAction != null)
            {
                return this.simpleAction.GetHashCode();
            }
            else if (this.requestResponseExceptionAction != null)
            {
                return this.requestResponseExceptionAction.GetHashCode();
            }
            else if (this.simpleExceptionAction != null)
            {
                return this.simpleExceptionAction.GetHashCode();
            }
            else
            {
                throw new InvalidOperationException("No action is defined.");
            }
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
