using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

        private DeviceCommander _commander;
        private bool _isUpdating = false;

        internal DeviceCommander Commander
        {
            get { return _commander; }
            set
            {
                _commander = value;
            }
        }

        public Option<bool>[] BoolOptions { get; }
        public Option<float>[] FloatOptions { get; }
        public Option<int>[] IntOptions { get; }

        internal ACO08_Options()
        {
            var factory = OptionFactory.Instance;

            BoolOptions = factory.CopyAllBoolOptions();
            FloatOptions = factory.CopyAllFloatOptions();
            IntOptions = factory.CopyAllIntOptions();

            BoolOptions.ForEach(option => option.PropertyChanged += OptionValueChangedHandler);
            FloatOptions.ForEach(option => option.PropertyChanged += OptionValueChangedHandler);
            IntOptions.ForEach(option => option.PropertyChanged += OptionValueChangedHandler);
        }

        public bool IsConnected
        {
            get { return Commander?.IsConnected ?? false; }
        }

        public void UpdateOptions()
        {
            _isUpdating = true;

            BoolOptions.ForEach(option => option.Value = GetBooleanOption(option.Id));
            FloatOptions.ForEach(option => option.Value = GetFloatOption(option.Id));
            IntOptions.ForEach(option => option.Value = GetIntOption(option.Id));

            _isUpdating = false;
        }

        public string GetOptionList()
        {
            CheckIsConnected();

            var command = CommandFactory.Instance.GetCommand(CommandId.GetOptionList);

            var response = Commander.SendCommandWithMultiPacketResponse(command);

            ThrowOnResponseError(response);

            return Encoding.Unicode.GetString(response.GetBody());
        }

        public void SaveSetup()
        {
            CheckIsConnected();

            var command = CommandFactory.Instance.GetCommand(CommandId.SaveSetup);

            var response = Commander.SendCommand(command);

            ThrowOnResponseError(response);
        }

        public bool GetBooleanOption(OptionId id)
        {
            CheckIsConnected();

            var command = CommandFactory.Instance.GetCommand(CommandId.GetOption);

            command.Header.Extension1 = (byte)id;

            var response = Commander.SendCommand(command);

            ThrowOnResponseError(response);

            // Is any of the bits in the body set?
            return response.GetBody().Any(b => b != 0);
        }

        public float GetFloatOption(OptionId id)
        {
            CheckIsConnected();

            var command = CommandFactory.Instance.GetCommand(CommandId.GetOption);

            command.Header.Extension1 = (byte)id;

            var response = Commander.SendCommand(command);

            ThrowOnResponseError(response);

            return BitConverter.ToSingle(response.GetBody(), 0);
        }

        public int GetIntOption(OptionId id)
        {
            CheckIsConnected();

            var command = CommandFactory.Instance.GetCommand(CommandId.GetOption);

            command.Header.Extension1 = (byte)id;

            var response = Commander.SendCommand(command);

            ThrowOnResponseError(response);

            return BitConverter.ToInt32(response.GetBody(), 0);
        }

        private void SetBooleanOption(OptionId id, bool value)
        {
            CheckIsConnected();

            var option = OptionFactory.Instance.CopyBoolOption(id);

            var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

            command.Header.Extension1 = (byte)option.Id;

            // The device expects 4 bytes for a boolean value 
            // (Probably just an int on the device)
            var data = value ? PaddedTrue : PaddedFalse;

            command.Body.AddRange(data);

            var response = Commander.SendCommand(command);

            ThrowOnResponseError(response);
        }

        private void SetFloatOption(OptionId id, float value)
        {
            CheckIsConnected();

            var option = OptionFactory.Instance.CopyFloatOption(id);

            var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

            command.Header.Extension1 = (byte)option.Id;

            var data = BitConverter.GetBytes(value);

            command.Body.AddRange(data);

            var response = Commander.SendCommand(command);

            ThrowOnResponseError(response);
        }

        private void SetIntOption(OptionId id, int value)
        {
            CheckIsConnected();

            var option = OptionFactory.Instance.CopyIntOption(id);

            var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

            command.Header.Extension1 = (byte)option.Id;

            var data = BitConverter.GetBytes(value);

            command.Body.AddRange(data);

            var response = Commander.SendCommand(command);

            ThrowOnResponseError(response);
        }

        private void CheckIsConnected()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("The device is not connected.");
            }
        }

        private static void ThrowOnResponseError(CommandResponse response)
        {
            if (response.IsError)
            {
                throw new ACO08_Exception((ErrorId)response.GetBody().Last());
            }
        }

        private void OptionValueChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value" && !_isUpdating)
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
                        throw new NotSupportedException("Unexpected sender type.");
                }
            }
        }

    }
}
