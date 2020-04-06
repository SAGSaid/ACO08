﻿using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ACO08_Library.Communication.Networking;
using ACO08_Library.Communication.Protocol;
using ACO08_Library.Data;
using ACO08_Library.Enums;

namespace ACO08_Library.Public
{
    public sealed class ACO08_Device : IDisposable, INotifyPropertyChanged
    {
        private DeviceEventListener _listener;
        private DeviceCommander _commander;

        private bool _isListeningForEvents = false;
        private bool _isConnected = false;

        public uint SerialNumber { get; }
        public IPEndPoint EndPoint { get; }

        public bool IsListeningForEvents
        {
            get { return _isListeningForEvents; }
            private set
            {
                _isListeningForEvents = value;
                OnPropertyChanged();
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            private set
            {
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler<CrimpDataReceivedEventArgs> CrimpDataReceived;


        public ACO08_Device(uint serialNumber, IPEndPoint endPoint)
        {
            SerialNumber = serialNumber;
            EndPoint = endPoint;
        }

        public async Task BeginListeningForCrimpDataEvent(CancellationToken token)
        {
            if (!IsListeningForEvents)
            {
                try
                {
                    _listener = new DeviceEventListener(EndPoint.Address);

                    _listener.StartListening();

                    _listener.CrimpDataChanged += CrimpDataChangedHandler;

                    IsListeningForEvents = true;

                    await Task.Delay(100, token);
                }
                catch (Exception)
                {
                    // In case something goes wrong,
                    // we don't want that to screw the consumer of the method

                }
                finally
                {
                    IsListeningForEvents = false;

                    _listener.StopListening();
                    _listener.CrimpDataChanged -= CrimpDataChangedHandler;
                    _listener.Dispose();
                    _listener = null;
                } 
            }
        }

        public async Task<bool> ConnectAsync()
        {
            if (!IsConnected)
            {
                try
                {
                    _commander = new DeviceCommander(EndPoint.Address);

                    if (!await _commander.ConnectAsync())
                    {
                        throw new Exception("Connection failed.");
                    }

                    // Go to workmode main, so the inital state is consistent.
                    var workmodeModeCommand = CommandFactory.Instance.GetCommand(CommandId.SetWorkmodeMain);
                    var response = _commander.SendCommand(workmodeModeCommand);

                    if (response.IsError)
                    {
                        throw new Exception("Setting the initial workmode to main failed.");
                    }

                    IsConnected = true;

                    return true;
                }
                catch (Exception)
                {
                    IsConnected = false;
                } 
            }

            return false;
        }

        private void CrimpDataChangedHandler(object sender, EventArgs e)
        {
            var getCrimpData = CommandFactory.Instance.GetCommand(CommandId.GetCrimpData);

            var response = _commander.SendCommand(getCrimpData);

            if (!response.IsError)
            {
                var crimpData = new CrimpData(response.GetBody());

                OnCrimpDataReceived(new CrimpDataReceivedEventArgs(crimpData));
            }
        }

        private void OnCrimpDataReceived(CrimpDataReceivedEventArgs args)
        {
            CrimpDataReceived?.Invoke(this, args);
        }


        public void Dispose()
        {
            _listener?.Dispose();
            _commander?.Dispose();
        }

        #region INotifyPropertyChanged

        // Autogenerated by ReSharper
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }
}
