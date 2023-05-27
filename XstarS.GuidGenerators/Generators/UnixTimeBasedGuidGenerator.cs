#if !FEATURE_DISABLE_UUIDREV
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using XNetEx.Guids.Components;

namespace XNetEx.Guids.Generators;

internal sealed class UnixTimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile UnixTimeBasedGuidGenerator? Singleton;

    private static volatile UnixTimeBasedGuidGenerator? SingletonM;

    private readonly GuidComponents GuidComponents;

    private readonly TimestampProvider TimestampProvider;

    private readonly ClockResetCounter ClockResetCounter;

    private UnixTimeBasedGuidGenerator() : this(isGlobalMonotonic: false) { }

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

    internal static UnixTimeBasedGuidGenerator InstanceM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static UnixTimeBasedGuidGenerator Initialize()
            {
                return UnixTimeBasedGuidGenerator.SingletonM ??=
                    new UnixTimeBasedGuidGenerator(isGlobalMonotonic: true);
            }

            return UnixTimeBasedGuidGenerator.SingletonM ?? Initialize();
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
            var tsSubMsFrac = this.GetSubMillisecondFraction(timestamp);
            var counter = this.ClockResetCounter;
            if (counter.TryGetSequence(ref guid, tsSubMsFrac, out var sequence))
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
    private short GetSubMillisecondFraction(long timestamp)
    {
        var tsSubMs = timestamp % TimeSpan.TicksPerMillisecond;
        var tsSubMsFrac = (tsSubMs << 12) / TimeSpan.TicksPerMillisecond;
        return (short)tsSubMsFrac;
    }

    private void FillMonotonicityFields(ref Guid guid, short tsSubMsFrac, short sequence)
    {
        ref var timeHi_Ver = ref guid.TimeHi_Ver();
        timeHi_Ver = (ushort)(timeHi_Ver & 0xF000 | (ushort)tsSubMsFrac);
        ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
        clkSeqHi_Var = (byte)(clkSeqHi_Var & 0xC0 | (byte)(sequence >> 8));
        guid.ClkSeqLow() = (byte)sequence;
    }
}
#endif
