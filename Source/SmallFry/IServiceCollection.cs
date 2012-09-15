namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    public interface IServiceCollection
    {
        IEndpointCollection AfterService(Func<bool> action);

        IEndpointCollection AfterService(Func<IRequestMessage, IResponseMessage, bool> action);

        IEndpointCollection AfterService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IEndpointCollection BeforeService(Func<bool> action);

        IEndpointCollection BeforeService(Func<IRequestMessage, IResponseMessage, bool> action);

        IEndpointCollection BeforeService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        IEndpointCollection ErrorService(Func<Exception, bool> action);

        IEndpointCollection ErrorService(Func<Exception, IRequestMessage, IResponseMessage, bool> action);

        IEndpointCollection ErrorService<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action);

        IMethodCollection WithEndpoint(string route);

        IEndpointCollection WithoutServiceEncoding(string accept, IEncoding encoding);

        IEndpointCollection WithoutServiceFormat(string mediaTypes, IFormat format);

        IEndpointCollection WithService(string name, string baseUrl);

        IEndpointCollection WithServiceEncoding(string accept, IEncoding encoding);

        IEndpointCollection WithServiceFormat(string mediaTypes, IFormat format);

        IServiceCollection WithServicesEncoding(string accept, IEncoding encoding);

        IServiceCollection WithServicesFormat(string mediaTypes, IFormat format);
    }
}