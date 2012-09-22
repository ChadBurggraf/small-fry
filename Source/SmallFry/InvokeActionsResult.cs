//-----------------------------------------------------------------------------
// <copyright file="InvokeActionsResult.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Collections.Generic;

    internal sealed class InvokeActionsResult
    {
        public InvokeActionsResult(bool success, bool cont, ICollection<FilterActionResult> results)
        {
            this.Success = success;
            this.Continue = cont;
            this.Results = results;
        }

        public bool Continue { get; private set; }

        public ICollection<FilterActionResult> Results { get; private set; }

        public bool Success { get; private set; }
    }
}