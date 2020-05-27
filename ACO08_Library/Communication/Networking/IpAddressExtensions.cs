using System.Net;

namespace ACO08_Library.Communication.Networking
{
    public static class IpAddressExtensions
    {
        /// <summary>
        /// Network address refers to the address you get when you AND
        /// a subnet mask with an IP address.
        /// E.g. address 192.168.1.1 with subnet 255.255.255.0 results in the network address 192.168.1.0
        /// This is important because only devices with the same network address can directly reach each other
        /// without the need for a gateway (router).
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

        /// <summary>
        /// Checks if both addresses are in the same network, based on the subnet mask.
        /// </summary>
        public static bool IsInSameSubnet(this IPAddress address1, IPAddress address2, IPAddress subnetMask)
        {
            var network1 = address1.GetNetworkAddress(subnetMask);
            var network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }
    }
}
