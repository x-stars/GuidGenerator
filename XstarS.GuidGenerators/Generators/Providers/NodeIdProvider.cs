using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal abstract class NodeIdProvider
{
    private readonly RefreshingCache<byte[]> NodeIdBytesCache;

    private NodeIdProvider()
    {
        const int beforeSleep = 10;
        this.NodeIdBytesCache = new RefreshingCache<byte[]>(
            this.GetNodeIdBytes, this.RefreshPeriod, beforeSleep);
    }

    public byte[] NodeIdBytes => this.NodeIdBytesCache.Value;

    protected virtual int RefreshPeriod => 1 * 1000;

    protected abstract byte[] GetNodeIdBytes();

    internal sealed class RandomNumber : NodeIdProvider
    {
        private static volatile NodeIdProvider.RandomNumber? Singleton;

        internal RandomNumber() { }

        internal static NodeIdProvider.RandomNumber Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NodeIdProvider.RandomNumber Initialize()
                {
                    return NodeIdProvider.RandomNumber.Singleton ??=
                        new NodeIdProvider.RandomNumber();
                }

                return NodeIdProvider.RandomNumber.Singleton ?? Initialize();
            }
        }

        protected override int RefreshPeriod => Timeout.Infinite;

        protected override byte[] GetNodeIdBytes()
        {
            var nodeId = new byte[6];
            GlobalRandom.NextBytes(nodeId);
            nodeId[0] |= 0x01;
            return nodeId;
        }
    }

    internal sealed class MacAddress : NodeIdProvider
    {
        private static volatile NodeIdProvider.MacAddress? Singleton;

        private MacAddress() { }

        internal static NodeIdProvider.MacAddress Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NodeIdProvider.MacAddress Initialize()
                {
                    return NodeIdProvider.MacAddress.Singleton ??=
                        new NodeIdProvider.MacAddress();
                }

                return NodeIdProvider.MacAddress.Singleton ?? Initialize();
            }
        }

        protected override byte[] GetNodeIdBytes()
        {
            var validIface = this.GetValidNetworkInterface();
            return (validIface is null) ?
                NodeIdProvider.RandomNumber.Instance.NodeIdBytes :
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
