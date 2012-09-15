namespace SmallFry
{
    using System;

    internal sealed class MethodResult
    {
        public Exception Exception { get; set; }

        public bool Success { get; set; }
    }
}