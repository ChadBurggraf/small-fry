﻿//-----------------------------------------------------------------------------
// <copyright file="ServiceHost.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Provides a container for registering and defining services.
    /// </summary>
    public sealed class ServiceHost : IServiceHost
    {
        private static readonly ServiceHost ServiceHostInstance = new ServiceHost();
        private ServiceCollection services;
        private ServiceResolver serviceResolver;

        private ServiceHost()
        {
            this.services = new ServiceCollection();
        }

        /// <summary>
        /// Gets the singleton <see cref="ServiceHost"/> instance.
        /// </summary>
        public static ServiceHost Instance
        {
            get { return ServiceHost.ServiceHostInstance; }
        }

        /// <summary>
        /// Gets the collection of services hosted by this host.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ApplicationStart", Justification = "Reviewed.")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asax", Justification = "Reviewed.")]
        public IServiceCollection Services
        {
            get 
            {
                if (this.serviceResolver != null)
                {
                    throw new InvalidOperationException("Services have already started being resolved and invoked by this application. You must perform all service registration and definition on application launch (i.e., in Application_Start() of Global.asax or similar).");
                }

                return this.services; 
            }
        }

        internal ServiceResolver ServiceResolver
        {
            get { return this.serviceResolver ?? (this.serviceResolver = new ServiceResolver(this.services)); }
        }
    }
}