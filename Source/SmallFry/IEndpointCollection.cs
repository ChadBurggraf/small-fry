namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    public interface IEndpointCollection : IServiceCollection
    {
        IMethodCollection AfterEndpoint();

        IMethodCollection BeforeEndpoint();

        IMethodCollection ErrorEndpoint();

        IMethodCollection WithoutAfterEndpoint();

        IMethodCollection WithoutBeforeEndpoint();

        IMethodCollection WithoutErrorEndpoint();
    }
}