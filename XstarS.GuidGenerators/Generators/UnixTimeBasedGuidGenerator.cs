#if !UUIDREV_DISABLE
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using XNetEx.Guids.Components;

namespace XNetEx.Guids.Generators;

internal sealed class UnixTimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
{
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

    private UnixTimeBasedGuidGenerator(Func<DateTime>? timestampProvider = null)
        : this(isGlobalMonotonic: true)
    {
        this.TimestampProvider = (timestampProvider is not null) ?
            TimestampProvider.CreateCustom(timestampProvider) :
            TimestampProvider.Instance;
        this.ClockResetCounter = ClockResetCounter.CreateCustom();
    }

    internal static UnixTimeBasedGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static UnixTimeBasedGuidGenerator Initialize()
            {
                return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                    new UnixTimeBasedGuidGenerator());
            }

            return Volatile.Read(ref field) ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.Version7;

    private long CurrentTimestamp => this.TimestampProvider.CurrentTimestamp;

    internal static UnixTimeBasedGuidGenerator CreateCustomState(
        Func<DateTime>? timestampProvider = null)
    {
        return new UnixTimeBasedGuidGenerator(timestampProvider);
    }

    public override Guid NewGuid()
    {
        var guid = Guid.NewGuid();
        var spinner = new SpinWait();
        while (true)
        {
            var timestamp = this.CurrentTimestamp;
            var tsNoClock = this.DivideTimestamp(timestamp, out var clock);
            var initSeq = (short)((guid.ClkSeqHi_Var() << 8) | guid.ClkSeqLow());
            var counter = this.ClockResetCounter;
            if (counter.TryGetSequence(tsNoClock, clock, initSeq, out var sequence))
            {
                this.FillTimeFieldsChecked(ref guid, timestamp);
                this.FillMonotonicityFields(ref guid, clock, sequence);
                this.FillVersionFieldUnchecked(ref guid);
                Debug.Assert(guid.GetVariant() == this.Variant);
                return guid;
            }
            spinner.SpinOnce();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private long DivideTimestamp(long timestamp, out short clock)
    {
        var tsSubMs = timestamp % TimeSpan.TicksPerMillisecond;
        clock = (short)((tsSubMs << 12) / TimeSpan.TicksPerMillisecond);
        return timestamp / TimeSpan.TicksPerMillisecond;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void FillTimeFieldsChecked(ref Guid guid, long timestamp)
    {
        var components = this.GuidComponents;
        if (components.TrySetTimestamp(ref guid, timestamp) is string errorMessage)
        {
            throw new InvalidOperationException(errorMessage);
        }
    }

    private void FillMonotonicityFields(ref Guid guid, short clock, short sequence)
    {
        var clkField = (ushort)(clock & ~0xF000);
        ref var timeHi_Ver = ref guid.TimeHi_Ver();
        timeHi_Ver = (ushort)(timeHi_Ver & 0xF000 | clkField);
        var seqHi = (byte)((sequence >> 8) & ~0xC0);
        ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
        clkSeqHi_Var = (byte)(clkSeqHi_Var & 0xC0 | seqHi);
        guid.ClkSeqLow() = (byte)(sequence >> 0);
    }
}
#endif
