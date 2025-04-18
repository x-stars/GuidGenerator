using System;

namespace XNetEx.Guids.Components;

internal abstract class TimeBasedGuidComponents : GuidComponents, ITimeBasedGuidComponents
{
    private readonly DateTime EpochDateTime;

    private readonly long MaxTimestamp;

    protected TimeBasedGuidComponents(DateTime epochDateTime, long maxTimestamp)
    {
        this.EpochDateTime = epochDateTime;
        this.MaxTimestamp = maxTimestamp;
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

    public sealed override void SetTimestampChecked(ref Guid guid, long timestamp)
    {
        var tsTicks = timestamp - this.EpochDateTime.Ticks;
        if ((ulong)tsTicks > (ulong)this.MaxTimestamp)
        {
            throw new ArgumentOutOfRangeException(nameof(timestamp),
                $"Timestamp for the current GUID version " +
                $"must be between {this.EpochDateTime} and {this.GetMaxDateTime()}.");
        }

        this.SetTimestamp(ref guid, timestamp);
    }

    protected abstract long GetTimestampCore(ref Guid guid);

    protected abstract void SetTimestampCore(ref Guid guid, long timestamp);

    private DateTime GetMaxDateTime()
    {
        var maxTicks = this.EpochDateTime.Ticks + this.MaxTimestamp;
        if (maxTicks <= DateTime.MaxValue.Ticks)
        {
            return new DateTime(maxTicks, DateTimeKind.Utc);
        }
        else
        {
            const ulong utcFlag = 1UL << 62;
            const ulong flagsMask = 3UL << 62;
            var maxData = (ulong)maxTicks & ~flagsMask | utcFlag;
            unsafe { return *(DateTime*)&maxData; }
        }
    }
}
