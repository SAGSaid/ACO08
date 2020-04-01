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
                new Command(
                    new CommandHeader(CommandId.GetVersion)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetWorkmode)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.SetWorkmodeMain)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.SetWorkmodeMeasure)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.SetWorkmodeReference)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.SetWorkmodeTest)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.Main | Workmode.Test),

                new Command(
                    new CommandHeader(CommandId.GetCrimpData)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.NextBlock)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetCrimpCounter)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetReferenceData)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetLowerEnvelope)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetUpperEnvelope)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.Reset)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetTime)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetTime)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetOptionList)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetOption)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetOption)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SaveSetup)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.ReferenceOk)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.ValidReference | 
                    Workmode.InvalidReference),

                new Command(
                    new CommandHeader(CommandId.ReferenceNotOk)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.ValidReference | 
                    Workmode.InvalidReference),

                new Command(
                    new CommandHeader(CommandId.GetForce)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetSerialNumber)
                    {
                        Channel = Channel.None
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetDebugMessageTypes)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetDebugMessageTypes)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetLCD_Activation)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetLCD_Activation)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetGPIO_Function)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetGPIO_Function)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.ResetFactorySetup)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetGPIO_Status)
                    {
                        Channel = Channel.None
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.ReadCrimpRecord)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.GetTrendList)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.Main),

                new Command(
                    new CommandHeader(CommandId.SetMultireference)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main |
                    Workmode.Measure |
                    Workmode.Reference |
                    Workmode.Test),

                new Command(
                    new CommandHeader(CommandId.GetMultireference)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main |
                    Workmode.Measure |
                    Workmode.Reference |
                    Workmode.Test),

                new Command(
                    new CommandHeader(CommandId.GetAreaDeviation)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.All),

                new Command(
                    new CommandHeader(CommandId.GetMaxima)
                    {
                        Channel = Channel.Channel1
                    },
                    Workmode.Main |
                    Workmode.Measure |
                    Workmode.Reference),

                new Command(
                    new CommandHeader(CommandId.GetMaxima)
                    {
                        Channel = Channel.None
                    },
                    Workmode.Main |
                    Workmode.Measure |
                    Workmode.Reference)
            };
        }
    }
}
