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
}
