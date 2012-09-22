//-----------------------------------------------------------------------------
// <copyright file="MethodType.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    /// <summary>
    /// Defines the possible service endpoint method types.
    /// </summary>
    public enum MethodType
    {
        /// <summary>
        /// Identifies a DELETE method.
        /// </summary>
        Delete,

        /// <summary>
        /// Identifies a GET method.
        /// </summary>
        Get,

        /// <summary>
        /// Identifies a POST method.
        /// </summary>
        Post,

        /// <summary>
        /// Identifies a PUT method.
        /// </summary>
        Put
    }
}