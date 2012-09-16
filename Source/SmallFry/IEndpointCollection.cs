//-----------------------------------------------------------------------------
// <copyright file="IEndpointCollection.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the interface for a collection of endpoints belonging to a service.
    /// </summary>
    public interface IEndpointCollection : IServiceCollection
    {
        /// <summary>
        /// Adds an action to perform after an endpoint's method to the current endpoint and
        /// returns the endpoint's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection AfterEndpoint(Func<bool> action);

        /// <summary>
        /// Adds an action to perform after an endpoint's method to the current endpoint and
        /// returns the endpoint's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection AfterEndpoint(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an action to perform after an endpoint's method to the current endpoint and
        /// returns the endpoint's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection AfterEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an action to perform before an endpoint's method to the current endpoint and
        /// returns the endpoint's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection BeforeEndpoint(Func<bool> action);

        /// <summary>
        /// Adds an action to perform before an endpoint's method to the current endpoint and
        /// returns the endpoint's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection BeforeEndpoint(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an action to perform before an endpoint's method to the current endpoint and
        /// returns the endpoint's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection BeforeEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an error handler to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection ErrorEndpoint(Func<Exception, bool> action);

        /// <summary>
        /// Adds an error handler to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection ErrorEndpoint(Func<Exception, IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an error handler to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection ErrorEndpoint<T>(Func<Exception, IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an action to perform after one of the current endpoint's methods and returns the
        /// endpoint's <see cref="IMethodCollection"/>. Use this method to exclude an after action
        /// added to the current service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutAfterEndpoint(Func<bool> action);

        /// <summary>
        /// Removes an action to perform after one of the current endpoint's methods and returns the
        /// endpoint's <see cref="IMethodCollection"/>. Use this method to exclude an after action
        /// added to the current service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutAfterEndpoint(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an action to perform after one of the current endpoint's methods and returns the
        /// endpoint's <see cref="IMethodCollection"/>. Use this method to exclude an after action
        /// added to the current service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutAfterEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an action to perform before one of the current endpoint's methods and returns the
        /// endpoint's <see cref="IMethodCollection"/>. Use this method to exclude a before action
        /// added to the current service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutBeforeEndpoint(Func<bool> action);

        /// <summary>
        /// Removes an action to perform before one of the current endpoint's methods and returns the
        /// endpoint's <see cref="IMethodCollection"/>. Use this method to exclude a before action
        /// added to the current service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutBeforeEndpoint(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an action to perform before one of the current endpoint's methods and returns the
        /// endpoint's <see cref="IMethodCollection"/>. Use this method to exclude a before action
        /// added to the current service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutBeforeEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an error handler from the current endpoint. Use this method to exclude an error handler
        /// added to the current service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutErrorEndpoint(Func<bool> action);

        /// <summary>
        /// Removes an error handler from the current endpoint and returns the endpoint's 
        /// <see cref="IMethodCollection"/>. Use this method to exclude an error handler
        /// added to the current service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutErrorEndpoint(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an error handler from the current endpoint and returns the endpoint's 
        /// <see cref="IMethodCollection"/>. Use this method to exclude an error handler
        /// added to the current service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutErrorEndpoint<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);
    }
}