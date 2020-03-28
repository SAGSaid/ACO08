using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ACO08_Library.Communication
{
    internal class DeviceLocator : IDisposable
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

        private void MessageReceivedCallback(IAsyncResult result)
        {
            if (!_isLocating)
            {
                // A new message has been received,
                // but the instance isn't locating anymore.
                return;
            }

            // This is a dummy variable reassigned below
            var source = new IPEndPoint(0,0);

            var message = _udpClient.EndReceive(result, ref source);

            var messageString = Encoding.ASCII.GetString(message);

            if (messageString.StartsWith(MessagePrefix))
            {
                // Remove the prefix so only the serial number remains
                var serialString = messageString.Replace(MessagePrefix, string.Empty);
                var serialNumber = uint.Parse(serialString);

                OnDeviceLocated(new DeviceLocatedEventArgs(serialNumber, source));

            }

            _udpClient.BeginReceive(MessageReceivedCallback, _udpClient);
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
