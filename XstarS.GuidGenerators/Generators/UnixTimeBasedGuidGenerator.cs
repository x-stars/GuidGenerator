#if !FEATURE_DISABLE_UUIDREV
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using XNetEx.Guids.Components;

namespace XNetEx.Guids.Generators;

internal sealed class UnixTimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile UnixTimeBasedGuidGenerator? Singleton;

    private readonly GuidComponents GuidComponents;

    private readonly TimestampProvider TimestampProvider;

    private UnixTimeBasedGuidGenerator()
    {
        this.GuidComponents = GuidComponents.OfVersion(this.Version);
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

    private long CurrentTimestamp => this.TimestampProvider.CurrentTimestamp;

    public override Guid NewGuid()
    {
        var guid = Guid.NewGuid();
        var timestamp = this.CurrentTimestamp;
        var components = this.GuidComponents;
        components.SetTimestamp(ref guid, timestamp);
        this.FillExtraTimeFields(ref guid, timestamp);
        this.FillVersionField(ref guid);
        Debug.Assert(guid.GetVariant() == this.Variant);
        return guid;
    }

    private void FillExtraTimeFields(ref Guid guid, long timestamp)
    {
        var tsSubMs = timestamp % TimeSpan.TicksPerMillisecond;
        var tsSubMsFrac = (tsSubMs << (12 + 2)) / TimeSpan.TicksPerMillisecond;
        ref var timeHi_Ver = ref guid.TimeHi_Ver();
        var fracHi12 = (int)tsSubMsFrac >> 2;
        timeHi_Ver = (ushort)(timeHi_Ver & 0xF000 | fracHi12);
        ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
        var fracLow2 = (int)tsSubMsFrac & ~(-1 << 2);
        clkSeqHi_Var = (byte)(clkSeqHi_Var & 0xCF | (fracLow2 << 4));
    }
}
#endif
