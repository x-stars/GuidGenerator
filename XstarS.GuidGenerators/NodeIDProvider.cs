using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal abstract class NodeIDProvider
    {
        private static class Singleton
        {
            internal static readonly NodeIDProvider Value =
                new NodeIDProvider.Network();
        }

        protected NodeIDProvider() { }

        internal static NodeIDProvider Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => NodeIDProvider.Singleton.Value;
        }

        public abstract byte[] GetMacAddressBytes();

        private sealed class Network : NodeIDProvider
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
