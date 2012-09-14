namespace SmallFry
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class ServiceCollection : ICollection<Service>, IEnumerable<Service>, IEnumerable, IServiceCollection
    {
        private const string CurrentServiceNotSetMessage = "Please add a service by calling WithService() before attempting to call this method.";
        private List<Service> list = new List<Service>();
        
        public int Count
        {
            get { return this.list.Count; }
        }

        public Service CurrentService { get; private set; }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(Service item)
        {
            this.list.Add(item);
        }

        public IEndpointCollection AfterService(Func<bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.AfterActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection AfterService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.AfterActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection AfterService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.AfterActions.Add(new FilterAction<T>(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection BeforeService(Func<bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.BeforeActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection BeforeService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.BeforeActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection BeforeService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.BeforeActions.Add(new FilterAction<T>(action));
            return this.CurrentService.Endpoints;
        }

        public void Clear()
        {
            this.list.Clear();
            this.CurrentService = null;
        }

        public bool Contains(Service item)
        {
            return this.list.Contains(item);
        }

        public void CopyTo(Service[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public IEndpointCollection ErrorService(Func<Exception, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.ErrorActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection ErrorService(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.ErrorActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection ErrorService<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.ErrorActions.Add(new FilterAction<T>(action));
            return this.CurrentService.Endpoints;
        }

        public IEnumerator<Service> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public bool Remove(Service item)
        {
            bool removed = this.list.Remove(item);

            if (item == this.CurrentService)
            {
                this.CurrentService = this.list.LastOrDefault();
            }

            return removed;
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
            throw new NotImplementedException();
        }

        public IEndpointCollection WithServiceEncoding(string names, IEncoding encoding)
        {
            throw new NotImplementedException();
        }

        public IEndpointCollection WithServiceFormat(string mimeTypes, IFormat format)
        {
            throw new NotImplementedException();
        }

        public IServiceCollection WithServicesEncoding(string names, IEncoding encoding)
        {
            throw new NotImplementedException();
        }

        public IServiceCollection WithServicesFormat(string mimeTypes, IFormat format)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.list).GetEnumerator();
        }

        IEnumerator<Service> IEnumerable<Service>.GetEnumerator()
        {
            return ((IEnumerable<Service>)this.list).GetEnumerator();
        }
    }
}