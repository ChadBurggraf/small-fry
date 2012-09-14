namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    public interface IServiceCollection
    {
        IServiceCollection AfterService();

        IServiceCollection BeforeService();

        IServiceCollection ErrorService();

        IServiceCollection WithService(string name);
    }
}