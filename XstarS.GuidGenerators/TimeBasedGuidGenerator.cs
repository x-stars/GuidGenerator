using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XstarS.GuidGenerators
{
    internal class TimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
    {
        private static class Singleton
        {
            internal static readonly TimeBasedGuidGenerator Value =
                new TimeBasedGuidGenerator();
        }

        private readonly Stopwatch HiResTimer;

        private readonly long StartTimestamp;

        private volatile int ClockSequence;

        private readonly Lazy<byte[]> LazyMacAddressBytes;

        protected TimeBasedGuidGenerator()
        {
            var nowTime = DateTime.UtcNow;
            this.HiResTimer = Stopwatch.StartNew();
            var baseTime = GuidExtensions.BaseTimestamp;
            this.StartTimestamp = nowTime.Ticks - baseTime.Ticks;
            this.ClockSequence = new Random().Next();
            this.LazyMacAddressBytes = new Lazy<byte[]>(this.GetMacAddressBytes);
        }

        internal static TimeBasedGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => TimeBasedGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Version1;

        private long CurrentTimestamp => this.StartTimestamp + this.HiResTimer.ElapsedTicks;

        private byte[] MacAddressBytes => this.LazyMacAddressBytes.Value;

        public override Guid NewGuid()
        {
            var guid = default(Guid);
            this.FillTimestampFields(ref guid);
            this.FillVersionField(ref guid);
            this.FillClockSeqFields(ref guid);
            this.FillVariantField(ref guid);
            guid.SetNodeID(this.MacAddressBytes);
            return guid;
        }

        private byte[] GetMacAddressBytes()
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FillTimestampFields(ref Guid guid)
        {
            var timestamp = this.CurrentTimestamp;
            guid.TimeLow() = (uint)(timestamp >> (0 * 8));
            guid.TimeMid() = (ushort)(timestamp >> (4 * 8));
            guid.TimeHi_Ver() = (ushort)(timestamp >> (6 * 8));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FillClockSeqFields(ref Guid guid)
        {
            var clockSeq = Interlocked.Increment(ref this.ClockSequence);
            guid.ClkSeqLow() = (byte)(clockSeq >> (0 * 8));
            guid.ClkSeqHi_Var() = (byte)(clockSeq >> (1 * 8));
        }
    }
}
