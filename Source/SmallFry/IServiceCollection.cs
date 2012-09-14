namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    public interface IServiceCollection
    {
        IEndpointCollection AfterService();

        IEndpointCollection BeforeService();

        IEndpointCollection ErrorService();

        IMethodCollection WithEndpoint(string route);

        IEndpointCollection WithoutServiceEncoding(string names, IEncoding encoding);

        IEndpointCollection WithoutServiceFormat(string mimeTypes, IFormat format);

        IEndpointCollection WithService(string name);

        IEndpointCollection WithServiceEncoding(string names, IEncoding encoding);

        IEndpointCollection WithServiceFormat(string mimeTypes, IFormat format);

        IServiceCollection WithServicesEncoding(string names, IEncoding encoding);

        IServiceCollection WithServicesFormat(string mimeTypes, IFormat format);
    }
}