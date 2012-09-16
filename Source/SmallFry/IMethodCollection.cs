//-----------------------------------------------------------------------------
// <copyright file="IMethodCollection.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Specialized;

    public interface IMethodCollection : IEndpointCollection
    {
        IMethodCollection AfterMethod(Func<bool> action);

        IMethodCollection AfterMethod(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection AfterMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection BeforeMethod(Func<bool> action);

        IMethodCollection BeforeMethod(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection BeforeMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection Delete(Action action);

        IMethodCollection Delete(Action<IRequestMessage> action);

        IMethodCollection Delete(Action<IRequestMessage, IResponseMessage> action);

        IMethodCollection ErrorMethod(Func<bool> action);

        IMethodCollection ErrorMethod(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection ErrorMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection Get(Action action);

        IMethodCollection Get(Action<IRequestMessage> action);

        IMethodCollection Get(Action<IRequestMessage, IResponseMessage> action);

        IMethodCollection Post(Action action);

        IMethodCollection Post<T>(Action<T> action);

        IMethodCollection Post(Action<IRequestMessage> action);

        IMethodCollection Post<T>(Action<IRequestMessage<T>> action);

        IMethodCollection Post(Action<IRequestMessage, IResponseMessage> action);

        IMethodCollection Post<T>(Action<IRequestMessage<T>, IResponseMessage> action);

        IMethodCollection Put(Action action);

        IMethodCollection Put<T>(Action<T> action);

        IMethodCollection Put(Action<IRequestMessage> action);

        IMethodCollection Put<T>(Action<IRequestMessage<T>> action);

        IMethodCollection Put(Action<IRequestMessage, IResponseMessage> action);

        IMethodCollection Put<T>(Action<IRequestMessage<T>, IResponseMessage> action);

        IMethodCollection WithoutAfterMethod(Func<bool> action);

        IMethodCollection WithoutAfterMethod(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection WithoutAfterMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection WithoutBeforeMethod(Func<bool> action);

        IMethodCollection WithoutBeforeMethod(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection WithoutBeforeMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection WithoutErrorMethod(Func<bool> action);

        IMethodCollection WithoutErrorMethod(Func<IRequestMessage, IResponseMessage, bool> action);

        IMethodCollection WithoutErrorMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);
    }
}