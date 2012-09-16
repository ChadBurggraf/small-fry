//-----------------------------------------------------------------------------
// <copyright file="IEndpointCollection.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    public interface IEndpointCollection : IServiceCollection
    {
        IMethodCollection AfterEndpoint(Func<bool> action);

        IMethodCollection AfterEndpoint(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection AfterEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection BeforeEndpoint(Func<bool> action);

        IMethodCollection BeforeEndpoint(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection BeforeEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection ErrorEndpoint(Func<Exception, bool> action);

        IMethodCollection ErrorEndpoint(Func<Exception, IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection ErrorEndpoint<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection WithoutAfterEndpoint(Func<bool> action);

        IMethodCollection WithoutAfterEndpoint(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection WithoutAfterEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection WithoutBeforeEndpoint(Func<bool> action);

        IMethodCollection WithoutBeforeEndpoint(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection WithoutBeforeEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection WithoutErrorEndpoint(Func<bool> action);

        IMethodCollection WithoutErrorEndpoint(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection WithoutErrorEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);
    }
}