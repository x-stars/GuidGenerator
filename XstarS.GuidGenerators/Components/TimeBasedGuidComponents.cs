using System;

namespace XNetEx.Guids.Components;

internal abstract class TimeBasedGuidComponents : GuidComponents, ITimeBasedGuidComponents
{
    private readonly DateTime EpochDateTime;

    protected TimeBasedGuidComponents(DateTime epochDateTime)
    {
        this.EpochDateTime = epochDateTime;
    }

    public sealed override long GetTimestamp(ref Guid guid)
    {
        var tsTicks = this.GetTimestampCore(ref guid);
        return tsTicks + this.EpochDateTime.Ticks;
    }

    public sealed override void SetTimestamp(ref Guid guid, long timestamp)
    {
        var tsTicks = timestamp - this.EpochDateTime.Ticks;
        this.SetTimestampCore(ref guid, tsTicks);
    }

    protected abstract long GetTimestampCore(ref Guid guid);

    protected abstract void SetTimestampCore(ref Guid guid, long timestamp);

#if !UUIDREV_DISABLE
    internal sealed new class Version7 : TimeBasedGuidComponents, ITimeRandomBasedGuidComponents
    {
        internal static readonly TimeBasedGuidComponents.Version7 Instance =
            new TimeBasedGuidComponents.Version7();

        private Version7() : base(TimestampEpochs.UnixTime) { }

        public override byte[] GetRandomData(ref Guid guid, out byte[] bitmask)
        {
            var randomData = this.GetRawData(ref guid, out bitmask);
            Array.Clear(randomData, 0, 6);
            Array.Clear(bitmask, 0, 6);
            return randomData;
        }

        public override void SetRandomData(ref Guid guid, byte[] randomData)
        {
            var timestamp = this.GetTimestampCore(ref guid);
            this.SetRawData(ref guid, randomData);
            this.SetTimestampCore(ref guid, timestamp);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public override void WriteRandomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
        {
            this.WriteRawData(ref guid, destination, bitmask);
            destination[0..6].Clear();
            bitmask[0..6].Clear();
        }

        public override void SetRandomData(ref Guid guid, ReadOnlySpan<byte> randomData)
        {
            var timestamp = this.GetTimestampCore(ref guid);
            this.SetRawData(ref guid, randomData);
            this.SetTimestampCore(ref guid, timestamp);
        }
#endif

        protected override long GetTimestampCore(ref Guid guid)
        {
            var tsField = (long)(
                ((ulong)guid.TimeLow() << (2 * 8)) |
                ((ulong)guid.TimeMid() << (0 * 8)));
            return tsField * TimeSpan.TicksPerMillisecond;
        }

        protected override void SetTimestampCore(ref Guid guid, long timestamp)
        {
            var tsMilliSec = timestamp / TimeSpan.TicksPerMillisecond;
            guid.TimeLow() = (uint)(tsMilliSec >> (2 * 8));
            guid.TimeMid() = (ushort)(tsMilliSec >> (0 * 8));
        }
    }
#endif
}
