using System.Collections.Generic;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    internal class CommandFactory
    {
        private List<Command> _commands;

        public static CommandFactory Instance { get; } = new CommandFactory();

        private CommandFactory()
        {
            InitializeAllCommands();
        }

        public Command GetCommand(CommandId commandId)
        {
            return _commands.Find(comm => comm.Header.CommandId == commandId).Copy();
        }

        private void InitializeAllCommands()
        {
            _commands = new List<Command>
            {
                // TODO implement templates for all commands
            };
        }
    }
}
