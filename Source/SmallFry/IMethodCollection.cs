namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    public interface IMethodCollection : IEndpointCollection
    {
        IMethodCollection AfterMethod();

        IMethodCollection BeforeMethod();

        IMethodCollection Delete();

        IMethodCollection ErrorMethod();

        IMethodCollection Get();

        IMethodCollection Post();

        IMethodCollection Put();

        IMethodCollection WithoutAfterMethod();

        IMethodCollection WithoutBeforeMethod();

        IMethodCollection WithoutErrorMethod();
    }
}