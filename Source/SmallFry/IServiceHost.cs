//-----------------------------------------------------------------------------
// <copyright file="IServiceHost.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    /// <summary>
    /// Defines the interface for service hosts.
    /// </summary>
    public interface IServiceHost
    {
        /// <summary>
        /// Gets the collection of services hosted by this host.
        /// </summary>
        IServiceCollection Services { get; }
    }
}