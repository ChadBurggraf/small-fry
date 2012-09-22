//-----------------------------------------------------------------------------
// <copyright file="PipelineErrorType.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    /// <summary>
    /// Defines the type of unhandled errors that can occur
    /// during the execution of a service endpoint's pipeline.
    /// </summary>
    public enum PipelineErrorType
    {
        /// <summary>
        /// Identifies that the error type is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Identifies that an exception was thrown while executing
        /// one or more error handlers.
        /// </summary>
        ErrorHandlerThrewException,

        /// <summary>
        /// Identifies that an exception was thrown while writing
        /// response output to the client.
        /// </summary>
        WriteResponseOutputThrewException
    }
}