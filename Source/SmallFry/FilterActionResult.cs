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
