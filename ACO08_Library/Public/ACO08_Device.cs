using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ACO08_Library.Communication.Networking.DeviceInterfacing;
using ACO08_Library.Communication.Protocol;
using ACO08_Library.Data;
using ACO08_Library.Enums;

namespace ACO08_Library.Public
{
    /// <summary>
    /// Implements a public interface for device interaction.
    /// </summary>
    public sealed class ACO08_Device : IDisposable, INotifyPropertyChanged
    {
        private DeviceEventListener _listener;
        private DeviceCommander _commander;

        private bool _isListeningForEvents = false;
        private bool _isConnected = false;
        private Workmode _currentWorkmode = Workmode.Undefined;

        public uint SerialNumber { get; }
        public IPAddress Address { get; }

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

        public Workmode CurrentWorkmode
        {
            get { return _currentWorkmode; }
            private set
            {
                _currentWorkmode = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler<CrimpDataReceivedEventArgs> CrimpDataReceived;


        internal ACO08_Device(uint serialNumber, IPAddress address)
        {
            SerialNumber = serialNumber;
            Address = address;
        }

        /// <summary>
        /// Starts listening for the CrimpDataChanged event and invokes its own event.
        /// </summary>
        public void StartListeningForCrimpDataEvent()
        {
            if (!IsListeningForEvents)
            {
                IsListeningForEvents = true;

                _listener = new DeviceEventListener(Address);
                _listener.StartListening();
                _listener.CrimpDataChanged += CrimpDataChangedHandler;
            }
        }

        /// <summary>
        /// Stops listening for events.
        /// </summary>
        public void StopListeningForCrimpDataEvent()
        {
            if (IsListeningForEvents)
            {
                IsListeningForEvents = false;

                _listener.StopListening();
                _listener.CrimpDataChanged -= CrimpDataChangedHandler;
                _listener.Dispose();
                _listener = null; 
            }
        }

        /// <summary>
        /// Connects the TCP client for sending commands.
        /// </summary>
        /// <returns>Whether establishing the connection was successful</returns>
        public async Task<bool> ConnectAsync()
        {
            if (!IsConnected)
            {
                try
                {
                    _commander = new DeviceCommander(Address);

                    if (!await _commander.ConnectAsync())
                    {
                        throw new Exception("Connection failed.");
                    }

                    IsConnected = true;

                    GetWorkmode();

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
            if (_commander != null)
            {
                var getCrimpData = CommandFactory.Instance.GetCommand(CommandId.GetCrimpData);

                var response = _commander.SendCommand(getCrimpData);

                if (!response.IsError)
                {
                    var crimpData = new CrimpData(response.GetBody());

                    OnCrimpDataReceived(new CrimpDataReceivedEventArgs(crimpData));
                } 
            }
        }

        private void OnCrimpDataReceived(CrimpDataReceivedEventArgs args)
        {
            CrimpDataReceived?.Invoke(this, args);
        }

        #region Commands

        public Workmode GetWorkmode()
        {
            if (_isConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.GetWorkmode);

                var response = _commander.SendCommand(command);

                CurrentWorkmode = (Workmode)response.GetHeader().Extension1;

                return CurrentWorkmode;
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        public bool SetWorkmodeMain()
        {
            if (_isConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.SetWorkmodeMain);

                var response = _commander.SendCommand(command);

                if (!response.IsError)
                {
                    CurrentWorkmode = Workmode.Main;
                }

                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        public bool SetWorkmodeMeasure()
        {
            if (_isConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.SetWorkmodeMain);

                var response = _commander.SendCommand(command);

                if (!response.IsError)
                {
                    CurrentWorkmode = Workmode.Measure;
                }
                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        public bool SetWorkmodeReference()
        {
            if (_isConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.SetWorkmodeMain);

                var response = _commander.SendCommand(command);

                if (!response.IsError)
                {
                    CurrentWorkmode = Workmode.Reference;
                }

                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        #endregion

        /// <summary>
        /// Disposes the underlying sockets.
        /// </summary>
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
