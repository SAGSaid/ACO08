using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    internal class CommandHeader
    {
        public CommandId CommandId { get; }
        public Channel Channel { get; set; }
        public byte Extension1 { get; set; }
        public byte Extension2 { get; set; }
    }
}
