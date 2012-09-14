namespace SmallFry
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class EndpointCollection : ICollection<Endpoint>, IEnumerable<Endpoint>, IEnumerable, IEndpointCollection
    {
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

        public bool IsReadOnly
        {
            get { return false; }
        }

        public Service Service { get; private set; }

        public void Add(Endpoint item)
        {
            this.list.Add(item);
        }

        public void Clear()
        {
            this.list.Clear();
        }

        public bool Contains(Endpoint item)
        {
            return this.list.Contains(item);
        }

        public void CopyTo(Endpoint[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Endpoint> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public bool Remove(Endpoint item)
        {
            return this.list.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.list).GetEnumerator();
        }

        IEnumerator<Endpoint> IEnumerable<Endpoint>.GetEnumerator()
        {
            return ((IEnumerable<Endpoint>)this.list).GetEnumerator();
        }

        #region IEndpointCollection Members

        public IMethodCollection AfterEndpoint()
        {
            throw new NotImplementedException();
        }

        public IMethodCollection BeforeEndpoint()
        {
            throw new NotImplementedException();
        }

        public IMethodCollection ErrorEndpoint()
        {
            throw new NotImplementedException();
        }

        public IMethodCollection WithoutAfterEndpoint()
        {
            throw new NotImplementedException();
        }

        public IMethodCollection WithoutBeforeEndpoint()
        {
            throw new NotImplementedException();
        }

        public IMethodCollection WithoutErrorEndpoint()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServiceCollection Members

        public IEndpointCollection AfterService(Func<bool> action)
        {
            this.Service.AfterActions.Add(new FilterAction(action));
            return this;
        }

        public IEndpointCollection AfterService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            this.Service.AfterActions.Add(new FilterAction(action));
            return this;
        }

        public IEndpointCollection AfterService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            this.Service.AfterActions.Add(new FilterAction<T>(action));
            return this;
        }

        public IEndpointCollection BeforeService(Func<bool> action)
        {
            this.Service.BeforeActions.Add(new FilterAction(action));
            return this;
        }

        public IEndpointCollection BeforeService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            this.Service.BeforeActions.Add(new FilterAction(action));
            return this;
        }

        public IEndpointCollection BeforeService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            this.Service.BeforeActions.Add(new FilterAction<T>(action));
            return this;
        }

        public IEndpointCollection ErrorService(Func<Exception, bool> action)
        {
            this.Service.ErrorActions.Add(new FilterAction(action));
            return this;
        }

        public IEndpointCollection ErrorService(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            this.Service.ErrorActions.Add(new FilterAction(action));
            return this;
        }

        public IEndpointCollection ErrorService<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            this.Service.ErrorActions.Add(new FilterAction<T>(action));
            return this;
        }

        public IMethodCollection WithEndpoint(string route)
        {
            throw new NotImplementedException();
        }

        public IEndpointCollection WithoutServiceEncoding(string names, IEncoding encoding)
        {
            throw new NotImplementedException();
        }

        public IEndpointCollection WithoutServiceFormat(string mimeTypes, IFormat format)
        {
            throw new NotImplementedException();
        }

        public IEndpointCollection WithService(string name)
        {
            return this.Service.ServiceCollection.WithService(name);
        }

        public IEndpointCollection WithServiceEncoding(string names, IEncoding encoding)
        {
            this.Service.Encodings.Add(new EncodingFilter(names, encoding));
            return this;
        }

        public IEndpointCollection WithServiceFormat(string mimeTypes, IFormat format)
        {
            this.Service.Formats.Add(new FormatFilter(mimeTypes, format));
            return this;
        }

        public IServiceCollection WithServicesEncoding(string names, IEncoding encoding)
        {
            return this.Service.ServiceCollection.WithServicesEncoding(names, encoding);
        }

        public IServiceCollection WithServicesFormat(string mimeTypes, IFormat format)
        {
            return this.Service.ServiceCollection.WithServicesFormat(mimeTypes, format);
        }

        #endregion
    }
}