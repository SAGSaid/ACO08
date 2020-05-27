using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ACO08_Library.Communication.Networking
{
    internal class NetworkInterfaceSelector
    {
        public static readonly IPAddress Slash24SubnetMask = new IPAddress(new byte[]{255, 255, 255, 0});

        private List<UnicastIPAddressInformation> _localIpAddressInfos;

        public NetworkInterfaceSelector()
        {
            RefreshLocalIpAddressInfos();
        }

        public IPAddress GetLocalIpAddressInSameSubnet(IPAddress addressToReach, IPAddress subnetMask)
        {
            RefreshLocalIpAddressInfos();

            var availableIpAddresses = _localIpAddressInfos.Select(addr => addr.Address);

            var addressInSameSubnet =
                availableIpAddresses.FirstOrDefault(addr => addr.IsInSameSubnet(addressToReach, subnetMask));

            return addressInSameSubnet;
        }

        private void RefreshLocalIpAddressInfos()
        {
            _localIpAddressInfos = NetworkInterface.GetAllNetworkInterfaces()
                .Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                            i.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                .ToList();
        }

    }
}
