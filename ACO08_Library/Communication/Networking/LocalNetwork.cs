using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ACO08_Library.Communication.Networking
{
    public static class LocalNetwork
    {
        // It is expected that communication only permeates over local /24 subnets
        private static readonly byte[] Slash24SubnetMask = {255, 255, 255, 0};

        /// <summary>
        /// Gets all IP-addresses of the system's network adapters that interface with Ethernet or Wireless Ethernet.
        /// </summary>
        /// <returns>Sequence of found IP-addresses</returns>
        public static IEnumerable<IPAddress> GetAdapterAdresses()
        {
            return GetLocalIPAddresses()
                .Select(a => a.Address);
        }

        public static IPAddress GetSubnetMask(IPAddress address)
        {
            return GetLocalIPAddresses()
                .First(add => add.Address.Equals(address))
                .IPv4Mask;
        }

        public static bool IsSubnetMaskSlash24(IPAddress address)
        {
            var localMask = GetSubnetMask(address);

            return localMask.Equals(Slash24SubnetMask);
        }

        private static IEnumerable<UnicastIPAddressInformation> GetLocalIPAddresses()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                            i.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}
