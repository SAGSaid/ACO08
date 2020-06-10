using System.Linq;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    /// <summary>
    /// Encapsulates the implementation details of the commands
    /// according to specification V4.1.5. This class is a singleton. 
    /// </summary>
    internal class CommandFactory
    {
        private readonly Command[] _commands;

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static CommandFactory Instance { get; } = new CommandFactory();
        
        /// <summary>
        /// Gets a copy of the command according to the parameter.
        /// </summary>
        /// <param name="commandId">The ID of the command</param>
        /// <returns>A copy of the requested command</returns>
        public Command GetCommand(CommandId commandId)
        {
            return _commands.First(comm => comm.Header.Id == commandId).Copy();
        }

        private CommandFactory()
        {
            _commands = new[]
            {
                new Command(
                    new CommandHeader(CommandId.GetVersion),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetWorkmode),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.SetWorkmodeMain),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.SetWorkmodeMeasure),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.SetWorkmodeReference),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.SetWorkmodeTest),
                    Workmode.Main | Workmode.Test),

                new Command(
                    new CommandHeader(CommandId.GetCrimpData),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.NextBlock),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetCrimpCounter),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetReferenceData),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetLowerEnvelope),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetUpperEnvelope),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.Reset),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetTime),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetTime),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetOptionList),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetOption),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetOption),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SaveSetup),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.ReferenceOk),
                    Workmode.ValidReference | 
                    Workmode.InvalidReference),

                new Command(
                    new CommandHeader(CommandId.ReferenceNotOk),
                    Workmode.ValidReference | 
                    Workmode.InvalidReference),

                new Command(
                    new CommandHeader(CommandId.GetForce),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetSerialNumber),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetDebugMessageTypes),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetDebugMessageTypes),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetLCD_Activation),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetLCD_Activation),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetGPIO_Function),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetGPIO_Function),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.ResetFactorySetup),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetGPIO_Status),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.ReadCrimpRecord),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetTrendList),
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetMultireference),
                    Workmode.Main |
                    Workmode.Measure |
                    Workmode.Reference |
                    Workmode.Test),

                new Command(
                    new CommandHeader(CommandId.GetMultireference),
                    Workmode.Main |
                    Workmode.Measure |
                    Workmode.Reference |
                    Workmode.Test),

                new Command(
                    new CommandHeader(CommandId.GetAreaDeviation),
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetMaxima),
                    Workmode.Main |
                    Workmode.Measure |
                    Workmode.Reference),

                new Command(
                    new CommandHeader(CommandId.GetMaxima),
                    Workmode.Main |
                    Workmode.Measure |
                    Workmode.Reference)
            };
        }
    }
}
