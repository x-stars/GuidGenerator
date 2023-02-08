using System;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

internal class TimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile TimeBasedGuidGenerator? Singleton;

    private static volatile TimeBasedGuidGenerator? SingletonR;

    private readonly TimestampProvider TimestampProvider;

    private readonly NodeIdProvider NodeIdProvider;

    protected TimeBasedGuidGenerator()
        : this(NodeIdSource.PhysicalAddress)
    {
    }

    protected TimeBasedGuidGenerator(NodeIdSource nodeIdSource)
    {
        this.TimestampProvider = TimestampProvider.Instance;
        this.NodeIdProvider = NodeIdProvider.GetInstance(nodeIdSource);
    }

    internal static TimeBasedGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static TimeBasedGuidGenerator Initialize()
            {
                return TimeBasedGuidGenerator.Singleton ??=
                    new TimeBasedGuidGenerator();
            }

            return TimeBasedGuidGenerator.Singleton ?? Initialize();
        }
    }

    internal static TimeBasedGuidGenerator InstanceR
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static TimeBasedGuidGenerator Initialize()
            {
                return TimeBasedGuidGenerator.SingletonR ??=
                    new TimeBasedGuidGenerator(NodeIdSource.NonVolatileRandom);
            }

            return TimeBasedGuidGenerator.SingletonR ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.Version1;

    protected virtual bool IsTimestampLittleEndian => true;

    private DateTime EpochDataTime => TimestampEpochs.Gregorian;

    private long CurrentTimestamp =>
        this.TimestampProvider.CurrentTimestamp - this.EpochDataTime.Ticks;

    private byte[] NodeIdBytes => this.NodeIdProvider.NodeIdBytes;

    private NodeIdSource NodeIdSource => this.NodeIdProvider.SourceType;

    internal static TimeBasedGuidGenerator CreateInstanceR()
    {
        return new TimeBasedGuidGenerator(NodeIdSource.VolatileRandom);
    }

    public override Guid NewGuid()
    {
        var guid = default(Guid);
        this.FillTimeAndNodeFields(ref guid);
        this.FillVersionField(ref guid);
        this.FillVariantField(ref guid);
        return guid;
    }

    private void FillTimeAndNodeFields(ref Guid guid)
    {
        var timestamp = this.CurrentTimestamp;
        var nodeId = this.NodeIdBytes;
        var clockSeq = GuidGeneratorState.RefreshState(
            timestamp, nodeId, this.NodeIdSource);
        if (this.IsTimestampLittleEndian)
        {
            guid.TimeLow() = (uint)(timestamp >> (0 * 8));
            guid.TimeMid() = (ushort)(timestamp >> (4 * 8));
            guid.TimeHi_Ver() = (ushort)(timestamp >> (6 * 8));
        }
        else
        {
            guid.TimeLow() = (uint)(timestamp >> (4 * 8 - 4));
            guid.TimeMid() = (ushort)(timestamp >> (2 * 8 - 4));
            guid.TimeHi_Ver() = (ushort)(timestamp >> (0 * 8));
        }
        guid.ClkSeqLow() = (byte)(clockSeq >> (0 * 8));
        guid.ClkSeqHi_Var() = (byte)(clockSeq >> (1 * 8));
        guid.SetNodeId(nodeId);
    }

    internal sealed class Sequential : TimeBasedGuidGenerator
    {
        private static new volatile TimeBasedGuidGenerator.Sequential? Singleton;

        private static volatile TimeBasedGuidGenerator.Sequential? SingletonP;

        private Sequential()
            : base(NodeIdSource.NonVolatileRandom)
        {
        }

        private Sequential(NodeIdSource nodeIdSource)
            : base(nodeIdSource)
        {
        }

        internal static new TimeBasedGuidGenerator.Sequential Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static TimeBasedGuidGenerator.Sequential Initialize()
                {
                    return TimeBasedGuidGenerator.Sequential.Singleton ??=
                        new TimeBasedGuidGenerator.Sequential();
                }

                return TimeBasedGuidGenerator.Sequential.Singleton ?? Initialize();
            }
        }

        internal static TimeBasedGuidGenerator.Sequential InstanceP
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static TimeBasedGuidGenerator.Sequential Initialize()
                {
                    return TimeBasedGuidGenerator.Sequential.SingletonP ??=
                        new TimeBasedGuidGenerator.Sequential(NodeIdSource.PhysicalAddress);
                }

                return TimeBasedGuidGenerator.Sequential.SingletonP ?? Initialize();
            }
        }

        public override GuidVersion Version => GuidVersion.Version6;

        protected override bool IsTimestampLittleEndian => false;

        internal static TimeBasedGuidGenerator.Sequential CreateInstance()
        {
            return new TimeBasedGuidGenerator.Sequential(NodeIdSource.VolatileRandom);
        }
    }
}
