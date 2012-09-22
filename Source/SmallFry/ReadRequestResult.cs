//-----------------------------------------------------------------------------
// <copyright file="ReadRequestResult.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    internal sealed class ReadRequestResult
    {
        public Exception Exception { get; set; }

        public object RequestObject { get; set; }

        public StatusCode StatusCode { get; set; }

        public bool Success { get; set; }
    }
}