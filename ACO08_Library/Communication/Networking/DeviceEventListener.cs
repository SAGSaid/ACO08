using System;
using System.Net;
using System.Net.Sockets;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Networking
{
    /// <summary>
    /// Translates the device's UDP events into C# native events.
    /// </summary>
    internal sealed class DeviceEventListener : IDisposable
    {
        private const int EventPort = 11001;

        private readonly UdpClient _udpClient;

        private bool _isListening = false;

        public event EventHandler CrimpDataChanged;
        public event EventHandler WorkmodeChanged;
        public event EventHandler MultireferenceChanged;

        public DeviceEventListener(IPAddress address)
        {
            _udpClient = new UdpClient(new IPEndPoint(address, EventPort));
        }

        /// <summary>
        /// Starts listening on the network for UDP events.
        /// </summary>
        public void StartListening()
        {
            if (!IsListening)
            {
                _isListening = true;

                _udpClient.BeginReceive(EventReceivedCallback, _udpClient); 
            }
        }

        /// <summary>
        /// Stops listening on the network for UDP events.
        /// </summary>
        public void StopListening()
        {
            _isListening = false;
        }

        /// <summary>
        /// Indicates whether the listener is activated.
        /// </summary>
        public bool IsListening
        {
            get { return _isListening; }
        }

        private void EventReceivedCallback(IAsyncResult result)
        {
            if (_isListening)
            {
                var dummy = new IPEndPoint(0, 0);

                try
                {
                    var message = _udpClient.EndReceive(result, ref dummy);

                    // Message is specified as a 16 Bit integer that identifies the event type.
                    if (message.Length == 2)
                    {
                        var eventNumber = message[1];

                        var eventType = (EventType)eventNumber;

                        switch (eventType)
                        {
                            case EventType.CrimpDataChanged:
                                OnCrimpDataChanged();
                                break;

                            case EventType.WorkmodeChanged:
                                OnWorkmodeChanged();
                                break;

                            case EventType.MultireferenceChanged:
                                OnMultireferenceChanged();
                                break;
                        }
                    }

                    _udpClient.BeginReceive(EventReceivedCallback, _udpClient);
                }
                catch (ObjectDisposedException)
                {
                    // Saveguards the possibility of a race condition between 
                    // disposing and accessing the UDP-Client.
                }
            }
        }

        private void OnCrimpDataChanged()
        {
            CrimpDataChanged?.Invoke(this, null);
        }

        private void OnWorkmodeChanged()
        {
            WorkmodeChanged?.Invoke(this, null);
        }

        private void OnMultireferenceChanged()
        {
            MultireferenceChanged?.Invoke(this, null);
        }

        /// <summary>
        /// Stops the listener and disposes the underlying UDP client.
        /// </summary>
        public void Dispose()
        {
            if (_isListening)
            {
                StopListening();
            }

            _udpClient.Dispose();
        }
    }
}
