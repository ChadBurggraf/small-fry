//-----------------------------------------------------------------------------
// <copyright file="WriteResponseResult.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    internal sealed class WriteResponseResult
    {
        public Exception Exception { get; set; }

        public StatusCode StatusCode { get; set; }

        public bool Success { get; set; }
    }
}