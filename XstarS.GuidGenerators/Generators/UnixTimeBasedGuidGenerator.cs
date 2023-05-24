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
            var counter = this.ClockResetCounter;
            if (counter.TryGetSequence(ref guid, timestamp, out var sequence))
            {
                var components = this.GuidComponents;
                components.SetTimestamp(ref guid, timestamp);
                this.FillMonotonicityFields(ref guid, timestamp, sequence);
                this.FillVersionField(ref guid);
                Debug.Assert(guid.GetVariant() == this.Variant);
                return guid;
            }
            spinner.SpinOnce();
        }
    }

    private void FillMonotonicityFields(ref Guid guid, long timestamp, short sequence)
    {
        var tsSubMs = timestamp % TimeSpan.TicksPerMillisecond;
        var tsSubMsFrac = (tsSubMs << (12 + 2)) / TimeSpan.TicksPerMillisecond;
        ref var timeHi_Ver = ref guid.TimeHi_Ver();
        var fracHi12 = (int)tsSubMsFrac >> 2;
        timeHi_Ver = (ushort)(timeHi_Ver & 0xF000 | fracHi12);
        ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
        var fracLow2 = (int)tsSubMsFrac & ~(-1 << 2);
        var seqHi4 = (byte)((int)sequence >> 8);
        var fracLow2_seqHi4 = (fracLow2 << 4) | seqHi4;
        clkSeqHi_Var = (byte)(clkSeqHi_Var & 0xC0 | fracLow2_seqHi4);
        guid.ClkSeqLow() = (byte)sequence;
    }
}
#endif
