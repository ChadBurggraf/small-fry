namespace SmallFry
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class EndpointCollection : ICollection<Endpoint>, IEnumerable<Endpoint>, IEnumerable, IEndpointCollection
    {
        private const string CurrentEndpointNotSetMessage = "Please add an endpoint by calling WithEndpoint() before attempting to call this method.";
        private List<Endpoint> list = new List<Endpoint>();

        public EndpointCollection(Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service", "service cannot be null.");
            }

            this.Service = service;
        }

        public int Count
        {
            get { return this.list.Count; }
        }

        public Endpoint CurrentEndpoint { get; private set; }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public Service Service { get; private set; }

        public void Add(Endpoint item)
        {
            this.list.Add(item);
        }

        public IEndpointCollection AfterService(Func<bool> action)
        {
            return this.Service.ServiceCollection.AfterService(action);
        }

        public IEndpointCollection AfterService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Service.ServiceCollection.AfterService(action);
        }

        public IEndpointCollection AfterService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Service.ServiceCollection.AfterService(action);
        }

        public IEndpointCollection BeforeService(Func<bool> action)
        {
            return this.Service.ServiceCollection.BeforeService(action);
        }

        public IEndpointCollection BeforeService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Service.ServiceCollection.BeforeService(action);
        }

        public IEndpointCollection BeforeService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Service.ServiceCollection.BeforeService(action);
        }

        public void Clear()
        {
            this.list.Clear();
            this.CurrentEndpoint = null;
        }

        public bool Contains(Endpoint item)
        {
            return this.list.Contains(item);
        }

        public void CopyTo(Endpoint[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public IEndpointCollection ErrorService(Func<Exception, bool> action)
        {
            return this.Service.ServiceCollection.ErrorService(action);
        }

        public IEndpointCollection ErrorService(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Service.ServiceCollection.ErrorService(action);
        }

        public IEndpointCollection ErrorService<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            return this.Service.ServiceCollection.ErrorService(action);
        }

        public IEnumerator<Endpoint> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public bool Remove(Endpoint item)
        {
            bool removed = this.list.Remove(item);

            if (item == this.CurrentEndpoint)
            {
                this.CurrentEndpoint = this.list.LastOrDefault();
            }

            return removed;
        }

        public IMethodCollection WithEndpoint(string route)
        {
            Endpoint endpoint = new Endpoint(route, this.Service, this);
            this.Add(endpoint);
            this.CurrentEndpoint = endpoint;
            return endpoint.MethodCollection;
        }

        public IEndpointCollection WithoutServiceEncoding(string accept, IEncoding encoding)
        {
            return this.Service.ServiceCollection.WithoutServiceEncoding(accept, encoding);
        }

        public IEndpointCollection WithoutServiceFormat(string mediaTypes, IFormat format)
        {
            return this.Service.ServiceCollection.WithoutServiceFormat(mediaTypes, format);
        }

        public IEndpointCollection WithService(string name, string baseUrl)
        {
            return this.Service.ServiceCollection.WithService(name, baseUrl);
        }

        public IEndpointCollection WithServiceEncoding(string accept, IEncoding encoding)
        {
            return this.Service.ServiceCollection.WithServiceEncoding(accept, encoding);
        }

        public IEndpointCollection WithServiceFormat(string mediaTypes, IFormat format)
        {
            return this.Service.ServiceCollection.WithServiceFormat(mediaTypes, format);
        }

        public IServiceCollection WithServicesEncoding(string accept, IEncoding encoding)
        {
            return this.Service.ServiceCollection.WithServicesEncoding(accept, encoding);
        }

        public IServiceCollection WithServicesFormat(string mediaTypes, IFormat format)
        {
            return this.Service.ServiceCollection.WithServicesFormat(mediaTypes, format);
        }

        IEnumerator<Endpoint> IEnumerable<Endpoint>.GetEnumerator()
        {
            return ((IEnumerable<Endpoint>)this.list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.list).GetEnumerator();
        }

        public IMethodCollection AfterEndpoint(Func<bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.AfterActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection AfterEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.AfterActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection AfterEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.AfterActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection BeforeEndpoint(Func<bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.BeforeActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection BeforeEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.BeforeActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection BeforeEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.BeforeActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection ErrorEndpoint(Func<Exception, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ErrorActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection ErrorEndpoint(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ErrorActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection ErrorEndpoint<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ErrorActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection WithoutAfterEndpoint(Func<bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeAfterActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection WithoutAfterEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeAfterActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection WithoutAfterEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeAfterActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection WithoutBeforeEndpoint(Func<bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeBeforeActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection WithoutBeforeEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeBeforeActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection WithoutBeforeEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeBeforeActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection WithoutErrorEndpoint(Func<bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeErrorActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection WithoutErrorEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeErrorActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.MethodCollection;
        }

        public IMethodCollection WithoutErrorEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeErrorActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.MethodCollection;
        }
    }
}