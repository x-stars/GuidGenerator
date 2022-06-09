using System;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal abstract class NodeIdProvider
    {
        private NodeIdProvider() { }

        public abstract byte[] GetNodeIdBytes();

        internal sealed class RandomNumber : NodeIdProvider
        {
            private static class Singleton
            {
                internal static readonly NodeIdProvider.RandomNumber Value =
                    new NodeIdProvider.RandomNumber();
            }

            private RandomNumber() { }

            internal static NodeIdProvider.RandomNumber Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => NodeIdProvider.RandomNumber.Singleton.Value;
            }

            public override byte[] GetNodeIdBytes()
            {
                var nodeId = new byte[6];
                new Random().NextBytes(nodeId);
                nodeId[0] |= 0x01;
                return nodeId;
            }
        }

        internal sealed class MacAddress : NodeIdProvider
        {
            private static class Singleton
            {
                internal static readonly NodeIdProvider.MacAddress Value =
                    new NodeIdProvider.MacAddress();
            }

            private MacAddress() { }

            internal static NodeIdProvider.MacAddress Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => NodeIdProvider.MacAddress.Singleton.Value;
            }

            public override byte[] GetNodeIdBytes()
            {
                var validIface = this.GetValidNetworkInterface();
                return (validIface is null) ?
                    NodeIdProvider.RandomNumber.Instance.GetNodeIdBytes() :
                    validIface.GetPhysicalAddress().GetAddressBytes();
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
