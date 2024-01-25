using System;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using XNetEx.Threading;

namespace XNetEx.Guids.Generators;

internal abstract class NodeIdProvider
{
    private NodeIdProvider() { }

    public abstract byte[] NodeIdBytes { get; }

    public abstract NodeIdSource SourceType { get; }

    internal static NodeIdProvider GetInstance(NodeIdSource source) => source switch
    {
        NodeIdSource.None => NodeIdProvider.Nothing.Instance,
        NodeIdSource.PhysicalAddress => NodeIdProvider.PhysicalAddress.Instance,
        NodeIdSource.VolatileRandom => NodeIdProvider.RandomNumber.Create(),
        NodeIdSource.NonVolatileRandom => NodeIdProvider.RandomNumber.Instance,
        _ => throw new ArgumentOutOfRangeException(nameof(source)),
    };

    private sealed class Nothing : NodeIdProvider
    {
        private static volatile NodeIdProvider.Nothing? Singleton;

        private Nothing() { }

        internal static NodeIdProvider.Nothing Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NodeIdProvider.Nothing Initialize()
                {
                    return NodeIdProvider.Nothing.Singleton ??=
                        new NodeIdProvider.Nothing();
                }

                return NodeIdProvider.Nothing.Singleton ?? Initialize();
            }
        }

        public override byte[] NodeIdBytes =>
            throw new NotSupportedException(
                "Cannot get the node ID value for NodeIdSource.None.");

        public override NodeIdSource SourceType => NodeIdSource.None;
    }

    private sealed class PhysicalAddress : NodeIdProvider
    {
        private static volatile NodeIdProvider.PhysicalAddress? Singleton;

        private readonly AutoRefreshCache<byte[]> NodeIdBytesCache;

        private PhysicalAddress()
        {
            this.NodeIdBytesCache = new AutoRefreshCache<byte[]>(
                this.GetNodeIdBytes, refreshPeriod: 1 * 1000, sleepAfter: 10);
        }

        internal static NodeIdProvider.PhysicalAddress Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NodeIdProvider.PhysicalAddress Initialize()
                {
                    return NodeIdProvider.PhysicalAddress.Singleton ??=
                        new NodeIdProvider.PhysicalAddress();
                }

                return NodeIdProvider.PhysicalAddress.Singleton ?? Initialize();
            }
        }

        public override byte[] NodeIdBytes => this.NodeIdBytesCache.Value;

        public override NodeIdSource SourceType => NodeIdSource.PhysicalAddress;

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
            var ifaces = Environment.OSVersion.Platform is PlatformID.Win32NT ?
                SystemNetworkInterface.GetAllNetworkInterfaces() :
                NetworkInterface.GetAllNetworkInterfaces();
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
                   (macAddress.GetAddressBytes().Length == 6);
        }
    }

    private class RandomNumber : NodeIdProvider
    {
        private static volatile NodeIdProvider.RandomNumber? Singleton;

        private readonly byte[] NodeIdBytesValue;

        private RandomNumber()
        {
            this.NodeIdBytesValue =
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

        public override byte[] NodeIdBytes => this.NodeIdBytesValue;

        public override NodeIdSource SourceType => NodeIdSource.VolatileRandom;

        internal static NodeIdProvider.RandomNumber Create()
        {
            return new NodeIdProvider.RandomNumber();
        }

        private static unsafe byte[] CreateNodeIdBytes()
        {
            const int nodeIdSize = 6;
            var newGuid = Guid.NewGuid();
            var nodeId = new byte[nodeIdSize];
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            newGuid.NodeId().CopyTo((Span<byte>)nodeId);
#else
            fixed (byte* pGuidNodeId = &newGuid.NodeId(0), pNodeId = &nodeId[0])
            {
                Buffer.MemoryCopy(pGuidNodeId, pNodeId, nodeIdSize, nodeIdSize);
            }
#endif
            nodeId[0] |= 0x01;
            return nodeId;
        }

        private sealed class NonVolatile : NodeIdProvider.RandomNumber
        {
            internal NonVolatile() { }

            public override byte[] NodeIdBytes =>
                GuidGeneratorState.RandomNodeId ?? this.NodeIdBytesValue;

            public override NodeIdSource SourceType => NodeIdSource.NonVolatileRandom;
        }
    }
}
