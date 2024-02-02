#if !UUIDREV_DISABLE
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using XNetEx.Guids.Components;

namespace XNetEx.Guids.Generators;

internal sealed class UnixTimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile UnixTimeBasedGuidGenerator? Singleton;

    private readonly GuidComponents GuidComponents;

    private readonly TimestampProvider TimestampProvider;

    private readonly ClockResetCounter ClockResetCounter;

    private UnixTimeBasedGuidGenerator() : this(isGlobalMonotonic: true) { }

    private UnixTimeBasedGuidGenerator(bool isGlobalMonotonic)
    {
        this.GuidComponents = GuidComponents.OfVersion(this.Version);
        this.TimestampProvider = TimestampProvider.Instance;
        this.ClockResetCounter = ClockResetCounter.GetInstance(isGlobalMonotonic);
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
        var spinner = new SpinWait();
        while (true)
        {
            var timestamp = this.CurrentTimestamp;
            var tsMilliSec = this.DivideTimestamp(timestamp, out var tsSubMsFrac);
            var counter = this.ClockResetCounter;
            if (counter.TryGetSequence(ref guid, tsMilliSec, tsSubMsFrac, out var sequence))
            {
                var components = this.GuidComponents;
                components.SetTimestamp(ref guid, timestamp);
                this.FillMonotonicityFields(ref guid, tsSubMsFrac, sequence);
                this.FillVersionField(ref guid);
                Debug.Assert(guid.GetVariant() == this.Variant);
                return guid;
            }
            spinner.SpinOnce();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private long DivideTimestamp(long timestamp, out short tsSubMsFrac)
    {
        var tsSubMs = timestamp % TimeSpan.TicksPerMillisecond;
        tsSubMsFrac = (short)((tsSubMs << 12) / TimeSpan.TicksPerMillisecond);
        return timestamp / TimeSpan.TicksPerMillisecond;
    }

    private void FillMonotonicityFields(ref Guid guid, short tsSubMsFrac, short sequence)
    {
        var tsFrac = (ushort)(tsSubMsFrac & ~0xF000);
        ref var timeHi_Ver = ref guid.TimeHi_Ver();
        timeHi_Ver = (ushort)(timeHi_Ver & 0xF000 | tsFrac);
        var seqHi = (byte)((sequence >> 8) & ~0xC0);
        ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
        clkSeqHi_Var = (byte)(clkSeqHi_Var & 0xC0 | seqHi);
        guid.ClkSeqLow() = (byte)(sequence >> 0);
    }
}
#endif
