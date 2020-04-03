using System;
using System.Net;

namespace ACO08_Library.Communication.Networking
{
    internal class DeviceLocatedEventArgs : EventArgs
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

