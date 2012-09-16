﻿namespace SmallFry
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class ServiceCollection : ICollection<Service>, IEnumerable<Service>, IEnumerable, IServiceCollection
    {
        private const string CurrentServiceNotSetMessage = "Please add a service by calling WithService() before attempting to call this method.";
        private List<Service> list = new List<Service>();

        public ServiceCollection()
        {
            this.list = new List<Service>();
            this.Pipeline = new Pipeline();
        }
        
        public int Count
        {
            get { return this.list.Count; }
        }

        public Service CurrentService { get; private set; }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public Pipeline Pipeline { get; private set; }

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

            this.CurrentService.Pipeline.AfterActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection AfterService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.AfterActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection AfterService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.AfterActions.Add(new FilterAction<T>(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection BeforeService(Func<bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.BeforeActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection BeforeService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.BeforeActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection BeforeService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.BeforeActions.Add(new FilterAction<T>(action));
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

            this.CurrentService.Pipeline.ErrorActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection ErrorService(Func<Exception, IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.ErrorActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection ErrorService<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.ErrorActions.Add(new FilterAction<T>(action));
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
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            return this.CurrentService.Endpoints.WithEndpoint(route);
        }

        public IServiceCollection WithHostEncoding(string accept, IEncoding encoding)
        {
            this.Pipeline.Encodings.Add(new EncodingFilter(accept, encoding));
            return this;
        }

        public IServiceCollection WithHostFormat(string mediaTypes, IFormat format)
        {
            this.Pipeline.Formats.Add(new FormatFilter(mediaTypes, format));
            return this;
        }

        public IEndpointCollection WithoutServiceEncoding(string accept, IEncoding encoding)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.ExcludeEncodings.Add(new EncodingFilter(accept, encoding));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection WithoutServiceFormat(string mediaTypes, IFormat format)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.ExcludeFormats.Add(new FormatFilter(mediaTypes, format));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection WithService(string name, string baseUrl)
        {
            Service service = new Service(name, baseUrl, this);
            this.Add(service);
            this.CurrentService = service;
            return service.Endpoints;
        }

        public IEndpointCollection WithServiceEncoding(string accept, IEncoding encoding)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.Encodings.Add(new EncodingFilter(accept, encoding));
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection WithServiceFormat(string mediaTypes, IFormat format)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.Formats.Add(new FormatFilter(mediaTypes, format));
            return this.CurrentService.Endpoints;
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