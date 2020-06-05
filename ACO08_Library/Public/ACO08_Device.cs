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
        private Workmode _currentWorkmode = Workmode.Undefined;

        public uint SerialNumber { get; }
        public IPAddress Address { get; }
        public ACO08_Options Options { get; }

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
            get { return _commander?.IsConnected ?? false; }
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


        public ACO08_Device(uint serialNumber, IPAddress address)
        {
            SerialNumber = serialNumber;
            Address = address;
            Options = new ACO08_Options();
        }

        /// <summary>
        /// Starts listening for the CrimpDataChanged event and invokes its own event.
        /// </summary>
        public void StartListeningForEvents()
        {
            if (!_isListeningForEvents)
            {
                IsListeningForEvents = true;

                _listener = new DeviceEventListener();
                _listener.StartListening();
                _listener.CrimpDataChanged += CrimpDataChangedHandler;
                _listener.WorkmodeChanged += WorkmodeChangedHandler;
            }
        }

        /// <summary>
        /// Stops listening for events.
        /// </summary>
        public void StopListeningForEvents()
        {
            if (_isListeningForEvents)
            {
                IsListeningForEvents = false;

                _listener.StopListening();
                _listener.CrimpDataChanged -= CrimpDataChangedHandler;
                _listener.WorkmodeChanged -= WorkmodeChangedHandler;
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

                    Options.Commander = _commander;

                    _commander.PropertyChanged += IsConnectedChangedHandler;

                    StartListeningForEvents();
                    GetWorkmode();

                    return true;
                }
                catch (Exception)
                {

                }
            }

            return false;
        }

        private void CrimpDataChangedHandler(object sender, EventArgs args)
        {
            if (IsConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.GetCrimpData);

                var response = _commander.SendCommand(command);

                if (!response.IsError)
                {
                    var crimpData = new CrimpData(response.GetBody());

                    OnCrimpDataReceived(new CrimpDataReceivedEventArgs(crimpData));
                }
            }
        }

        private void IsConnectedChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(_commander.IsConnected))
            {
                OnPropertyChanged(nameof(IsConnected));
            }
        }

        private void WorkmodeChangedHandler(object sender, EventArgs args)
        {
            GetWorkmode();
        }

        private void OnCrimpDataReceived(CrimpDataReceivedEventArgs args)
        {
            CrimpDataReceived?.Invoke(this, args);
        }

        #region Commands

        public Version GetVersion()
        {
            if (IsConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.GetVersion);

                var response = _commander.SendCommand(command);

                if (!response.IsError)
                {
                    var body = response.GetBody();
                    
                    return new Version(body[0], body[1], body[2]);
                }
            }

            return new Version();
        }

        public Workmode GetWorkmode()
        {
            if (IsConnected)
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
            return SetWorkmode(CommandId.SetWorkmodeMain);
        }

        public bool SetWorkmodeMeasure()
        {
            return SetWorkmode(CommandId.SetWorkmodeReference);
        }

        public bool SetWorkmodeReference()
        {
            return SetWorkmode(CommandId.SetWorkmodeReference);
        }

        private bool SetWorkmode(CommandId id)
        {
            if (IsConnected)
            {
                var command = CommandFactory.Instance.GetCommand(id);

                var response = _commander.SendCommand(command);

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
