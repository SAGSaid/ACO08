using System;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    /// <summary>
    /// Encapsulates the response data and provides methods
    /// with protocol specifics for further introspection.
    /// </summary>
    internal class CommandResponse
    {
        private const byte ErrorMask = 0b1000_0000;

        public byte[] RawData { get; }
        public Command Request { get; }

        public CommandResponse(byte[] data, Command request)
        {
            RawData = data;
            Request = request;
        }

        public bool IsError
        {
            get { return (RawData[0] | ErrorMask) > 0; }
        }

        /// <summary>
        /// Extracts a properly formed header from the data.
        /// </summary>
        /// <returns></returns>
        public CommandHeader GetHeader()
        {
            // In case the error bit is set, it needs to be unset, so it's castable to the enum.
            byte commandId = IsError ? (byte)(RawData[0] ^ ErrorMask) : RawData[0];

            return new CommandHeader((CommandId)commandId)
            {
                Channel = (Channel)RawData[1],
                Extension1 = RawData[2],
                Extension2 = RawData[3]
            };
        }

        /// <summary>
        /// Extracts the body of the response.
        /// </summary>
        /// <returns>The body data or in case of no body an empty array</returns>
        public byte[] GetBody()
        {
            if (RawData.Length > 4)
            {
                var body = new byte[RawData.Length - 4];
                Array.Copy(RawData, 4, body, 0, RawData.Length - 4);
                return body;
            }
            else
            {
                return new byte[0];
            }
        }


    }
}
