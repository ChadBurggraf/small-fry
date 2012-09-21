//-----------------------------------------------------------------------------
// <copyright file="FormatLookupResult.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    internal sealed class FormatLookupResult
    {
        public IFormat Format { get; set; }

        public MediaType MediaType { get; set; }
    }
}