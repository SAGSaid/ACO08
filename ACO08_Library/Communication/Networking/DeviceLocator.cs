using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ACO08_Library.Communication.Networking
{
    internal sealed class DeviceLocator : IDisposable
    {
        private const int BroadcastPort = 65531;
        private const string MessagePrefix = "CrimpNet_ACO08#";

        private readonly UdpClient _udpClient = 
            new UdpClient(new IPEndPoint(IPAddress.Any, BroadcastPort));

        private bool _isLocating = false;

        public event EventHandler<DeviceLocatedEventArgs> DeviceLocated; 

        public void StartLocating()
        {
            if (!_isLocating)
            {
                _isLocating = true;

                _udpClient.BeginReceive(MessageReceivedCallback, _udpClient);
            }
        }

        public void StopLocating()
        {
            _isLocating = false;
        }

        public bool IsLocating
        {
            get { return _isLocating; }
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
                        // Remove the prefix so only the serial number remains
                        var serialString = messageString.Replace(MessagePrefix, string.Empty);

                        if (uint.TryParse(serialString, out var serialNumber))
                        {
                            OnDeviceLocated(new DeviceLocatedEventArgs(serialNumber, source));

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
