namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    public interface IEndpointCollection : IServiceCollection
    {
        IEndpointCollection AfterEndpoint();

        IEndpointCollection BeforeEndpoint();

        IEndpointCollection ErrorEndpoint();

        IEndpointCollection WithEndpoint(string route);

        IEndpointCollection WithoutAfterEndpoint();

        IEndpointCollection WithoutBeforeEndpoint();

        IEndpointCollection WithoutErrorEndpoint();
    }
}