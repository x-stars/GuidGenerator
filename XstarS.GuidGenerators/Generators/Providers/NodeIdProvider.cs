using System;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

internal abstract class NodeIdProvider
{
    private NodeIdProvider() { }

    public abstract byte[] NodeIdBytes { get; }

    public virtual bool IsNonVolatile => true;

    internal class RandomNumber : NodeIdProvider
    {
        private static volatile NodeIdProvider.RandomNumber? Singleton;

        private readonly byte[] RandomNodeIdBytes;

        private RandomNumber()
        {
            this.RandomNodeIdBytes =
                NodeIdProvider.RandomNumber.CreateNodeIdBytes();
        }

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

        public override byte[] NodeIdBytes => this.RandomNodeIdBytes;

        public override bool IsNonVolatile => false;

        internal static NodeIdProvider.RandomNumber Create()
        {
            return new NodeIdProvider.RandomNumber();
        }

        private static unsafe byte[] CreateNodeIdBytes()
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

            public override byte[] NodeIdBytes =>
                GuidGeneratorState.RandomNodeIdBytes ?? this.RandomNodeIdBytes;

            public override bool IsNonVolatile => true;
        }
    }

    internal sealed class MacAddress : NodeIdProvider
    {
        private static volatile NodeIdProvider.MacAddress? Singleton;

        private readonly AutoRefreshCache<byte[]> NodeIdBytesCache;

        private MacAddress()
        {
            this.NodeIdBytesCache = new AutoRefreshCache<byte[]>(
                this.GetNodeIdBytes, refreshPeriod: 1 * 1000, sleepAfter: 10);
        }

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

        public override byte[] NodeIdBytes => this.NodeIdBytesCache.Value;

        private byte[] GetNodeIdBytes()
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
