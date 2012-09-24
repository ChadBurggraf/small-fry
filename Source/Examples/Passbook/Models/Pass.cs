namespace Passbook.Models
{
    using System;

    public sealed class Pass
    {
        public DateTime Created { get; set; }

        public string Data { get; set; }

        public long Id { get; set; }

        public DateTime LastUpdated { get; set; }

        public string PassTypeIdentifier { get; set; }

        public string SerialNumber { get; set; }
    }
}