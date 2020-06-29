using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using ACO08_Library.Enums;
using ACO08_Library.Public;

namespace ACO08_Library.Communication.Network
{
    /// <summary>
    /// Translates the device's UDP events into native C# events. Singleton
    /// </summary>
    public sealed class ACO08_EventListener : IDisposable
    {
        private const int EventPort = 11001;

        private readonly UdpClient _udpClient;

        private bool _isListening = false;

        private readonly List<ACO08_Device> _eventSubscribers = new List<ACO08_Device>();

        public static ACO08_EventListener Instance { get; } = new ACO08_EventListener();

        private ACO08_EventListener()
        {
            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, EventPort));
        }

        /// <summary>
        /// Starts listening on the network for UDP events.
        /// Nothing happens in case it is already listening.
        /// </summary>
        public void StartListening()
        {
            if (!_isListening)
            {
                _isListening = true;

                _udpClient.BeginReceive(EventReceivedCallback, _udpClient); 
            }
        }

        public void Subscribe4Events(ACO08_Device device)
        {
            // Prevent multiple subscriptions from the same device
            if (!_eventSubscribers.Contains(device))
            {
                _eventSubscribers.Add(device);
            }
        }

        public void UnsubscribeFromEvents(ACO08_Device device)
        {
            if (_eventSubscribers.Contains(device))
            {
                _eventSubscribers.Remove(device);
            }
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
                var eventDevice = new IPEndPoint(0, 0);

                try
                {
                    var message = _udpClient.EndReceive(result, ref eventDevice);

                    // Message is specified as a 16 Bit integer that identifies the event type.
                    if (message.Length == 2)
                    {
                        var eventNumber = message[0];

                        var eventType = (EventType)eventNumber;

                        var matchingDevice =
                            _eventSubscribers.FirstOrDefault(device => device.Address.Equals(eventDevice.Address));

                        if (matchingDevice != null)
                        {
                            switch (eventType)
                            {
                                case EventType.CrimpDataChanged:
                                    matchingDevice.CrimpDataChanged();
                                    break;

                                case EventType.WorkmodeChanged:
                                    matchingDevice.WorkmodeChanged();
                                    break;

                                case EventType.MultireferenceChanged:
                                    matchingDevice.MultiReferenceChanged();
                                    break;
                            }
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


        /// <summary>
        /// Stops the listener and disposes the underlying UDP client.
        /// Be very careful to call this explicitly, as other subscribers might
        /// unexpectedly not receive events anymore.
        /// </summary>
        public void Dispose()
        {
            _isListening = false;

            _udpClient.Dispose();
        }
    }
}
