using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ACO08_Library.Communication.Networking.DeviceInterfacing
{
    /// <summary>
    /// Locates ACO08 devices on the network and
    /// provides necessary information on them.
    /// </summary>
    public sealed class DeviceLocator : IDisposable
    {
        private const int BroadcastPort = 65531;
        private const string MessagePrefix = "CrimpNet";

        private readonly UdpClient _udpClient = 
            new UdpClient(new IPEndPoint(IPAddress.Any, BroadcastPort));

        private bool _isLocating = false;

        public event EventHandler<DeviceLocatedEventArgs> DeviceLocated; 

        /// <summary>
        /// Starts the locating devices and
        /// invokes the appropriate event in case location is successful.
        /// </summary>
        public void StartLocating()
        {
            if (!_isLocating)
            {
                _isLocating = true;

                _udpClient.BeginReceive(MessageReceivedCallback, _udpClient);
            }
        }

        /// <summary>
        /// Stops the locating process.
        /// </summary>
        public void StopLocating()
        {
            _isLocating = false;
        }

        private void MessageReceivedCallback(IAsyncResult result)
        {
            if (_isLocating)
            {
                // This is a dummy variable reassigned below
                var source = new IPEndPoint(0, 0);

                try
                {
                    var message = _udpClient.EndReceive(result, ref source);

                    var messageString = Encoding.ASCII.GetString(message);

                    if (messageString.StartsWith(MessagePrefix))
                    {
                        // Take the part behind the hash sign to get the serial number
                        var split = messageString.Split('#');
                        var serialString = split[1];

                        if (uint.TryParse(serialString, out var serialNumber))
                        {
                            OnDeviceLocated(new DeviceLocatedEventArgs(serialNumber, source.Address));
                        }
                    }

                    _udpClient.BeginReceive(MessageReceivedCallback, _udpClient);
                }
                catch (ObjectDisposedException)
                {
                    // Saveguards the possibility of a race condition between 
                    // disposing and accessing the UDP-Client.
                }
            }
        }

        private void OnDeviceLocated(DeviceLocatedEventArgs args)
        {
            DeviceLocated?.Invoke(this, args);
        }

        /// <summary>
        /// Stops the locating process and disposes the underlying UDP client.
        /// </summary>
        public void Dispose()
        {
            if (_isLocating)
            {
                StopLocating();
            }

            _udpClient.Dispose();
        }
    }
}
