using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

        private long LastTimestamp;

        private volatile int ClockSequence;

        private readonly Lazy<byte[]> LazyNodeIdBytes;

        protected TimeBasedGuidGenerator()
        {
            var nowTime = DateTime.UtcNow;
            this.HiResTimer = Stopwatch.StartNew();
            var baseTime = GuidExtensions.BaseTimestamp;
            this.StartTimestamp = nowTime.Ticks - baseTime.Ticks;
            this.LastTimestamp = this.StartTimestamp;
            this.ClockSequence = new Random().Next();
            var provider = NodeIdProvider.MacAddress.Instance;
            this.LazyNodeIdBytes = new Lazy<byte[]>(provider.GetNodeIdBytes);
        }

        internal static TimeBasedGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => TimeBasedGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Version1;

        protected virtual int TimestampShift => 0;

        private long CurrentTimestamp => this.StartTimestamp + this.HiResTimer.ElapsedTicks;

        private byte[] NodeIdBytes => this.LazyNodeIdBytes.Value;

        public override Guid NewGuid()
        {
            var guid = default(Guid);
            this.FillTimeRelatedFields(ref guid);
            this.FillVersionField(ref guid);
            this.FillVariantField(ref guid);
            guid.SetNodeId(this.NodeIdBytes);
            return guid;
        }

        private long GetTimestampAndClockSeq(out int clockSeq)
        {
            lock (this.HiResTimer)
            {
                var tsShift = this.TimestampShift;
                var lastTs = this.LastTimestamp;
                var timestamp = this.CurrentTimestamp;
                var lClockSeq = this.ClockSequence;
                if ((timestamp >> tsShift) <= (lastTs >> tsShift))
                {
                    this.ClockSequence = ++lClockSeq;
                }
                this.LastTimestamp = timestamp;
                clockSeq = lClockSeq;
                return timestamp;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FillTimeRelatedFields(ref Guid guid)
        {
            var timestamp = this.GetTimestampAndClockSeq(out var clockSeq);
            guid.TimeLow() = (uint)(timestamp >> (0 * 8));
            guid.TimeMid() = (ushort)(timestamp >> (4 * 8));
            guid.TimeHi_Ver() = (ushort)(timestamp >> (6 * 8));
            guid.ClkSeqLow() = (byte)(clockSeq >> (0 * 8));
            guid.ClkSeqHi_Var() = (byte)(clockSeq >> (1 * 8));
        }
    }
}
