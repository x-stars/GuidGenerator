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
        : this(NodeIdProvider.MacAddress.Instance)
    {
    }

    protected TimeBasedGuidGenerator(NodeIdProvider nodeIdProvider)
    {
        this.TimestampProvider = TimestampProvider.Instance;
        this.NodeIdProvider = nodeIdProvider;
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
                    new TimeBasedGuidGenerator(
                        NodeIdProvider.RandomNumber.Instance);
            }

            return TimeBasedGuidGenerator.SingletonR ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.Version1;

    private long CurrentTimestamp => this.TimestampProvider.GetCurrentTimestamp();

    private byte[] NodeIdBytes => this.NodeIdProvider.NodeIdBytes;

    private bool IsNodeIdNonVolatile => this.NodeIdProvider.IsNonVolatile;

    internal static TimeBasedGuidGenerator CreateInstanceR()
    {
        var randomNodeId = NodeIdProvider.RandomNumber.Create();
        return new TimeBasedGuidGenerator(randomNodeId);
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
        var nodeId = this.NodeIdBytes;
        var nvNodeId = this.IsNodeIdNonVolatile ? nodeId : null;
        var timestamp = this.CurrentTimestamp;
        var clockSeq = GuidGeneratorState.RefreshState(nvNodeId, timestamp);
        guid.TimeLow() = (uint)(timestamp >> (0 * 8));
        guid.TimeMid() = (ushort)(timestamp >> (4 * 8));
        guid.TimeHi_Ver() = (ushort)(timestamp >> (6 * 8));
        guid.ClkSeqLow() = (byte)(clockSeq >> (0 * 8));
        guid.ClkSeqHi_Var() = (byte)(clockSeq >> (1 * 8));
        guid.SetNodeId(nodeId);
    }
}
