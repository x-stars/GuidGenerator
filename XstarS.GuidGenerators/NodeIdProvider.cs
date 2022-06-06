using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal abstract class NodeIdProvider
    {
        private static class Singleton
        {
            internal static readonly NodeIdProvider Value =
                new NodeIdProvider.MacAddress();
        }

        protected NodeIdProvider() { }

        internal static NodeIdProvider Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => NodeIdProvider.Singleton.Value;
        }

        public abstract byte[] GetNodeIdBytes();

        private sealed class MacAddress : NodeIdProvider
        {
            internal MacAddress() { }

            public override byte[] GetNodeIdBytes()
            {
                var validIface = this.GetValidNetworkInterface();
                if (validIface is null) { return new byte[6]; }
                return validIface.GetPhysicalAddress().GetAddressBytes();
            }

            private NetworkInterface? GetValidNetworkInterface()
            {
                var validIface = default(NetworkInterface);
                var upValidIface = default(NetworkInterface);
                var ifaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var iface in ifaces)
                {
                    if (this.IsValidNetworkInterface(iface))
                    {
                        validIface ??= iface;
                        var ifaceStatus = iface.OperationalStatus;
                        if (ifaceStatus == OperationalStatus.Up)
                        {
                            upValidIface ??= iface;
                        }
                    }
                }
                return upValidIface ?? validIface;
            }

            private bool IsValidNetworkInterface(NetworkInterface iface)
            {
                var ifaceType = iface.NetworkInterfaceType;
                var macAddress = iface.GetPhysicalAddress();
                return (ifaceType != NetworkInterfaceType.Loopback) &&
                       (ifaceType != NetworkInterfaceType.Tunnel) &&
                       (macAddress.GetAddressBytes().Length > 0);
            }
        }
    }
}
