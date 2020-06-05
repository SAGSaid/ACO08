using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ACO08_Library.Communication.Protocol;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Networking.DeviceInterfacing
{
    /// <summary>
    /// Manages the TCP connection with the device and sends/receives commands.
    /// </summary>
    internal class DeviceCommander : IDisposable, INotifyPropertyChanged
    {
        private const int Port = 11000;
        private const int ReceiveBufferLength = 8192;

        // Protocol specific control signs
        private const byte STX = 0x02; // Start Text 
        private const byte ETX = 0x03; // End Text 
        private const byte DLE = 0x10; // Data Link escape, the byte after this byte is not interpreted as a control sign

        private TcpClient _tcpClient;
        private readonly IPEndPoint _deviceEndPoint;

        private bool _isConnected = false;

        public DeviceCommander(IPAddress address)
        {
            _deviceEndPoint = new IPEndPoint(address, Port);
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

        /// <summary>
        /// Connects the underlying TCP client.
        /// </summary>
        /// <returns>If it successfully connected</returns>
        public bool Connect()
        {
            if (!_isConnected)
            {
                IsConnected = true;

                try
                {
                    _tcpClient = new TcpClient(
                        new IPEndPoint(GetLocalIp(), Port));

                    _tcpClient.Connect(_deviceEndPoint);
                    return true;
                }
                catch (SocketException)
                {
                    _tcpClient?.Close();
                }
            }

            return false;
        }

        /// <summary>
        /// Connects the underlying TCP client asynchronously.
        /// </summary>
        /// <returns>Whether it successfully connected</returns>
        public async Task<bool> ConnectAsync()
        {
            if (!_isConnected)
            {
                IsConnected = true;

                try
                {
                    _tcpClient = new TcpClient(new IPEndPoint(GetLocalIp(), Port));

                    await _tcpClient.ConnectAsync(_deviceEndPoint.Address, Port);
                    return true;
                }
                catch (SocketException)
                {
                    _tcpClient?.Close();
                }
            }

            return false;
        }
            
        /// <summary>
        /// Sends a command to the device.
        /// </summary>
        /// <param name="command">The command to be sent</param>
        /// <returns>The device's response to the command.</returns>
        public CommandResponse SendCommand(Command command)
        {
            if (_isConnected)
            {
                var frame = PutDataIntoFrame(command.GetRawCommand());

                var stream = _tcpClient.GetStream();

                stream.Write(frame, 0, frame.Length);

                // This means that we check the network every 50 milliseconds for possible reads
                stream.ReadTimeout = 50;

                var responseFrame = new byte[ReceiveBufferLength];
                int bytesReadFromStream;
                
                do
                {
                    bytesReadFromStream = stream.Read(responseFrame, 0, ReceiveBufferLength);
                } while (bytesReadFromStream == 0);

                var responseData = ExtractDataFromFrame(responseFrame);

                return new CommandResponse(responseData, command);
            }

            throw new InvalidOperationException("The commander isn't connected to a device.");
        }

        /// <summary>
        /// Sends a command to the device asynchronously.
        /// </summary>
        /// <param name="command">The command to be sent</param>
        /// <returns>The device's response to the command</returns>
        public async Task<CommandResponse> SendCommandAsync(Command command)
        {
            if (_isConnected)
            {
                var frame = PutDataIntoFrame(command.GetRawCommand());

                var stream = _tcpClient.GetStream();

                await stream.WriteAsync(frame, 0, frame.Length);

                var responseFrame = new byte[ReceiveBufferLength];

                await stream.ReadAsync(responseFrame, 0, ReceiveBufferLength);

                var responseData = ExtractDataFromFrame(responseFrame);

                return new CommandResponse(responseData, command);
            }

            throw new InvalidOperationException("The commander isn't connected to a device.");
        }

        public CommandResponse SendCommandWithMultiPacketResponse(Command command)
        {
            if (_isConnected)
            {
                var initialResponse = SendCommand(command);

                if (!initialResponse.IsError && !IsLastResponse(initialResponse))
                {
                    var nextBlockCommand = CommandFactory.Instance.GetCommand(CommandId.NextBlock);
                    nextBlockCommand.Header.Channel = Channel.None;
                    var additionalData = new List<byte>();

                    CommandResponse response;

                    do
                    {
                        response = SendCommand(nextBlockCommand);

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

            throw new InvalidOperationException("The commander isn't connected to a device.");
        }

        /// <summary>
        /// Takes the raw data and transforms it according to protocol.
        /// </summary>
        /// <param name="rawData">The payload</param>
        /// <returns>A frame of data, ready to be sent</returns>
        private byte[] PutDataIntoFrame(byte[] rawData)
        {
            // Every frame starts with two STX control signs
            var buffer = new List<byte>
            {
                STX, STX
            };

            byte checkSum = 0;

            foreach (byte b in rawData)
            {
                checkSum += b;

                if (IsControlSign(b))
                {
                    // Escape control signs in the data with DLE
                    buffer.Add(DLE);
                }

                buffer.Add(b);
            }

            // Inverts the checksum 
            // XOR the byte with a fully set bit mask to invert the checksum safely
            checkSum ^= 0xFF;

            // In case the checksum must be escaped
            if (IsControlSign(checkSum))
            {
                buffer.Add(DLE);
            }

            // Second to last byte in frame is the checksum
            buffer.Add(checkSum);
            // Frame is terminated with ETX control sign.
            buffer.Add(ETX);

            return buffer.ToArray();
        }

        /// <summary>
        /// Extracts the raw data from a frame.
        /// </summary>
        /// <param name="responseFrame">The frame wherein the data lies.</param>
        /// <returns>The raw data</returns>
        private byte[] ExtractDataFromFrame(byte[] responseFrame)
        {
            var data = new List<byte>();

            bool isNextByteEscaped = false;
            byte checkSum = 0;

            // Escape control signs and add up the checksum
            foreach (byte b in responseFrame)
            {
                if (isNextByteEscaped)
                {
                    data.Add(b);
                    checkSum += b;
                    isNextByteEscaped = false;
                }
                else if (b == DLE)
                {
                    isNextByteEscaped = true;
                }
                else if (b == ETX)
                {
                    // Frame is terminated, so the loop can be exited
                    break;
                }
                else if (b == STX)
                {
                    // Ignore the control sign
                }
                else
                {
                    data.Add(b);
                    checkSum += b;
                }
            }

            // Remove the checksum at the end of the frame
            if (data.Any())
            {
                data.RemoveAt(data.Count - 1);
            }

            if (checkSum != 0)
            {
                throw new ACO08_Exception(ErrorId.Checksum);
            }

            return data.ToArray();
        }

        private static bool IsControlSign(byte byteToCheck)
        {
            return byteToCheck == STX || byteToCheck == ETX || byteToCheck == DLE;
        }

        private static bool IsLastResponse(CommandResponse response)
        {
            return response.GetHeader().Extension2 == 1;
        }

        private IPAddress GetLocalIp()
        {
            var selector = new NetworkInterfaceSelector();

            var localIp = selector.GetLocalIpAddressInSameSubnet(_deviceEndPoint.Address, 
                NetworkInterfaceSelector.Slash24SubnetMask);

            if (selector == null)
            {
                throw new Exception("The ACO08 and this device are not in the same logical network. " +
                                    "Please check the IP address and subnet mask configuration for both devices to ensure, " +
                                    "that they are in the same network.");
            }

            return localIp;
        }

        /// <summary>
        /// Closes the underlying TCP client.
        /// </summary>
        public void Dispose()
        {
            IsConnected = false;

            _tcpClient.Dispose();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }
}
