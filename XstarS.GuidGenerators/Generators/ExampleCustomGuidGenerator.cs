using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal sealed class ExampleCustomGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile ExampleCustomGuidGenerator? Singleton;

    private readonly TimestampProvider TimestampProvider;

    private readonly NodeIdProvider NodeIdProvider;

    private volatile int Sequence;

    private ExampleCustomGuidGenerator()
    {
        this.TimestampProvider = TimestampProvider.Instance;
        this.NodeIdProvider = NodeIdProvider.GetInstance(NodeIdSource.NonVolatileRandom);
        this.Sequence = -1;
    }

    internal static ExampleCustomGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static ExampleCustomGuidGenerator Initialize()
            {
                return ExampleCustomGuidGenerator.Singleton ??=
                    new ExampleCustomGuidGenerator();
            }

            return ExampleCustomGuidGenerator.Singleton ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.Version8;

    private DateTime EpochDateTime => TimestampEpochs.Epoch2020;

    private long CurrentTimestamp =>
        this.TimestampProvider.CurrentTimestamp - this.EpochDateTime.Ticks;

    private byte NodeIdByte => this.NodeIdProvider.NodeIdBytes[4];

    public override Guid NewGuid()
    {
        var guid = Guid.NewGuid();
        this.FillTimestampFields(ref guid);
        this.FillVersionField(ref guid);
        Debug.Assert(guid.GetVariant() == this.Variant);
        guid.NodeId(4) = this.NodeIdByte;
        guid.NodeId(5) = (byte)Interlocked.Increment(ref this.Sequence);
        return guid;
    }

    private void FillTimestampFields(ref Guid guid)
    {
        const long ticksPerSec = 1_000_000_000 / 100;
        var timestamp = this.CurrentTimestamp;
        var tsSeconds = timestamp / ticksPerSec;
        guid.TimeLow() = (uint)tsSeconds;
        var tsNanoSec = timestamp % ticksPerSec * 100;
        var tsNsFrac = (tsNanoSec << 16) / 1_000_000_000;
        guid.TimeMid() = (ushort)tsNsFrac;
    }
}
