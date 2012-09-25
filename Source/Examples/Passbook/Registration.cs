namespace Passbook
{
    using System;

    public sealed class Registration
    {
        public DateTime Created { get; set; }

        public string DeviceLibraryIdentifier { get; set; }

        public long Id { get; set; }

        public DateTime LastUpdated { get; set; }
        
        public long PassId { get; set; }

        public string PushToken { get; set; }
    }
}