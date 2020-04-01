using System.Linq;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    internal class CommandFactory
    {
        private readonly Command[] _commands;

        public static CommandFactory Instance { get; } = new CommandFactory();
        

        public Command GetCommand(CommandId commandId)
        {
            return _commands.First(comm => comm.Header.Id == commandId).Copy();
        }

        private CommandFactory()
        {
            _commands = new[]
            {
                new Command(new CommandHeader(CommandId.GetVersion),
                    Workmode.All)

                // TODO implement templates for all commands
            };
        }
    }
}
