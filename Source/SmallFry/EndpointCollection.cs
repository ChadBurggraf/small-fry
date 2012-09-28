//-----------------------------------------------------------------------------
// <copyright file="EndpointCollection.cs" company="Tasty Codes">
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

    internal sealed class EndpointCollection : 
        ICollection<Endpoint>, 
        IEnumerable<Endpoint>, 
        IEnumerable, 
        IEndpointCollection
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

        public IEndpointCollection ErrorService(Func<IEnumerable<Exception>, bool> action)
        {
            return this.Service.ServiceCollection.ErrorService(action);
        }

        public IEndpointCollection ErrorService(Func<IEnumerable<Exception>, IRequestMessage, IResponseMessage, bool> action)
        {
            return this.Service.ServiceCollection.ErrorService(action);
        }

        public IEndpointCollection ErrorService<T>(Func<IEnumerable<Exception>, IRequestMessage<T>, IResponseMessage, bool> action)
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
            return this.WithEndpoint(route, null);
        }

        public IMethodCollection WithEndpoint(string route, object typeConstraints)
        {
            return this.WithEndpoint(route, typeConstraints, null);
        }

        public IMethodCollection WithEndpoint(string route, object typeConstraints, object patternConstraints)
        {
            Endpoint endpoint = new Endpoint(route, this.Service, this);
            endpoint.SetParameterTypes(typeConstraints);
            endpoint.SetParameterPatterns(patternConstraints);
            this.Add(endpoint);
            this.CurrentEndpoint = endpoint;
            return endpoint.Methods;
        }

        public IEndpointCollection WithoutServiceEncoding(IEncoding encoding)
        {
            return this.Service.ServiceCollection.WithoutServiceEncoding(encoding);
        }

        public IEndpointCollection WithoutServiceFormat(IFormat format)
        {
            return this.Service.ServiceCollection.WithoutServiceFormat(format);
        }

        public IEndpointCollection WithService(string name, string baseUrl)
        {
            return this.Service.ServiceCollection.WithService(name, baseUrl);
        }

        public IEndpointCollection WithServiceEncoding(IEncoding encoding)
        {
            return this.Service.ServiceCollection.WithServiceEncoding(encoding);
        }

        public IEndpointCollection WithServiceFormat(IFormat format)
        {
            return this.Service.ServiceCollection.WithServiceFormat(format);
        }

        IEnumerator<Endpoint> IEnumerable<Endpoint>.GetEnumerator()
        {
            return ((IEnumerable<Endpoint>)this.list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.list).GetEnumerator();
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection AfterEndpoint(Func<bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.AfterActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection AfterEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.AfterActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection AfterEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.AfterActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection BeforeEndpoint(Func<bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.BeforeActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection BeforeEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.BeforeActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection BeforeEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.BeforeActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection ErrorEndpoint(Func<IEnumerable<Exception>, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ErrorActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection ErrorEndpoint(Func<IEnumerable<Exception>, IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ErrorActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection ErrorEndpoint<T>(Func<IEnumerable<Exception>, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ErrorActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.Methods;
        }

        public IServiceCollection WithHostEncoding(IEncoding encoding)
        {
            return this.Service.ServiceCollection.WithHostEncoding(encoding);
        }

        public IServiceCollection WithHostFormat(IFormat format)
        {
            return this.Service.ServiceCollection.WithHostFormat(format);
        }

        public IServiceCollection WithHostParameterParser(IRouteParameterParser parser)
        {
            return this.Service.ServiceCollection.WithHostParameterParser(parser);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection WithoutAfterEndpoint(Func<bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeAfterActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection WithoutAfterEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeAfterActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection WithoutAfterEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeAfterActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection WithoutBeforeEndpoint(Func<bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeBeforeActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection WithoutBeforeEndpoint(Func<IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeBeforeActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection WithoutBeforeEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeBeforeActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection WithoutErrorEndpoint(Func<IEnumerable<Exception>, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeErrorActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection WithoutErrorEndpoint(Func<IEnumerable<Exception>, IRequestMessage, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeErrorActions.Add(new FilterAction(action));
            return this.CurrentEndpoint.Methods;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WithEndpoint", Justification = "Reviewed.")]
        public IMethodCollection WithoutErrorEndpoint<T>(Func<IEnumerable<Exception>, IRequestMessage<T>, IResponseMessage, bool> action)
        {
            if (this.CurrentEndpoint == null)
            {
                throw new InvalidOperationException(EndpointCollection.CurrentEndpointNotSetMessage);
            }

            this.CurrentEndpoint.Pipeline.ExcludeErrorActions.Add(new FilterAction<T>(action));
            return this.CurrentEndpoint.Methods;
        }
    }
}