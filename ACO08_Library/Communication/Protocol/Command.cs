using System.Collections.Generic;
using System.Linq;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    /// <summary>
    /// Encapsulates the data of a command and provides needed methods.
    /// </summary>
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

        /// <summary>
        /// Provides the raw byte array that represents the command.
        /// </summary>
        /// <returns>Raw byte array of the command</returns>
        public byte[] GetRawCommand()
        {
            return Header.GetRawHeader().Concat(Body).ToArray();
        }

        /// <summary>
        /// Provides a deep copy of the instance.
        /// </summary>
        /// <returns>A deep copy of the command instance</returns>
        public Command Copy()
        {
            return new Command(Header.Copy(), AllowedWorkmodes)
            {
                Body = Body.ToList()
            };
        }

    }
}
