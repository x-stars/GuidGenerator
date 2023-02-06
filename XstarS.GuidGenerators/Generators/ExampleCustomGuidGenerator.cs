using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal sealed class ExampleCustomGuidGenerator : CustomGuidGenerator, IGuidGenerator
{
    private static volatile ExampleCustomGuidGenerator? Singleton;

    private volatile int Sequence;

    private ExampleCustomGuidGenerator()
        : base(TimestampEpochs.Epoch2020, NodeIdSource.NonVolatileRandom)
    {
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

    public override Guid NewGuid()
    {
        var guid = Guid.NewGuid();
        this.FillTimestampFields(ref guid);
        this.FillVersionField(ref guid);
        Debug.Assert(guid.GetVariant() == this.Variant);
        guid.NodeId(4) = this.GetNodeIdByte(4);
        guid.NodeId(5) = (byte)Interlocked.Increment(ref this.Sequence);
        return guid;
    }

    private void FillTimestampFields(ref Guid guid)
    {
        const long ticksPerSec = 1_000_000_000 / 100;
        var timestamp = this.GetCurrentTimestamp();
        var tsSeconds = timestamp / ticksPerSec;
        guid.TimeLow() = (uint)tsSeconds;
        var tsNanoSec = timestamp % ticksPerSec * 100;
        var tsNsFrac = (tsNanoSec << 16) / 1_000_000_000;
        guid.TimeMid() = (ushort)tsNsFrac;
    }
}
