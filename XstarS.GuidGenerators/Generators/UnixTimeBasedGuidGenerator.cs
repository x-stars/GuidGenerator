using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

internal sealed class UnixTimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile UnixTimeBasedGuidGenerator? Singleton;

    private readonly TimestampProvider TimestampProvider;

    private UnixTimeBasedGuidGenerator()
    {
        this.TimestampProvider = TimestampProvider.Instance;
    }

    internal static UnixTimeBasedGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static UnixTimeBasedGuidGenerator Initialize()
            {
                return UnixTimeBasedGuidGenerator.Singleton ??=
                    new UnixTimeBasedGuidGenerator();
            }

            return UnixTimeBasedGuidGenerator.Singleton ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.Version7;

    private DateTime EpochDataTime => TimestampEpochs.UnixTime;

    private long CurrentTimestamp =>
        this.TimestampProvider.CurrentTimestamp - this.EpochDataTime.Ticks;

    public override Guid NewGuid()
    {
        var guid = Guid.NewGuid();
        this.FillTimestampFields(ref guid);
        this.FillVersionField(ref guid);
        Debug.Assert(guid.GetVariant() == this.Variant);
        return guid;
    }

    private void FillTimestampFields(ref Guid guid)
    {
        const long ticksPerMs = 1_000_000 / 100;
        var timestamp = this.CurrentTimestamp;
        var tsMilliSec = timestamp / ticksPerMs;
        guid.TimeLow() = (uint)(tsMilliSec >> (2 * 8));
        guid.TimeMid() = (ushort)(tsMilliSec >> (0 * 8));
    }
}
