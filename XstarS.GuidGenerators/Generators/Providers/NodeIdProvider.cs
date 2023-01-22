using System;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal abstract class NodeIdProvider
{
    private readonly AutoRefreshCache<byte[]> NodeIdBytesCache;

    private NodeIdProvider()
    {
        const int sleepAfter = 10;
        this.NodeIdBytesCache = new AutoRefreshCache<byte[]>(
            this.GetNodeIdBytes, this.RefreshPeriod, sleepAfter);
    }

    public byte[] NodeIdBytes => this.NodeIdBytesCache.Value;

    public virtual bool IsNonVolatile => true;

    protected virtual int RefreshPeriod => 1 * 1000;

    protected abstract byte[] GetNodeIdBytes();

    internal class RandomNumber : NodeIdProvider
    {
        private static volatile NodeIdProvider.RandomNumber? Singleton;

        private RandomNumber() { }

        internal static NodeIdProvider.RandomNumber Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NodeIdProvider.RandomNumber Initialize()
                {
                    return NodeIdProvider.RandomNumber.Singleton ??=
                        new NodeIdProvider.RandomNumber.NonVolatile();
                }

                return NodeIdProvider.RandomNumber.Singleton ?? Initialize();
            }
        }

        public override bool IsNonVolatile => false;

        protected override int RefreshPeriod => Timeout.Infinite;

        internal static NodeIdProvider.RandomNumber Create()
        {
            return new NodeIdProvider.RandomNumber();
        }

        protected override unsafe byte[] GetNodeIdBytes()
        {
            const int size = 6;
            var newGuid = Guid.NewGuid();
            var nodeId = new byte[size];
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            newGuid.NodeId().CopyTo((Span<byte>)nodeId);
#else
            fixed (byte* pGuidNodeId = &newGuid.NodeId(0), pNodeId = &nodeId[0])
            {
                Buffer.MemoryCopy(pGuidNodeId, pNodeId, size, size);
            }
#endif
            nodeId[0] |= 0x01;
            return nodeId;
        }

        private sealed class NonVolatile : NodeIdProvider.RandomNumber
        {
            internal NonVolatile() { }

            public override bool IsNonVolatile => true;

            protected override byte[] GetNodeIdBytes()
            {
                var nodeId = GuidGeneratorState.RandomNodeIdBytes;
                nodeId ??= base.GetNodeIdBytes();
                nodeId[0] |= 0x01;
                return nodeId;
            }
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
