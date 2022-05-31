using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XstarS.GuidGenerators
{
    internal class TimeBasedGuidGenerator : GuidGenerator
    {
        private static class Singleton
        {
            internal static readonly TimeBasedGuidGenerator Value =
                new TimeBasedGuidGenerator();
        }

        private readonly DateTime StartTimestamp;

        private readonly Stopwatch HiResTimer;

        private volatile int ClockSequence;

        private readonly Lazy<NetworkInterface?> LazyUpNetworkInterface;

        private readonly Lazy<byte[]> LazyMacAddressBytes;

        protected TimeBasedGuidGenerator()
        {
            this.StartTimestamp = DateTime.UtcNow;
            this.HiResTimer = Stopwatch.StartNew();
            this.ClockSequence = new Random().Next();
            this.LazyMacAddressBytes = new Lazy<byte[]>(this.GetMacAdddressBytes);
            this.LazyUpNetworkInterface = new Lazy<NetworkInterface?>(this.GetUpNetworkInterface);
        }

        internal static TimeBasedGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => TimeBasedGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Version1;

        protected NetworkInterface? UpNetworkInterface => this.LazyUpNetworkInterface.Value;

        private byte[] MacAddressBytes => this.LazyMacAddressBytes.Value;

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

        private byte[] GetMacAdddressBytes()
        {
            var upIface = this.UpNetworkInterface;
            if (upIface is null) { return new byte[6]; }
            return upIface.GetPhysicalAddress().GetAddressBytes();
        }

        public override Guid NewGuid()
        {
            var guid = default(Guid);
            this.FillTimestampFields(ref guid);
            this.FillVersionField(ref guid);
            this.FillClockSeqFields(ref guid);
            this.FillVariantField(ref guid);
            this.FillNodeIDFields(ref guid);
            return guid;
        }

        private long GetCurrentTimestamp()
        {
            var nowTs = this.StartTimestamp + this.HiResTimer.Elapsed;
            return nowTs.Ticks - GuidExtensions.BaseTimestamp.Ticks;
        }

        private void FillTimestampFields(ref Guid guid)
        {
            var timestamp = this.GetCurrentTimestamp();
            guid.TimeLow() = (uint)(timestamp >> (0 * 8));
            guid.TimeMid() = (ushort)(timestamp >> (4 * 8));
            guid.TimeHi_Ver() = (ushort)(timestamp >> (6 * 8));
        }

        private void FillClockSeqFields(ref Guid guid)
        {
            var clockSeq = Interlocked.Increment(ref this.ClockSequence);
            guid.ClkSeqLow() = (byte)(clockSeq >> (0 * 8));
            guid.ClkSeqHi_Var() = (byte)(clockSeq >> (1 * 8));
        }

        private unsafe void FillNodeIDFields(ref Guid guid)
        {
            var nodeID = this.MacAddressBytes;
            var size = Math.Min(nodeID.Length, 6);
            fixed (byte* pNodeID = &nodeID[0])
            {
                Buffer.MemoryCopy(pNodeID, guid.NodeID(), size, size);
            }
        }
    }
}
