namespace SmallFry
{
    using System;
    using System.Collections.Specialized;

    public interface IMethodCollection : IEndpointCollection
    {
        IMethodCollection AfterMethod();

        IMethodCollection BeforeMethod();

        IMethodCollection Delete(Action action);

        IMethodCollection Delete(Action<IRequestMessage> action);

        IMethodCollection Delete(Action<IRequestMessage, IResponseMessage> action);

        IMethodCollection ErrorMethod();

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

        IMethodCollection WithoutAfterMethod();

        IMethodCollection WithoutBeforeMethod();

        IMethodCollection WithoutErrorMethod();
    }
}