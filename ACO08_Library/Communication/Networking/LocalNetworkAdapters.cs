using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ACO08_Library.Communication.Networking
{
    public static class LocalNetworkAdapters
    {
        // It is expected that communication only permeates over local /24 subnets
        private static readonly byte[] Slash24SubnetMaskBytes = {255, 255, 255, 0};
        public static readonly IPAddress Slash24SubnetMask = new IPAddress(Slash24SubnetMaskBytes);

        /// <summary>
        /// Gets all IP-addresses of the system's network adapters that interface with Ethernet or Wireless Ethernet.
        /// </summary>
        /// <returns>Sequence of found IP-addresses</returns>
        public static IEnumerable<IPAddress> GetIpAddresses()
        {
            var interfaces = GetEthernetInterfaces();

            return interfaces.Select(a => a.Address);
        }

        public static IPAddress GetSubnetMask(IPAddress address)
        {
            return GetEthernetInterfaces()
                .First(add => add.Address.Equals(address))
                .IPv4Mask;
        }

        public static bool IsSubnetMaskSlash24(IPAddress address)
        {
            var localMask = GetSubnetMask(address);

            return localMask.Equals(Slash24SubnetMask);
        }

        /// <summary>
        /// Network address refers to the address you get when you AND
        /// a subnet mask with an IP address.
        /// E.g. address 192.168.1.1 with subnet 255.255.255.0 results in the network address 192.168.1.0
        /// This is important because only devices with the same network address can directly reach each other
        /// without a need for a gateway (router).
        /// </summary>
        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            var ipAdressBytes = address.GetAddressBytes();
            var subnetMaskBytes = subnetMask.GetAddressBytes();

            var networkAddress = new byte[ipAdressBytes.Length];
            
            for (int i = 0; i < networkAddress.Length; i++)
            {
                networkAddress[i] = (byte)(ipAdressBytes[i] & subnetMaskBytes[i]);
            }
            return new IPAddress(networkAddress);
        }

        public static bool IsInSameSubnet(this IPAddress address, IPAddress address2, IPAddress subnetMask)
        {
            var network1 = address.GetNetworkAddress(subnetMask);
            var network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }
    
        private static IEnumerable<UnicastIPAddressInformation> GetEthernetInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                            i.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}
