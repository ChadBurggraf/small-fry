//-----------------------------------------------------------------------------
// <copyright file="IServiceHost.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    public interface IServiceHost
    {
        IServiceCollection Services { get; }
    }
}