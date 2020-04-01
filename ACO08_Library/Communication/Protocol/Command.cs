using System.Collections.Generic;
using System.Linq;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    internal class Command
    {
        public CommandHeader Header { get; }
        public List<byte> Body { get; set; } = new List<byte>();
        public Workmode AllowedWorkmodes { get; }

        public Command(CommandHeader header, Workmode allowedWorkmodes)
        {
            Header = header;
            AllowedWorkmodes = allowedWorkmodes;
        }

        public List<byte> GetRawCommand()
        {
            return Header.GetRawHeader().Concat(Body).ToList();
        }

        public Command Copy()
        {
            return new Command(Header.Copy(), AllowedWorkmodes)
            {
                Body = Body.ToList()
            };
        }

    }
}
