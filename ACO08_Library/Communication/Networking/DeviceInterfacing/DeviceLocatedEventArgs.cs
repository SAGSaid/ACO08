using System;
using System.Net;
using ACO08_Library.Public;

namespace ACO08_Library.Communication.Networking.DeviceInterfacing
{
    /// <summary>
    /// Arguments that are sent with the DeviceLocator.DeviceLocated event.
    /// </summary>
    public class DeviceLocatedEventArgs : EventArgs
    {
        public ACO08_Device Device { get; }

        internal DeviceLocatedEventArgs(uint serialNumber, IPAddress address)
        {
            Device = new ACO08_Device(serialNumber, address);
        }
    }
}

