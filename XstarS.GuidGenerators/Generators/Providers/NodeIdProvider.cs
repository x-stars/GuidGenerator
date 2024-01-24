using System;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

    private class PhysicalAddress : NodeIdProvider
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
                        NodeIdProvider.PhysicalAddress.Create();
                }

                return NodeIdProvider.PhysicalAddress.Singleton ?? Initialize();
            }
        }

        private static NodeIdProvider.PhysicalAddress Create()
        {
            return Environment.OSVersion.Platform is PlatformID.Win32NT ?
                new NodeIdProvider.PhysicalAddress.Windows() :
                new NodeIdProvider.PhysicalAddress();
        }

        public sealed override byte[] NodeIdBytes => this.NodeIdBytesCache.Value;

        public sealed override NodeIdSource SourceType => NodeIdSource.PhysicalAddress;

        protected virtual byte[] GetNodeIdBytes()
        {
            var validIface = this.GetValidNetworkInterface();
            return (validIface is not null) ?
                validIface.GetPhysicalAddress().GetAddressBytes() :
                NodeIdProvider.RandomNumber.Instance.NodeIdBytes;
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
                   (macAddress.GetAddressBytes().Length == 6);
        }

        private sealed class Windows : PhysicalAddress
        {
            [System.Security.SuppressUnmanagedCodeSecurity]
            private static class SafeNativeMethods
            {
                [DllImport("rpcrt4.dll", ExactSpelling = true)]
                internal static extern int UuidCreateSequential(out Guid uuid);
            }

            internal Windows() { }

            protected override byte[] GetNodeIdBytes()
            {
                var status = SafeNativeMethods.UuidCreateSequential(out var uuid);
                if (status == 0)
                {
                    var nodeId = uuid.NodeIdToArray();
                    if ((nodeId[0] & 0x01) != 0x01)
                    {
                        return nodeId;
                    }
                }
                return base.GetNodeIdBytes();
            }
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

        private static byte[] CreateNodeIdBytes()
        {
            var newGuid = Guid.NewGuid();
            var nodeId = newGuid.NodeIdToArray();
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
