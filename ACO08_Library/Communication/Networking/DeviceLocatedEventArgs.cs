using System;
using System.Net;

namespace ACO08_Library.Communication.Networking
{
    /// <summary>
    /// Arguments that are sent with the DeviceLocator.DeviceLocated event.
    /// </summary>
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

