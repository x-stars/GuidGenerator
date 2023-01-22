using System;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

internal class TimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile TimeBasedGuidGenerator? Singleton;

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

    public override GuidVersion Version => GuidVersion.Version1;

    private long CurrentTimestamp => this.TimestampProvider.GetCurrentTimestamp();

    private byte[] NodeIdBytes => this.NodeIdProvider.NodeIdBytes;

    internal static TimeBasedGuidGenerator CreateWithRandomNodeId()
    {
        var randomNodeId = new NodeIdProvider.RandomNumber();
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
        var isRandomNodeId = (nodeId[0] & 0x01) == 0x01;
        var timestamp = this.CurrentTimestamp;
        var clockSeq = GuidGeneratorState.RefreshState(
            isRandomNodeId ? null : nodeId, timestamp);
        guid.TimeLow() = (uint)(timestamp >> (0 * 8));
        guid.TimeMid() = (ushort)(timestamp >> (4 * 8));
        guid.TimeHi_Ver() = (ushort)(timestamp >> (6 * 8));
        guid.ClkSeqLow() = (byte)(clockSeq >> (0 * 8));
        guid.ClkSeqHi_Var() = (byte)(clockSeq >> (1 * 8));
        guid.SetNodeId(nodeId);
    }
}
