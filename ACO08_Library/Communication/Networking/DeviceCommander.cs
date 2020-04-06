using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ACO08_Library.Communication.Protocol;

namespace ACO08_Library.Communication.Networking
{
    internal class DeviceCommander : IDisposable
    {
        private const int Port = 11000;
        private const int BufferLength = 8192;

        // Control signs
        private const byte STX = 0x02; // Start Text 
        private const byte ETX = 0x03; // End Text 
        private const byte DLE = 0x10; // Data Link escape, the byte after this byte is not interpreted as a control sign

        private readonly TcpClient _tcpClient;
        private readonly IPEndPoint _deviceEndPoint;

        private bool _isConnected = false;

        public DeviceCommander(IPAddress address)
        {
            _deviceEndPoint = new IPEndPoint(address, Port);
            _tcpClient = new TcpClient(_deviceEndPoint);
        }

        public bool Connect()
        {
            if (!_isConnected)
            {
                _isConnected = true;

                try
                {
                    _tcpClient.Connect(_deviceEndPoint);
                    return true;
                }
                catch (SocketException)
                {
                    
                }
            }

            return false;
        }

        public async Task<bool> ConnectAsync()
        {
            if (!_isConnected)
            {
                _isConnected = true;

                try
                {
                    await _tcpClient.ConnectAsync(_deviceEndPoint.Address, Port);
                    return true;
                }
                catch (SocketException)
                {

                }
            }

            return false;
        }
            
        public CommandResponse SendCommand(Command command)
        {
            if (_isConnected)
            {
                var frame = PutDataIntoFrame(command.GetRawCommand());

                var stream = _tcpClient.GetStream();

                stream.Write(frame, 0, frame.Length);

                // This means that we check the network every 50 milliseconds for possible reads
                stream.ReadTimeout = 50;
                int bytesReadFromStream;

                var responseFrame = new byte[BufferLength];

                do
                {
                    bytesReadFromStream = stream.Read(responseFrame, 0, BufferLength);
                } while (bytesReadFromStream == 0);

                var responseData = ExtractDataFromFrame(responseFrame);

                return new CommandResponse(responseData, command);
            }

            throw new InvalidOperationException("The commander isn't connected to a device.");
        }

        public async Task<CommandResponse> SendCommandAsync(Command command)
        {
            if (_isConnected)
            {
                var frame = PutDataIntoFrame(command.GetRawCommand());

                var stream = _tcpClient.GetStream();

                await stream.WriteAsync(frame, 0, frame.Length);

                var responseFrame = new byte[BufferLength];

                await stream.ReadAsync(responseFrame, 0, BufferLength);

                var responseData = ExtractDataFromFrame(responseFrame);

                return new CommandResponse(responseData, command);
            }

            throw new InvalidOperationException("The commander isn't connected to a device.");
        }


        private byte[] PutDataIntoFrame(byte[] rawData)
        {
            // Every frame starts with two STX control signs
            var buffer = new List<byte>
            {
                STX, STX
            };

            int checkSum = 0;

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

            // Invert the checksum 
            checkSum *= -1;

            byte byteCheckSum = (byte)checkSum;

            // In case the checksum must be escaped
            if (IsControlSign(byteCheckSum))
            {
                buffer.Add(DLE);
            }

            // Second to last byte in frame is the checksum
            buffer.Add(byteCheckSum);
            // Frame is terminated with ETX control sign.
            buffer.Add(ETX);

            return buffer.ToArray();
        }

        private byte[] ExtractDataFromFrame(byte[] responseFrame)
        {
            var data = new List<byte>();

            bool isNextByteEscaped = false;
            int checkSum = 0;

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
                throw new ApplicationException("The checksum was wrong. Try again.");
            }

            return data.ToArray();
        }

        private static bool IsControlSign(byte byteToCheck)
        {
            return byteToCheck == STX || byteToCheck == ETX || byteToCheck == DLE;
        }

        public void Dispose()
        {
            _isConnected = false;

            _tcpClient.Dispose();
        }
    }
}
