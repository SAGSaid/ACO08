using System.Collections.Generic;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    /// <summary>
    /// Encapsulates a command header and provides needed methods.
    /// </summary>
    internal class CommandHeader
    {
        public CommandId Id { get; }
        public Channel Channel { get; set; }
        public byte Extension1 { get; set; }
        public byte Extension2 { get; set; }

        public CommandHeader(CommandId id)
        {
            Id = id;
        }

        /// <summary>
        /// Provides a raw byte representation of the header.
        /// </summary>
        /// <returns>Byte representation of the header</returns>
        public List<byte> GetRawHeader()
        {
            return new List<byte>
            {
                (byte)Id,
                (byte)Channel,
                Extension1,
                Extension2
            };
        }
        
        /// <summary>
        /// Copies the instance.
        /// </summary>
        /// <returns>A copy of the instance</returns>
        public CommandHeader Copy()
        {
            return new CommandHeader(Id)
            {
                Channel = Channel,
                Extension1 = Extension1,
                Extension2 = Extension2
            };
        }
    }
}
