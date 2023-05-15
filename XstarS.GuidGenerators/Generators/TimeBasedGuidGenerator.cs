using System;
using System.Runtime.CompilerServices;
using System.Threading;
using XNetEx.Guids.Components;

namespace XNetEx.Guids.Generators;

internal class TimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
#if !FEATURE_DISABLE_UUIDREV
    , IBlockingGuidGenerator
#endif
{
    private static volatile TimeBasedGuidGenerator? Singleton;

    private static volatile TimeBasedGuidGenerator? SingletonR;

    protected readonly GuidComponents GuidComponents;

    private readonly TimestampProvider TimestampProvider;

    private readonly NodeIdProvider NodeIdProvider;

    private readonly GuidGeneratorState GeneratorState;

    protected TimeBasedGuidGenerator()
        : this(NodeIdSource.PhysicalAddress)
    {
    }

    protected TimeBasedGuidGenerator(NodeIdSource nodeIdSource)
    {
        this.GuidComponents = GuidComponents.OfVersion(this.Version);
        this.TimestampProvider = TimestampProvider.Instance;
        this.NodeIdProvider = NodeIdProvider.GetInstance(nodeIdSource);
        this.GeneratorState = GuidGeneratorState.GetInstance(nodeIdSource);
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

    private long CurrentTimestamp => this.TimestampProvider.CurrentTimestamp;

    private byte[] NodeIdBytes => this.NodeIdProvider.NodeIdBytes;

    internal static TimeBasedGuidGenerator CreateInstanceR()
    {
        return new TimeBasedGuidGenerator(NodeIdSource.VolatileRandom);
    }

    public override Guid NewGuid()
    {
        var spinner = new SpinWait();
        while (true)
        {
            if (this.TryNewGuid(out var guid))
            {
                return guid;
            }
            spinner.SpinOnce();
        }
    }

    public bool TryNewGuid(out Guid result)
    {
        result = default(Guid);
        if (this.TryFillTimeAndNodeFields(ref result))
        {
            this.FillVersionField(ref result);
            this.FillVariantField(ref result);
            return true;
        }
        return false;
    }

    private bool TryFillTimeAndNodeFields(ref Guid guid)
    {
        var state = this.GeneratorState;
        var timestamp = this.CurrentTimestamp;
        var nodeId = this.NodeIdBytes;
        var refreshed = state.Refresh(
            timestamp, nodeId, out var clockSeq);
        if (!refreshed) { return false; }
        var components = this.GuidComponents;
        components.SetTimestamp(ref guid, timestamp);
        components.SetClockSequence(ref guid, clockSeq);
        components.SetNodeId(ref guid, nodeId);
        return true;
    }

#if !FEATURE_DISABLE_UUIDREV
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

        internal static TimeBasedGuidGenerator.Sequential CreateInstance()
        {
            return new TimeBasedGuidGenerator.Sequential(NodeIdSource.VolatileRandom);
        }
    }
#endif
}
