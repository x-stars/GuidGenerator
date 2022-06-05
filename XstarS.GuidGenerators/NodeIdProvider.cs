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
                var upIface = this.GetUpNetworkInterface();
                if (upIface is null) { return new byte[6]; }
                return upIface.GetPhysicalAddress().GetAddressBytes();
            }

            private NetworkInterface? GetUpNetworkInterface()
            {
                var ifaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var iface in ifaces)
                {
                    if (iface.OperationalStatus == OperationalStatus.Up)
                    {
                        return iface;
                    }
                }
                return null;
            }
        }
    }
}
