//-----------------------------------------------------------------------------
// <copyright file="FilterAction.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal class FilterAction : IEquatable<FilterAction>
    {
        private static readonly Type[] Types = new Type[0];
        private Func<bool> simpleAction;
        private Func<IEnumerable<Exception>, bool> simpleExceptionAction;
        private Func<IRequestMessage, IResponseMessage, bool> requestResponseAction;
        private Func<IEnumerable<Exception>, IRequestMessage, IResponseMessage, bool> requestResponseExceptionAction;

        public FilterAction(Func<bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.simpleAction = action;
        }

        public FilterAction(Func<IEnumerable<Exception>, bool> action)
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

        public FilterAction(Func<IEnumerable<Exception>, IRequestMessage, IResponseMessage, bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.requestResponseExceptionAction = action;
        }

        protected FilterAction()
        {
        }

        public virtual IEnumerable<Type> TypeArguments
        {
            get { return FilterAction.Types; }
        }

        public static bool operator ==(FilterAction left, FilterAction right)
        {
            return InternalExtensions.EqualsOperator(left, right);
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

        public virtual FilterActionResult Invoke(IRequestMessage request, IResponseMessage response, IEnumerable<Exception> exceptions)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", "request cannot be null.");
            }

            if (response == null)
            {
                throw new ArgumentNullException("response", "response cannot be null.");
            }

            FilterActionResult result = new FilterActionResult() { Continue = true };

            bool actionFound = false;

            if (this.requestResponseAction != null)
            {
                try
                {
                    result.Continue = this.requestResponseAction(request, response);
                    result.Success = true;
                    actionFound = true;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                }
            }
            else if (this.simpleAction != null)
            {
                try
                {
                    result.Continue = this.simpleAction();
                    result.Success = true;
                    actionFound = true;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                }
            }
            else
            {
                if (this.requestResponseExceptionAction != null)
                {
                    try
                    {
                        result.Continue = this.requestResponseExceptionAction(exceptions, request, response);
                        result.Success = true;
                        actionFound = true;
                    }
                    catch (Exception ex)
                    {
                        result.Exception = ex;
                    }
                }
                else if (this.simpleExceptionAction != null)
                {
                    try
                    {
                        result.Continue = this.simpleExceptionAction(exceptions);
                        result.Success = true;
                        actionFound = true;
                    }
                    catch (Exception ex)
                    {
                        result.Exception = ex;
                    }
                }
            }

            if (!actionFound)
            {
                throw new InvalidOperationException("No action was found to invoke.");
            }

            return result;
        }
    }
}
