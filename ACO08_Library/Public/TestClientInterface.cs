using System.Collections.ObjectModel;
using System.Linq;
using ACO08_Library.Communication.Networking;

namespace ACO08_Library.Public
{
    /// <summary>
    /// Public interface specifically made for the TestClient
    /// </summary>
    public class TestClientInterface
    {
        private DeviceLocator _locator;

        public ObservableCollection<ACO08_Device> Devices { get; } = 
            new ObservableCollection<ACO08_Device>();

        /// <summary>
        /// Starts the location process
        /// </summary>
        public void StartLocatingDevices()
        {
            if (_locator == null)
            {
                _locator = new DeviceLocator();
                _locator.DeviceLocated += DeviceLocatedHandler;
                _locator.StartLocating(); 
            }
        }

        /// <summary>
        /// Stops the location process
        /// </summary>
        public void StopLocatingDevices()
        {
            if (_locator != null)
            {
                _locator.StopLocating();
                _locator.DeviceLocated -= DeviceLocatedHandler;
                _locator.Dispose();
                _locator = null; 
            }
        }

        private void DeviceLocatedHandler(object sender, DeviceLocatedEventArgs args)
        {
            // The serial numbers are expected to be unique, so we can identify devices with them
            if (Devices.All(dev => dev.SerialNumber != args.SerialNumber))
            {
                Devices.Add(new ACO08_Device(args.SerialNumber, args.Address));
            }
        }

    }
}
