//-----------------------------------------------------------------------------
// <copyright file="FilterAction{T}.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal sealed class FilterAction<T> : FilterAction, IEquatable<FilterAction<T>>
    {
        private Func<IRequestMessage<T>, IResponseMessage, bool> action;
        private Func<IEnumerable<Exception>, IRequestMessage<T>, IResponseMessage, bool> exceptionAction;
        private IEnumerable<Type> typeArguments = new Type[] { typeof(T) };

        public FilterAction(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.action = action;
        }

        public FilterAction(Func<IEnumerable<Exception>, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "action cannot be null.");
            }

            this.exceptionAction = action;
        }

        public override IEnumerable<Type> TypeArguments
        {
            get { return this.typeArguments; }
        }

        public static bool operator ==(FilterAction<T> left, FilterAction<T> right)
        {
            return InternalExtensions.EqualsOperator(left, right);
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

        public override FilterActionResult Invoke(IRequestMessage request, IResponseMessage response, IEnumerable<Exception> exceptions)
        {
            return this.Invoke(request as IRequestMessage<T>, response, exceptions);
        }

        public FilterActionResult Invoke(IRequestMessage<T> request, IResponseMessage response, IEnumerable<Exception> exceptions)
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

            if (this.action != null)
            {
                try
                {
                    result.Continue = this.action(request, response);
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                }
            }
            else if (this.exceptionAction != null)
            {
                try
                {
                    result.Continue = this.exceptionAction(exceptions, request, response);
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