//-----------------------------------------------------------------------------
// <copyright file="IMethodCollection.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines the interface for a collection of methods belonging to an endpoint.
    /// </summary>
    public interface IMethodCollection : IEndpointCollection
    {
        /// <summary>
        /// Adds an action to perform after the current method and
        /// returns the method's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection AfterMethod(Func<bool> action);

        /// <summary>
        /// Adds an action to perform after the current method and
        /// returns the method's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection AfterMethod(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an action to perform after the current method and
        /// returns the method's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection AfterMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an action to perform before the current method and
        /// returns the method's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection BeforeMethod(Func<bool> action);

        /// <summary>
        /// Adds an action to perform before the current method and
        /// returns the method's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection BeforeMethod(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an action to perform before the current method and
        /// returns the method's <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection BeforeMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Adds a DELETE method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Delete(Action action);

        /// <summary>
        /// Adds a DELETE method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Delete(Action<IRequestMessage> action);

        /// <summary>
        /// Adds a DELETE method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Delete(Action<IRequestMessage, IResponseMessage> action);

        /// <summary>
        /// Adds an error handler to the current method and returns the method's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection ErrorMethod(Func<IEnumerable<Exception>, bool> action);

        /// <summary>
        /// Adds an error handler to the current method and returns the method's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection ErrorMethod(Func<IEnumerable<Exception>, IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Adds an error handler to the current method and returns the method's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection ErrorMethod<T>(Func<IEnumerable<Exception>, IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Adds a GET method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Sorry, case-insensitive languages.")]
        IMethodCollection Get(Action action);

        /// <summary>
        /// Adds a GET method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Sorry, case-insensitive languages.")]
        IMethodCollection Get(Action<IRequestMessage> action);

        /// <summary>
        /// Adds a GET method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Sorry, case-insensitive languages.")]
        IMethodCollection Get(Action<IRequestMessage, IResponseMessage> action);

        /// <summary>
        /// Adds a POST method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Post(Action action);

        /// <summary>
        /// Adds a POST method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Post<T>(Action<T> action);

        /// <summary>
        /// Adds a POST method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Post(Action<IRequestMessage> action);

        /// <summary>
        /// Adds a POST method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection Post<T>(Action<IRequestMessage<T>> action);

        /// <summary>
        /// Adds a POST method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Post(Action<IRequestMessage, IResponseMessage> action);

        /// <summary>
        /// Adds a POST method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection Post<T>(Action<IRequestMessage<T>, IResponseMessage> action);

        /// <summary>
        /// Adds a PUT method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Put(Action action);

        /// <summary>
        /// Adds a PUT method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Put<T>(Action<T> action);

        /// <summary>
        /// Adds a PUT method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Put(Action<IRequestMessage> action);

        /// <summary>
        /// Adds a PUT method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection Put<T>(Action<IRequestMessage<T>> action);

        /// <summary>
        /// Adds a PUT method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection Put(Action<IRequestMessage, IResponseMessage> action);

        /// <summary>
        /// Adds a PUT method to the current endpoint and returns the endpoint's
        /// <see cref="IMethodCollection"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current endpoint's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection Put<T>(Action<IRequestMessage<T>, IResponseMessage> action);

        /// <summary>
        /// Removes an action to perform after the current method and returns the
        /// method's <see cref="IMethodCollection"/>. Use this method to exclude an after action
        /// added to the current endpoint, service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutAfterMethod(Func<bool> action);

        /// <summary>
        /// Removes an action to perform after the current method and returns the
        /// method's <see cref="IMethodCollection"/>. Use this method to exclude an after action
        /// added to the current endpoint, service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutAfterMethod(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an action to perform after the current method and returns the
        /// method's <see cref="IMethodCollection"/>. Use this method to exclude an after action
        /// added to the current endpoint, service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection WithoutAfterMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an action to perform before the current method and returns the
        /// method's <see cref="IMethodCollection"/>. Use this method to exclude a before action
        /// added to the current endpoint, service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutBeforeMethod(Func<bool> action);

        /// <summary>
        /// Removes an action to perform before the current method and returns the
        /// method's <see cref="IMethodCollection"/>. Use this method to exclude a before action
        /// added to the current endpoint, service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        IMethodCollection WithoutBeforeMethod(Func<IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an action to perform before the current method and returns the
        /// method's <see cref="IMethodCollection"/>. Use this method to exclude a before action
        /// added to the current endpoint, service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection WithoutBeforeMethod<T>(Func<IRequestMessage<T>, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an error handler from the current method and returns the method's 
        /// <see cref="IMethodCollection"/>. Use this method to exclude an error handler
        /// added to the current endpoint, service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection WithoutErrorMethod(Func<IEnumerable<Exception>, bool> action);

        /// <summary>
        /// Removes an error handler from the current method and returns the method's 
        /// <see cref="IMethodCollection"/>. Use this method to exclude an error handler
        /// added to the current endpoint, service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection WithoutErrorMethod(Func<IEnumerable<Exception>, IRequestMessage, IResponseMessage, bool> action);

        /// <summary>
        /// Removes an error handler from the current method and returns the method's 
        /// <see cref="IMethodCollection"/>. Use this method to exclude an error handler
        /// added to the current endpoint, service or <see cref="IServiceHost"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the incoming request content.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current method's <see cref="IMethodCollection"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed.")]
        IMethodCollection WithoutErrorMethod<T>(Func<IEnumerable<Exception>, IRequestMessage<T>, IResponseMessage, bool> action);
    }
}