using System;
using System.Net;

namespace ACO08_Library.Communication.Networking
{
    internal class DeviceLocatedEventArgs : EventArgs
    {
        public uint SerialNumber { get; }
        public IPAddress Address { get; }

        internal DeviceLocatedEventArgs(uint serialNumber, IPAddress address)
        {
            SerialNumber = serialNumber;
            Address = address;
        }
    }
}

