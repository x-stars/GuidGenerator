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

    internal sealed new class Version7 : TimeBasedGuidComponents
    {
        internal static readonly TimeBasedGuidComponents.Version7 Instance =
            new TimeBasedGuidComponents.Version7();

        private Version7() : base(TimestampEpochs.UnixTime) { }

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
}
