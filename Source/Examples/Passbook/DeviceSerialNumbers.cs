namespace Passbook
{
    using System;
    using System.Collections.Generic;

    public sealed class DeviceSerialNumbers
    {
        private List<string> serialNumbers = new List<string>();

        public DateTime LastUpdated { get; set; }

        public IList<string> SerialNumbers
        {
            get { return this.serialNumbers; }
        }
    }
}