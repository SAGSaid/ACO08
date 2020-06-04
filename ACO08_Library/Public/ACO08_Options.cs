using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACO08_Library.Communication.Networking.DeviceInterfacing;
using ACO08_Library.Communication.Protocol;
using ACO08_Library.Enums;

namespace ACO08_Library.Public
{
    public class ACO08_Options
    {
        // For some options the device expects an int for what is essentially a bool.
        private static readonly byte[] PaddedTrue = { 1, 0, 0, 0 };
        private static readonly byte[] PaddedFalse = { 0, 0, 0, 0 };

        internal DeviceCommander Commander { get; set; }

        public Option<bool>[] BoolOptions { get; }
        public Option<float>[] FloatOptions { get; }
        public Option<int>[] IntOptions { get; }

        internal ACO08_Options()
        {
            var factory = OptionFactory.Instance;

            BoolOptions = factory.GetAllBoolOptions();
            FloatOptions = factory.GetAllFloatOptions();
            IntOptions = factory.GetAllIntOptions();
        }

        public bool IsConnected
        {
            get { return Commander?.IsConnected ?? false; }
        }

        public string GetOptionList()
        {
            if (IsConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.GetOptionList);

                var response = Commander.SendCommandWithMultiPacketResponse(command);

                if (response.IsError)
                {
                    return string.Empty;
                }

                return Encoding.Unicode.GetString(response.GetBody());
            }

            throw new InvalidOperationException("The device is not connected.");
        }



        public bool SaveSetup()
        {
            if (IsConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.SaveSetup);

                var response = Commander.SendCommand(command);

                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        public bool GetBooleanOption(OptionId id)
        {
            if (IsConnected)
            {
                var command = CommandFactory.Instance.GetCommand(CommandId.GetOption);

                command.Header.Extension1 = (byte)id;

                var response = Commander.SendCommand(command);

                if (!response.IsError)
                {
                    // Is any of the bytes in the body set?
                    return response.GetBody().Any(b => b != 0);
                }
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        public bool SetOptionEnableInternalTrigger(bool value)
        {
            return SetBooleanOption(OptionId.EnableInternalTrigger, value);
        }

        private bool SetBooleanOption(OptionId id, bool value)
        {
            if (IsConnected)
            {
                var option = OptionFactory.Instance.GetBoolOption(id);

                option.Value = value;

                var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

                command.Header.Extension1 = (byte)option.Id;

                // The device expects 4 bytes for a boolean value 
                // (Probably just an int on the device)
                var body = value ? PaddedTrue : PaddedFalse;

                command.Body.AddRange(body);

                var response = Commander.SendCommand(command);

                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");
        }
    }
}
