using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal abstract class NodeIdProvider
    {
        private static class Singleton
        {
            internal static readonly NodeIdProvider Value =
                new NodeIdProvider.Network();
        }

        protected NodeIdProvider() { }

        internal static NodeIdProvider Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => NodeIdProvider.Singleton.Value;
        }

        public abstract byte[] GetMacAddressBytes();

        private sealed class Network : NodeIdProvider
        {
            internal Network() { }

            public override byte[] GetMacAddressBytes()
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
