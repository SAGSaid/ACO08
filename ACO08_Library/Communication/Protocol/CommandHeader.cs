using System.Collections.Generic;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
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
