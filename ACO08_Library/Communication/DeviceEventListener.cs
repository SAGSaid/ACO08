﻿using System;
using System.Net;
using System.Net.Sockets;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication
{
    internal class DeviceEventListener : IDisposable
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

        public void StartListening()
        {
            _isListening = true;

            _udpClient.BeginReceive(MessageReceivedCallback, _udpClient);
        }

        public void StopListening()
        {
            _isListening = false;
        }

        public bool IsListening
        {
            get { return _isListening; }
        }

        private void MessageReceivedCallback(IAsyncResult result)
        {
            if (!_isListening)
            {
                // Not listening for events anymore
                return;
            }

            var dummy = new IPEndPoint(0, 0);

            var message = _udpClient.EndReceive(result, ref dummy);

            if (message.Length == 2)
            {
                var eventNumber = message[1];
                
                var eventType = (EventType) eventNumber;

                switch (eventType)
                {
                    case EventType.NewCrimpData:
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

            _udpClient.BeginReceive(MessageReceivedCallback, _udpClient);
        }

        private void OnCrimpDataChanged()
        {
            CrimpDataChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnWorkmodeChanged()
        {
            WorkmodeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnMultireferenceChanged()
        {
            MultireferenceChanged?.Invoke(this, EventArgs.Empty);
        }


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
