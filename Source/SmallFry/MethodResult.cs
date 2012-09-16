//-----------------------------------------------------------------------------
// <copyright file="MethodResult.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    internal sealed class MethodResult
    {
        public Exception Exception { get; set; }

        public bool Success { get; set; }
    }
}