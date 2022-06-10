using System;
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

        private long LastTimestamp;

        private volatile int ClockSequence;

        private readonly TimestampProvider TimestampProvider;

        private readonly Lazy<byte[]> LazyNodeIdBytes;

        protected TimeBasedGuidGenerator()
            : this(NodeIdProvider.MacAddress.Instance)
        {
        }

        protected TimeBasedGuidGenerator(NodeIdProvider nodeIdProvider)
        {
            this.TimestampProvider = TimestampProvider.Instance;
            this.LastTimestamp = this.CurrentTimestamp;
            this.ClockSequence = new Random().Next();
            this.LazyNodeIdBytes = new Lazy<byte[]>(nodeIdProvider.GetNodeIdBytes);
        }

        internal static TimeBasedGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => TimeBasedGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Version1;

        protected virtual int TimestampShift => 0;

        private long CurrentTimestamp => this.TimestampProvider.GetCurrentTimestamp();

        private byte[] NodeIdBytes => this.LazyNodeIdBytes.Value;

        internal static TimeBasedGuidGenerator CreateWithRandomNodeId()
        {
            var randomNodeId = NodeIdProvider.RandomNumber.Instance;
            return new TimeBasedGuidGenerator(randomNodeId);
        }

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
            var tsShift = this.TimestampShift;
            lock (this.TimestampProvider)
            {
                var lastTs = this.LastTimestamp;
                var timestamp = this.CurrentTimestamp;
                if ((timestamp >> tsShift) <= (lastTs >> tsShift))
                {
                    this.ClockSequence++;
                }
                this.LastTimestamp = timestamp;
                clockSeq = this.ClockSequence;
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
