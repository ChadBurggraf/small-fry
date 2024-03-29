﻿//-----------------------------------------------------------------------------
// <copyright file="IServiceCollection.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines the interface for a collection of services.
    /// </summary>
    public interface IServiceCollection : IEnumerable
    {
        /// <summary>
        /// Adds an action to perform after an endpoint's method to the current service and
        /// returns the service's <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        IEndpointCollection AfterService(Func<bool> action);

        /// <summary>
        /// Adds an action to perform after an endpoint's method to the current service and
        /// returns the service's <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        IEndpointCollection AfterService(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an action to perform after an endpoint's method to the current service and
        /// returns the service's <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IEndpointCollection AfterService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an action to perform before an endpoint's method to the current service and
        /// returns the service's <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        IEndpointCollection BeforeService(Func<bool> action);

        /// <summary>
        /// Adds an action to perform before an endpoint's method to the current service and
        /// returns the service's <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        IEndpointCollection BeforeService(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an action to perform before an endpoint's method to the current service and
        /// returns the service's <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IEndpointCollection BeforeService<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an error handler to the current service and returns the service's
        /// <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IEndpointCollection ErrorService(Func<IEnumerable<Exception>, bool> action);

        /// <summary>
        /// Adds an error handler to the current service and returns the service's
        /// <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IEndpointCollection ErrorService(Func<IEnumerable<Exception>, IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an error handler to the current service and returns the service's
        /// <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IEndpointCollection ErrorService<T>(Func<IEnumerable<Exception>, IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an endpoint to the current service and returns the new endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="route">The route defining the endpoint's URL format, relative to the service's
        /// base URL. Route parameters should be enclosed in curly brackets, with optional parameters
        /// identified by a quest mark (e.g., "accounts/{id}/{?emailAddress}"). You may add a wildcard
        /// parameter to the end of the route to capture arbitrary paths 
        /// (e.g., "accounts/{id}/{?emailAddress}/{*pathInfo}").</param>
        /// <returns>The new endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithEndpoint(string route);

        /// <summary>
        /// Adds an endpoint to the current service and returns the new endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="route">The route defining the endpoint's URL format, relative to the service's
        /// base URL. Route parameters should be enclosed in curly brackets, with optional parameters
        /// identified by a quest mark (e.g., "accounts/{id}/{?emailAddress}"). You may add a wildcard
        /// parameter to the end of the route to capture arbitrary paths 
        /// (e.g., "accounts/{id}/{?emailAddress}/{*pathInfo}").</param>
        /// <param name="typeConstraints">An object describing the type constraints of the route
        /// (e.g., new { id = typeof(int) }). Each type must be represented in the current host's
        /// <see cref="IRouteParameterParser"/> collection. All primitive .NET types are supported by default.</param>
        /// <returns>The new endpoint's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        IMethodCollection WithEndpoint(string route, object typeConstraints);

        /// <summary>
        /// Adds an endpoint to the current service and returns the new endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="route">The route defining the endpoint's URL format, relative to the service's
        /// base URL. Route parameters should be enclosed in curly brackets, with optional parameters
        /// identified by a quest mark (e.g., "accounts/{id}/{?emailAddress}"). You may add a wildcard
        /// parameter to the end of the route to capture arbitrary paths 
        /// (e.g., "accounts/{id}/{?emailAddress}/{*pathInfo}").</param>
        /// <param name="typeConstraints">An object describing the type constraints of the route
        /// (e.g., new { id = typeof(int) }). Each type must be represented in the current host's
        /// <see cref="IRouteParameterParser"/> collection. All primitive .NET types are supported by default.</param>
        /// <param name="patternConstraints">An object describing the pattern constraints of the route
        /// (e.g., new { id = "^\d+$" }). Each value should be a valid, parse-able regular expression.</param>
        /// <returns>The new endpoint's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        IMethodCollection WithEndpoint(string route, object typeConstraints, object patternConstraints);

        /// <summary>
        /// Adds an encoding to the current <see cref="IServiceHost"/> and returns the
        /// root <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="encoding">An <see cref="IEncoding"/> instance to use for processing.</param>
        /// <returns>The root <see cref="IServiceCollection"/>.</returns>
        IServiceCollection WithHostEncoding(IEncoding encoding);

        /// <summary>
        /// Adds a serialization format to the current <see cref="IServiceHost"/> and returns the
        /// root <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="format">An <see cref="IFormat"/> instance to use for serialization.</param>
        /// <returns>The root <see cref="IServiceCollection"/>.</returns>
        IServiceCollection WithHostFormat(IFormat format);

        /// <summary>
        /// Adds a new <see cref="IRouteParameterParser"/> to the current <see cref="IServiceHost"/>
        /// and returns the root <see cref="IServiceCollection"/>. Use this method to add custom route
        /// parameter type handling to all services. Note that all primitive .NET types are handled
        /// by the default <see cref="IRouteParameterParser"/> implementation.
        /// </summary>
        /// <param name="parser">An <see cref="IRouteParameterParser"/> that can be used for custom
        /// route parameter type handling.</param>
        /// <returns>The root <see cref="IServiceCollection"/>.</returns>
        IServiceCollection WithHostParameterParser(IRouteParameterParser parser);

        /// <summary>
        /// Removes a <see cref="IEncoding"/> from the current service. Use this method
        /// to exclude encodings added to the current <see cref="IServiceHost"/> from a service.
        /// </summary>
        /// <param name="encoding">An <see cref="IEncoding"/> instance to use for processing.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        IEndpointCollection WithoutServiceEncoding(IEncoding encoding);

        /// <summary>
        /// Removes a <see cref="IFormat"/> from the current service. Use this method
        /// to exclude formats added to the current <see cref="IServiceHost"/> from a service.
        /// </summary>
        /// <param name="format">An <see cref="IFormat"/> instance to use for serialization.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        IEndpointCollection WithoutServiceFormat(IFormat format);

        /// <summary>
        /// Adds a new service to the collection and returns its <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <param name="name">A friendly name for the service.</param>
        /// <param name="baseUrl">The service's base URL, relative to the application root
        /// (e.g., "/api").</param>
        /// <returns>The new service's <see cref="IEndpointCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Simpler API.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        IEndpointCollection WithService(string name, string baseUrl);

        /// <summary>
        /// Adds an encoding to the current service and returns the service's
        /// <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <param name="encoding">An <see cref="IEncoding"/> instance to use for processing.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        IEndpointCollection WithServiceEncoding(IEncoding encoding);

        /// <summary>
        /// Adds a serialization format to the current service and returns the service's
        /// <see cref="IEndpointCollection"/>.
        /// </summary>
        /// <param name="format">An <see cref="IFormat"/> instance to use for serialization.</param>
        /// <returns>The current service's <see cref="IEndpointCollection"/>.</returns>
        IEndpointCollection WithServiceFormat(IFormat format);
    }
}