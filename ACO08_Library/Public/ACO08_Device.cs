using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
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
        // For some commands the device expects an int for what is essentially a bool.
        private static readonly byte[] PaddedTrue = {1, 0, 0, 0};
        private static readonly byte[] PaddedFalse = {0, 0, 0, 0};

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
            if (!_isConnected)
            {
                try
                {
                    _commander = new DeviceCommander(Address);

                    if (!await _commander.ConnectAsync())
                    {
                        throw new Exception("Connection failed.");
                    }

                    IsConnected = true;

                    StartListeningForEvents();
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

        private void CrimpDataChangedHandler(object sender, EventArgs args)
        {
            if (_isConnected)
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
            if (_isConnected)
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
            if (_isConnected)
            {
                var command = CommandFactory.Instance.GetCommand(id);

                var response = _commander.SendCommand(command);

                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        #region Options

        public string GetOptionList()
        {
            if (_isConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.GetOptionList);

                var response = SendCommandWithMultiPacketResponse(command);

                if (response.IsError)
                {
                    return string.Empty;
                }

                return Encoding.Unicode.GetString(response.GetBody());
            }

            throw new InvalidOperationException("The device is not connected.");
        }



        public bool SaveSetup()
        {
            if (_isConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.SaveSetup);

                var response = _commander.SendCommand(command);

                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        public bool GetBooleanOption(OptionId id)
        {
            if (_isConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.GetOption);

                command.Header.Extension1 = (byte) id;

                var response = _commander.SendCommand(command);

                if (!response.IsError)
                {
                    // Is any of the bytes in the body set?
                    return response.GetBody().Any(b => b != 0);
                }
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        public bool SetOptionEnableInternalTrigger(bool value)
        {
            return SetBooleanOption(OptionId.EnableInternalTrigger, value);
        }

        private bool SetBooleanOption(OptionId id, bool value)
        {
            if (_isConnected)
            {
                var option = OptionFactory.Instance.GetBoolOption(id);

                option.Value = value;

                var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

                command.Header.Extension1 = (byte)option.Id;

                // The device expects 4 bytes for a boolean value 
                // (Probably just an int on the device)
                var body = value ? PaddedTrue : PaddedFalse;

                command.Body.AddRange(body);

                var response = _commander.SendCommand(command);

                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        #endregion

        private CommandResponse SendCommandWithMultiPacketResponse(Command command)
        {
            if (_isConnected)
            {
                var initialResponse = _commander.SendCommand(command);

                if (!initialResponse.IsError && initialResponse.GetHeader().Extension2 == 0)
                {
                    var nextBlockCommand = CommandFactory.Instance.GetCommand(CommandId.NextBlock);
                    nextBlockCommand.Header.Channel = Channel.None;
                    var additionalData = new List<byte>();

                    CommandResponse response;

                    do
                    {
                        response = _commander.SendCommand(nextBlockCommand);

                        if (!response.IsError)
                        {
                            additionalData.AddRange(response.GetBody());
                        }

                    } while (response.GetHeader().Extension2 == 0);

                    // Concatenate the initial response's data with the additional data.
                    initialResponse = new CommandResponse(
                        initialResponse.RawData.Concat(additionalData).ToArray(), command);
                }

                return initialResponse; 
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
