//-----------------------------------------------------------------------------
// <copyright file="FilterActionResult.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    internal sealed class FilterActionResult
    {
        public bool Continue { get; set; }

        public Exception Exception { get; set; }

        public bool Success { get; set; }
    }
}
