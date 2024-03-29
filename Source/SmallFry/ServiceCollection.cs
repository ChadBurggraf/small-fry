﻿//-----------------------------------------------------------------------------
// <copyright file="ServiceCollection.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    internal sealed class ServiceCollection : 
        ICollection<Service>, 
        IEnumerable<Service>, 
        IEnumerable, 
        IServiceCollection
    {
        private const string CurrentServiceNotSetMessage = "Please add a service by calling WithService() before attempting to call this method.";
        private List<Service> list = new List<Service>();

        public ServiceCollection()
        {
            this.list = new List<Service>();
            this.Pipeline = new Pipeline();
            this.RouteValueBinder = new RouteValueBinder();
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

        public RouteValueBinder RouteValueBinder { get; private set; }

        public void Add(Service item)
        {
            this.list.Add(item);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection AfterService(Func<bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.AfterActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection AfterService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.AfterActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection AfterService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.AfterActions.Add(new FilterAction<T>(action));
            return this.CurrentService.Endpoints;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection BeforeService(Func<bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.BeforeActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection BeforeService(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.BeforeActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
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

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection ErrorService(Func<IEnumerable<Exception>, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.ErrorActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection ErrorService(Func<IEnumerable<Exception>, IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.ErrorActions.Add(new FilterAction(action));
            return this.CurrentService.Endpoints;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection ErrorService<T>(Func<IEnumerable<Exception>, IRequestMessage<T>, IResponseMessage, bool> action)
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
            return this.WithEndpoint(route, null);
        }

        public IMethodCollection WithEndpoint(string route, object typeConstraints) 
        {
            return this.WithEndpoint(route, typeConstraints, null);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IMethodCollection WithEndpoint(string route, object typeConstraints, object patternConstraints)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            return this.CurrentService.Endpoints.WithEndpoint(route, typeConstraints, patternConstraints);
        }

        public IServiceCollection WithHostEncoding(IEncoding encoding)
        {
            this.Pipeline.Encodings.Add(encoding);
            return this;
        }

        public IServiceCollection WithHostFormat(IFormat format)
        {
            this.Pipeline.Formats.Add(format);
            return this;
        }

        public IServiceCollection WithHostParameterParser(IRouteParameterParser parser)
        {
            this.RouteValueBinder.AddParser(parser);
            return this;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection WithoutServiceEncoding(IEncoding encoding)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.ExcludeEncodings.Add(encoding);
            return this.CurrentService.Endpoints;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection WithoutServiceFormat(IFormat format)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.ExcludeFormats.Add(format);
            return this.CurrentService.Endpoints;
        }

        public IEndpointCollection WithService(string name, string baseUrl)
        {
            Service service = new Service(name, baseUrl, this);
            this.Add(service);
            this.CurrentService = service;
            return service.Endpoints;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection WithServiceEncoding(IEncoding encoding)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.Encodings.Add(encoding);
            return this.CurrentService.Endpoints;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithService", Justification = "Reviewed.")]
        public IEndpointCollection WithServiceFormat(IFormat format)
        {
            if (this.CurrentService == null)
            {
                throw new InvalidOperationException(ServiceCollection.CurrentServiceNotSetMessage);
            }

            this.CurrentService.Pipeline.Formats.Add(format);
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