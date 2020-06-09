using System;
using System.Collections.Generic;
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

        private readonly List<Option<bool>> _changedBoolOptions = 
            new List<Option<bool>>();
        private readonly List<Option<float>> _changedFloatOptions = 
            new List<Option<float>>();
        private readonly List<Option<int>> _changedIntOptions = 
            new List<Option<int>>();

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

        public void GetAll()
        {
            _isUpdating = true;

            ResetChanges();
            
            BoolOptions.ForEach(option => option.Value = GetBooleanOption(option.Id));
            FloatOptions.ForEach(option => option.Value = GetFloatOption(option.Id));
            IntOptions.ForEach(option => option.Value = GetIntOption(option.Id));

            _isUpdating = false;
        }

        public void SetChangedOptions()
        {
            _changedBoolOptions.ForEach(option => SetBooleanOption(option.Id, option.Value));
            _changedFloatOptions.ForEach(option => SetFloatOption(option.Id, option.Value));
            _changedIntOptions.ForEach(option => SetIntOption(option.Id, option.Value));

            ResetChanges();
        }

        public string GetOptionList()
        {
            var command = CommandFactory.Instance.GetCommand(CommandId.GetOptionList);

            var response = Commander.SendCommandWithMultiPacketResponse(command);

            ACO08_Exception.ThrowOnResponseError(response);

            return Encoding.Unicode.GetString(response.GetBody());
        }

        public void SaveSetup()
        {
            var command = CommandFactory.Instance.GetCommand(CommandId.SaveSetup);

            var response = Commander.SendCommand(command);

            ACO08_Exception.ThrowOnResponseError(response);
        }

        public bool GetBooleanOption(OptionId id)
        {
            var command = CommandFactory.Instance.GetCommand(CommandId.GetOption);

            command.Header.Extension1 = (byte)id;

            var response = Commander.SendCommand(command);

            ACO08_Exception.ThrowOnResponseError(response);

            // Is any of the bits in the body set?
            return response.GetBody().Any(b => b != 0);
        }

        public float GetFloatOption(OptionId id)
        {
            var command = CommandFactory.Instance.GetCommand(CommandId.GetOption);

            command.Header.Extension1 = (byte)id;

            var response = Commander.SendCommand(command);

            ACO08_Exception.ThrowOnResponseError(response);

            return BitConverter.ToSingle(response.GetBody(), 0);
        }

        public int GetIntOption(OptionId id)
        {
            var command = CommandFactory.Instance.GetCommand(CommandId.GetOption);

            command.Header.Extension1 = (byte)id;

            var response = Commander.SendCommand(command);

            ACO08_Exception.ThrowOnResponseError(response);

            return BitConverter.ToInt32(response.GetBody(), 0);
        }

        private void SetBooleanOption(OptionId id, bool value)
        {
            var option = OptionFactory.Instance.CopyBoolOption(id);

            var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

            command.Header.Extension1 = (byte)option.Id;

            // The device expects 4 bytes for a boolean value 
            // (Probably just an int on the device)
            var data = value ? PaddedTrue : PaddedFalse;

            command.Body.AddRange(data);

            var response = Commander.SendCommand(command);

            ACO08_Exception.ThrowOnResponseError(response);
        }

        private void SetFloatOption(OptionId id, float value)
        {
            var option = OptionFactory.Instance.CopyFloatOption(id);

            var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

            command.Header.Extension1 = (byte)option.Id;

            var data = BitConverter.GetBytes(value);

            command.Body.AddRange(data);

            var response = Commander.SendCommand(command);

            ACO08_Exception.ThrowOnResponseError(response);
        }

        private void SetIntOption(OptionId id, int value)
        {
            var option = OptionFactory.Instance.CopyIntOption(id);

            var command = CommandFactory.Instance.GetCommand(CommandId.SetOption);

            command.Header.Extension1 = (byte)option.Id;

            var data = BitConverter.GetBytes(value);

            command.Body.AddRange(data);

            var response = Commander.SendCommand(command);

            ACO08_Exception.ThrowOnResponseError(response);
        }
        
        private void ResetChanges()
        {
            _changedBoolOptions.Clear();
            _changedFloatOptions.Clear();
            _changedIntOptions.Clear();
        }

        private void OptionValueChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value" && !_isUpdating)
            {
                switch (sender)
                {
                    case Option<bool> option:
                        _changedBoolOptions.Add(option);
                        break;

                    case Option<float> option:
                        _changedFloatOptions.Add(option);
                        break;

                    case Option<int> option:
                        _changedIntOptions.Add(option);
                        break;

                    default:
                        throw new NotSupportedException("Unexpected sender type.");
                }
            }
        }

    }
}
