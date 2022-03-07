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

        private readonly Lazy<byte[]> LazyMacAddressBytes;

        protected TimeBasedGuidGenerator()
        {
            this.StartTimestamp = DateTime.UtcNow;
            this.HiResTimer = Stopwatch.StartNew();
            this.ClockSequence = new Random().Next();
            this.LazyMacAddressBytes =
                new Lazy<byte[]>(this.GetMacAdddressBytes);
        }

        internal static TimeBasedGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => TimeBasedGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Version1;

        private byte[] GetMacAdddressBytes()
        {
            var target = default(NetworkInterface);
            var ifaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var iface in ifaces)
            {
                if (iface.OperationalStatus == OperationalStatus.Up)
                {
                    target = iface;
                    break;
                }
            }
            if (target is null) { return new byte[6]; }
            return target.GetPhysicalAddress().GetAddressBytes();
        }

        public override Guid NewGuid()
        {
            var guidBytes = new byte[16];
            this.FillTimestampFields(guidBytes);
            this.FillVersionField(guidBytes);
            this.FillClockSeqFields(guidBytes);
            this.FillVariantField(guidBytes);
            this.FillNodeIDFields(guidBytes);
            return new Guid(guidBytes);
        }

        private long GetCurrentTimestamp()
        {
            var nowTs = this.StartTimestamp + this.HiResTimer.Elapsed;
            return nowTs.Ticks - GuidExtensions.BaseTimestamp.Ticks;
        }

        private void FillTimestampFields(byte[] guidBytes)
        {
            var timestamp = this.GetCurrentTimestamp();
            if (BitConverter.IsLittleEndian)
            {
                unsafe
                {
                    fixed (byte* pGuidBytes = &guidBytes[0])
                    {
                        *(long*)pGuidBytes = timestamp;
                    }
                }
            }
            else
            {
                foreach (var index in 0..8)
                {
                    var shifted = timestamp >> (index * 8);
                    guidBytes[index] = (byte)shifted;
                }
            }
        }

        private void FillClockSeqFields(byte[] guidBytes)
        {
            var clockSeq = Interlocked.Increment(ref this.ClockSequence);
            guidBytes[8] = (byte)(clockSeq >> (1 * 8));
            guidBytes[9] = (byte)(clockSeq >> (0 * 8));
        }

        private void FillNodeIDFields(byte[] guidBytes)
        {
            var nodeID = this.LazyMacAddressBytes.Value;
            Array.Copy(nodeID, 0, guidBytes, 10, nodeID.Length);
        }
    }
}
