using System;
using System.Runtime.CompilerServices;
using System.Threading;
using XNetEx.Guids.Components;

namespace XNetEx.Guids.Generators;

internal class TimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
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
        var spinner = new SpinWait();
        var state = this.GeneratorState;
    RefreshState:
        var timestamp = this.CurrentTimestamp;
        var nodeId = this.NodeIdBytes;
        var refreshed = state.Refresh(
            timestamp, nodeId, out var clockSeq);
        if (!refreshed)
        {
            spinner.SpinOnce();
            goto RefreshState;
        }
        var components = this.GuidComponents;
        components.SetTimestamp(ref guid, timestamp);
        components.SetClockSequence(ref guid, clockSeq);
        components.SetNodeId(ref guid, nodeId);
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
