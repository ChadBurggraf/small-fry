﻿namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal sealed class FilterAction<T> : FilterAction, IEquatable<FilterAction<T>>
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

        public static bool operator ==(FilterAction<T> left, FilterAction<T> right)
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

        public static bool operator !=(FilterAction<T> left, FilterAction<T> right)
        {
            return !(left == right);
        }

        public override bool Equals(FilterAction other)
        {
            return this.Equals(other as FilterAction<T>);
        }

        public bool Equals(FilterAction<T> other)
        {
            if ((object)other != null)
            {
                if (this.action != null)
                {
                    return this.action == other.action;
                }
                else if (this.exceptionAction != null)
                {
                    return this.exceptionAction == other.exceptionAction;
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
                return this.Equals(obj as FilterAction<T>);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.action != null)
            {
                return this.action.GetHashCode();
            }
            else if (this.exceptionAction != null)
            {
                return this.exceptionAction.GetHashCode();
            }
            else
            {
                throw new InvalidOperationException("No action is defined.");
            }
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