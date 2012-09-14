namespace SmallFry
{
    using System;

    public interface IServiceHost
    {
        IServiceCollection Services { get; }
    }
}