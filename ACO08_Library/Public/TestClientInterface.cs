using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ACO08_Library.Communication.Networking;

namespace ACO08_Library.Public
{
    public class TestClientInterface
    {
        public ObservableCollection<ACO08_Device> Devices { get; } = 
            new ObservableCollection<ACO08_Device>();


        public async Task BeginLocatingDevices(CancellationToken token)
        {
            var locator = new DeviceLocator();

            try
            {
                locator.DeviceLocated += DeviceLocatedHandler;

                locator.StartLocating();

                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(100, token);
                }
            }
            catch (Exception)
            {
                // In case something goes wrong,
                // we don't want that to screw the consumer of the method
            }
            finally
            {
                locator.StopLocating();
                locator.DeviceLocated -= DeviceLocatedHandler;
                locator.Dispose();
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
