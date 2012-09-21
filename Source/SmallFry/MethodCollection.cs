//-----------------------------------------------------------------------------
// <copyright file="MethodCollection.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class MethodCollection : ICollection<Method>, IEnumerable<Method>, IEnumerable, IMethodCollection
    {
        private const string CurrentMethodNotSetMessage = "Please add a method by calling one of Delete(), Get(), Post(), or Put() before attempting to call this method.";
        private List<Method> list = new List<Method>();

        public MethodCollection(Endpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint", "endpoint cannot be null.");
            }

            this.Endpoint = endpoint;
        }

        public int Count
        {
            get { return this.list.Count; }
        }

        public Method CurrentMethod { get; private set; }

        public Endpoint Endpoint { get; private set; }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(Method item)
        {
            this.list.Add(item);
        }

        public IMethodCollection AfterEndpoint(Func<bool> action)
        {
            return this.Endpoint.EndpointCollection.AfterEndpoint(action);
        }

        public IMethodCollection AfterEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.AfterEndpoint(action);
        }

        public IMethodCollection AfterEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.AfterEndpoint(action);
        }

        public IMethodCollection AfterMethod(Func<bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.AfterActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection AfterMethod(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.AfterActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection AfterMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.AfterActions.Add(new FilterAction<T>(action));
            return this;
        }

        public IEndpointCollection AfterService(Func<bool> action)
        {
            return this.Endpoint.EndpointCollection.AfterService(action);
        }

        public IEndpointCollection AfterService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.AfterService(action);
        }

        public IEndpointCollection AfterService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.AfterService(action);
        }

        public IMethodCollection BeforeEndpoint(Func<bool> action)
        {
            return this.Endpoint.EndpointCollection.BeforeEndpoint(action);
        }

        public IMethodCollection BeforeEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.BeforeEndpoint(action);
        }

        public IMethodCollection BeforeEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.BeforeEndpoint(action);
        }

        public IMethodCollection BeforeMethod(Func<bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.BeforeActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection BeforeMethod(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.BeforeActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection BeforeMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.BeforeActions.Add(new FilterAction<T>(action));
            return this;
        }

        public IEndpointCollection BeforeService(Func<bool> action)
        {
            return this.Endpoint.EndpointCollection.BeforeService(action);
        }

        public IEndpointCollection BeforeService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.BeforeService(action);
        }

        public IEndpointCollection BeforeService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.BeforeService(action);
        }

        public void Clear()
        {
            this.list.Clear();
            this.CurrentMethod = null;
        }

        public bool Contains(Method item)
        {
            return this.list.Contains(item);
        }

        public void CopyTo(Method[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public IMethodCollection Delete(Action action)
        {
            return this.AddMethod(new Method(MethodType.Delete, this.Endpoint, this, action));
        }

        public IMethodCollection Delete(Action<IRequestMessage> action)
        {
            return this.AddMethod(new Method(MethodType.Delete, this.Endpoint, this, action));
        }

        public IMethodCollection Delete(Action<IRequestMessage, IResponseMessage> action)
        {
            return this.AddMethod(new Method(MethodType.Delete, this.Endpoint, this, action));
        }

        public IMethodCollection ErrorEndpoint(Func<Exception, bool> action)
        {
            return this.Endpoint.EndpointCollection.ErrorEndpoint(action);
        }

        public IMethodCollection ErrorEndpoint(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.ErrorEndpoint(action);
        }

        public IMethodCollection ErrorEndpoint<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.ErrorEndpoint(action);
        }

        public IMethodCollection ErrorMethod(Func<Exception, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ErrorActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection ErrorMethod(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ErrorActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection ErrorMethod<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ErrorActions.Add(new FilterAction<T>(action));
            return this;
        }

        public IEndpointCollection ErrorService(Func<Exception, bool> action)
        {
            return this.Endpoint.EndpointCollection.ErrorService(action);
        }

        public IEndpointCollection ErrorService(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.ErrorService(action);
        }

        public IEndpointCollection ErrorService<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.ErrorService(action);
        }

        public IEnumerator<Method> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public IMethodCollection Get(Action action)
        {
            return this.AddMethod(new Method(MethodType.Get, this.Endpoint, this, action));
        }

        public IMethodCollection Get(Action<IRequestMessage> action)
        {
            return this.AddMethod(new Method(MethodType.Get, this.Endpoint, this, action));
        }

        public IMethodCollection Get(Action<IRequestMessage, IResponseMessage> action)
        {
            return this.AddMethod(new Method(MethodType.Get, this.Endpoint, this, action));
        }

        public IMethodCollection Post(Action action)
        {
            return this.AddMethod(new Method(MethodType.Post, this.Endpoint, this, action));
        }

        public IMethodCollection Post<T>(Action<T> action)
        {
            return this.AddMethod(new Method<T>(MethodType.Post, this.Endpoint, this, action));
        }

        public IMethodCollection Post(Action<IRequestMessage> action)
        {
            return this.AddMethod(new Method(MethodType.Post, this.Endpoint, this, action));
        }

        public IMethodCollection Post<T>(Action<IRequestMessage<T>> action)
        {
            return this.AddMethod(new Method<T>(MethodType.Post, this.Endpoint, this, action));
        }

        public IMethodCollection Post(Action<IRequestMessage, IResponseMessage> action)
        {
            return this.AddMethod(new Method(MethodType.Post, this.Endpoint, this, action));
        }

        public IMethodCollection Post<T>(Action<IRequestMessage<T>, IResponseMessage> action)
        {
            return this.AddMethod(new Method<T>(MethodType.Post, this.Endpoint, this, action));
        }

        public IMethodCollection Put(Action action)
        {
            return this.AddMethod(new Method(MethodType.Put, this.Endpoint, this, action));
        }

        public IMethodCollection Put<T>(Action<T> action)
        {
            return this.AddMethod(new Method<T>(MethodType.Put, this.Endpoint, this, action));
        }

        public IMethodCollection Put(Action<IRequestMessage> action)
        {
            return this.AddMethod(new Method(MethodType.Put, this.Endpoint, this, action));
        }

        public IMethodCollection Put<T>(Action<IRequestMessage<T>> action)
        {
            return this.AddMethod(new Method<T>(MethodType.Put, this.Endpoint, this, action));
        }

        public IMethodCollection Put(Action<IRequestMessage, IResponseMessage> action)
        {
            return this.AddMethod(new Method(MethodType.Put, this.Endpoint, this, action));
        }

        public IMethodCollection Put<T>(Action<IRequestMessage<T>, IResponseMessage> action)
        {
            return this.AddMethod(new Method<T>(MethodType.Put, this.Endpoint, this, action));
        }

        public bool Remove(Method item)
        {
            bool removed = this.list.Remove(item);

            if (item == this.CurrentMethod)
            {
                this.CurrentMethod = this.list.LastOrDefault();
            }

            return removed;
        }

        public IMethodCollection WithEndpoint(string route, object typeConstraints = null)
        {
            return this.Endpoint.EndpointCollection.WithEndpoint(route, typeConstraints);
        }

        public IServiceCollection WithHostEncoding(IEncoding encoding)
        {
            return this.Endpoint.EndpointCollection.WithHostEncoding(encoding);
        }

        public IServiceCollection WithHostFormat(string mediaTypes, IFormat format)
        {
            return this.Endpoint.EndpointCollection.WithHostFormat(mediaTypes, format);
        }

        public IServiceCollection WithHostParameterParser(IRouteParameterParser parser)
        {
            return this.Endpoint.EndpointCollection.WithHostParameterParser(parser);
        }

        public IMethodCollection WithoutAfterEndpoint(Func<bool> action)
        {
            return this.Endpoint.EndpointCollection.WithoutAfterEndpoint(action);
        }

        public IMethodCollection WithoutAfterEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.WithoutAfterEndpoint(action);
        }

        public IMethodCollection WithoutAfterEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.WithoutAfterEndpoint(action);
        }

        public IMethodCollection WithoutAfterMethod(Func<bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ExcludeAfterActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection WithoutAfterMethod(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ExcludeAfterActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection WithoutAfterMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ExcludeAfterActions.Add(new FilterAction<T>(action));
            return this;
        }

        public IMethodCollection WithoutBeforeEndpoint(Func<bool> action)
        {
            return this.Endpoint.EndpointCollection.WithoutBeforeEndpoint(action);
        }

        public IMethodCollection WithoutBeforeEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.WithoutBeforeEndpoint(action);
        }

        public IMethodCollection WithoutBeforeEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.WithoutBeforeEndpoint(action);
        }

        public IMethodCollection WithoutBeforeMethod(Func<bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ExcludeBeforeActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection WithoutBeforeMethod(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ExcludeBeforeActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection WithoutBeforeMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ExcludeBeforeActions.Add(new FilterAction<T>(action));
            return this;
        }

        public IMethodCollection WithoutErrorEndpoint(Func<Exception, bool> action)
        {
            return this.Endpoint.EndpointCollection.WithoutErrorEndpoint(action);
        }

        public IMethodCollection WithoutErrorEndpoint(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.WithoutErrorEndpoint(action);
        }

        public IMethodCollection WithoutErrorEndpoint<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Endpoint.EndpointCollection.WithoutErrorEndpoint(action);
        }

        public IMethodCollection WithoutErrorMethod(Func<Exception, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ExcludeErrorActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection WithoutErrorMethod(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ExcludeErrorActions.Add(new FilterAction(action));
            return this;
        }

        public IMethodCollection WithoutErrorMethod<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentMethod == null)
            {
                throw new InvalidOperationException(MethodCollection.CurrentMethodNotSetMessage);
            }

            this.CurrentMethod.Pipeline.ExcludeErrorActions.Add(new FilterAction<T>(action));
            return this;
        }

        public IEndpointCollection WithoutServiceEncoding(IEncoding encoding)
        {
            return this.Endpoint.EndpointCollection.WithoutServiceEncoding(encoding);
        }

        public IEndpointCollection WithoutServiceFormat(string mediaTypes, IFormat format)
        {
            return this.Endpoint.EndpointCollection.WithoutServiceFormat(mediaTypes, format);
        }

        public IEndpointCollection WithService(string name, string baseUrl)
        {
            return this.Endpoint.EndpointCollection.WithService(name, baseUrl);
        }

        public IEndpointCollection WithServiceEncoding(IEncoding encoding)
        {
            return this.Endpoint.EndpointCollection.WithServiceEncoding(encoding);
        }

        public IEndpointCollection WithServiceFormat(string mediaTypes, IFormat format)
        {
            return this.Endpoint.EndpointCollection.WithServiceFormat(mediaTypes, format);
        }

        IEnumerator<Method> IEnumerable<Method>.GetEnumerator()
        {
            return ((IEnumerable<Method>)this.list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.list).GetEnumerator();
        }

        private IMethodCollection AddMethod(Method method)
        {
            this.Add(method);
            this.CurrentMethod = method;
            return this;
        }
    }
}