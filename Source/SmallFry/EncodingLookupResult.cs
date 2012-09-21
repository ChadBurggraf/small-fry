//-----------------------------------------------------------------------------
// <copyright file="EncodingLookupResult.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    internal sealed class EncodingLookupResult
    {
        public IEncoding Encoding { get; set; }

        public EncodingType EncodingType { get; set; }
    }
}