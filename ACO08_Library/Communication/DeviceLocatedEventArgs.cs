using System;
using System.Net;

namespace ACO08_Library.Communication
{
    public class DeviceLocatedEventArgs : EventArgs
    {
        public uint SerialNumber { get; }
        public IPEndPoint EndPoint { get; }

        internal DeviceLocatedEventArgs(uint serialNumber, IPEndPoint endPoint)
        {
            SerialNumber = serialNumber;
            EndPoint = endPoint;
        }
    }
}

