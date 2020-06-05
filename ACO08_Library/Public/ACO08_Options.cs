using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACO08_Library.Communication.Networking.DeviceInterfacing;
using ACO08_Library.Communication.Protocol;
using ACO08_Library.Enums;
using ACO08_Library.Tools;

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

            BoolOptions.ForEach(option => option.PropertyChanged += OptionValueChangedHandler);
            FloatOptions.ForEach(option => option.PropertyChanged += OptionValueChangedHandler);
            IntOptions.ForEach(option => option.PropertyChanged += OptionValueChangedHandler);
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

        private bool SetBooleanOption(OptionId id, bool value)
        {
            if (IsConnected)
            {
                var option = OptionFactory.Instance.CopyBoolOption(id);

                var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

                command.Header.Extension1 = (byte)option.Id;

                // The device expects 4 bytes for a boolean value 
                // (Probably just an int on the device)
                var data = value ? PaddedTrue : PaddedFalse;

                command.Body.AddRange(data);

                var response = Commander.SendCommand(command);

                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        private bool SetFloatOption(OptionId id, float value)
        {
            if (IsConnected)
            {
                var option = OptionFactory.Instance.CopyFloatOption(id);

                var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

                command.Header.Extension1 = (byte)option.Id;

                var data = BitConverter.GetBytes(value);

                command.Body.AddRange(data);

                var response = Commander.SendCommand(command);

                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");
        }

        private bool SetIntOption(OptionId id, int value)
        {
            if (IsConnected)
            {
                var option = OptionFactory.Instance.CopyIntOption(id);

                var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

                command.Header.Extension1 = (byte)option.Id;

                var data = BitConverter.GetBytes(value);

                command.Body.AddRange(data);

                var response = Commander.SendCommand(command);

                return !response.IsError;
            }

            throw new InvalidOperationException("The device is not connected.");

        }

        private void OptionValueChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                switch (sender)
                {
                    case Option<bool> option:
                        SetBooleanOption(option.Id, option.Value);
                        break;

                    case Option<float> option:
                        SetFloatOption(option.Id, option.Value);
                        break;

                    case Option<int> option:
                        SetIntOption(option.Id, option.Value);
                        break;

                    default:
                        throw new InvalidOperationException("Unexpected sender type.");
                }
            }
        }
        

    }
}
