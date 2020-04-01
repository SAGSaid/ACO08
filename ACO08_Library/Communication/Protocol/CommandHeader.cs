using System.Collections.Generic;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    internal class CommandHeader
    {
        public CommandId CommandId { get; }
        public Channel Channel { get; set; }
        public byte Extension1 { get; set; }
        public byte Extension2 { get; set; }

        public CommandHeader(CommandId commandId)
        {
            CommandId = commandId;
        }

        public List<byte> GetRawHeader()
        {
            return new List<byte>
            {
                (byte)CommandId,
                (byte)Channel,
                Extension1,
                Extension2
            };
        }
        
        public CommandHeader Copy()
        {
            return new CommandHeader(CommandId)
            {
                Channel = Channel,
                Extension1 = Extension1,
                Extension2 = Extension2
            };
        }
    }
}
