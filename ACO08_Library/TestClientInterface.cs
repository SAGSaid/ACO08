using System;
using System.Threading.Tasks;
using ACO08_Library.Communication.Networking;
using ACO08_Library.Communication.Protocol;
using ACO08_Library.Data;
using ACO08_Library.Enums;

namespace ACO08_Library
{
    public sealed class TestClientInterface : IDisposable
    {
        private DeviceLocatedEventArgs _deviceInfo;

        private DeviceEventListener _listener;
        private DeviceCommander _commander;

        public event EventHandler<CrimpDataReceivedEventArgs> CrimpDataReceived;

        public async Task<bool> Start()
        {
            try
            {
                // Locate the device
                using (var locator = new DeviceLocator())
                {
                    locator.DeviceLocated += DeviceLocatedHandler;

                    locator.StartLocating();

                    while (_deviceInfo == null)
                    {
                        await Task.Delay(100);
                    }

                    locator.StopLocating();

                    locator.DeviceLocated -= DeviceLocatedHandler;
                }

                _listener = new DeviceEventListener(_deviceInfo.EndPoint.Address);



                _listener.StartListening();

                _commander = new DeviceCommander(_deviceInfo.EndPoint.Address);

                if (!_commander.Connect())
                {
                    throw new Exception("Connection failed.");
                }
                
                _listener.CrimpDataChanged += CrimpDataChangedHandler;

                // Go to workmode main, so the inital state is consistent.
                var workmodeModeCommand = CommandFactory.Instance.GetCommand(CommandId.SetWorkmodeMain);
                var response = _commander.SendCommand(workmodeModeCommand);

                if (response.IsError)
                {
                    throw new Exception("Setting the initial workmode to main failed.");
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void DeviceLocatedHandler(object sender, DeviceLocatedEventArgs args)
        {
            _deviceInfo = args;
        }

        private void CrimpDataChangedHandler(object sender, EventArgs e)
        {
            var getCrimpData = CommandFactory.Instance.GetCommand(CommandId.GetCrimpData);

            var response = _commander.SendCommand(getCrimpData);

            if (!response.IsError)
            {
                var crimpData = new CrimpData(response.GetBody());

                OnCrimpDataReceived(new CrimpDataReceivedEventArgs(crimpData));
            }
        }

        private void OnCrimpDataReceived(CrimpDataReceivedEventArgs args)
        {
            CrimpDataReceived?.Invoke(this, args);
        }

        public void Dispose()
        {
            _listener?.Dispose();
            _commander?.Dispose();
        }
    }
}
