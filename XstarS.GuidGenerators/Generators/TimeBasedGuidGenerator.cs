using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal class TimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile TimeBasedGuidGenerator? Singleton;

    private readonly TimestampProvider TimestampProvider;

    private readonly NodeIdProvider NodeIdProvider;

    private long Volatile_LastTimestamp;

    private volatile int ClockSequence;

    private volatile byte[]? LastNodeIdBytes;

    protected TimeBasedGuidGenerator()
        : this(NodeIdProvider.MacAddress.Instance)
    {
    }

    protected TimeBasedGuidGenerator(NodeIdProvider nodeIdProvider)
    {
        this.TimestampProvider = TimestampProvider.Instance;
        this.NodeIdProvider = nodeIdProvider;
        this.LastTimestamp = this.CurrentTimestamp;
        this.ClockSequence = GlobalRandom.Next<int>();
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

    protected virtual int TimestampShift => 0;

    private long CurrentTimestamp => this.TimestampProvider.GetCurrentTimestamp();

    private byte[] NodeIdBytes => this.NodeIdProvider.NodeIdBytes;

    private long LastTimestamp
    {
        get => Volatile.Read(ref this.Volatile_LastTimestamp);
        set => Volatile.Write(ref this.Volatile_LastTimestamp, value);
    }

    internal static TimeBasedGuidGenerator CreateWithRandomNodeId()
    {
        var randomNodeId = new NodeIdProvider.RandomNumber();
        return new TimeBasedGuidGenerator(randomNodeId);
    }

    public override Guid NewGuid()
    {
        var guid = default(Guid);
        this.FillNodeIdField(ref guid);
        this.FillTimeRelatedFields(ref guid);
        this.FillVersionField(ref guid);
        this.FillVariantField(ref guid);
        return guid;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void FillTimeRelatedFields(ref Guid guid)
    {
        var timestamp = this.GetTimestampAndClockSeq(out var clockSeq);
        guid.TimeLow() = (uint)(timestamp >> (0 * 8));
        guid.TimeMid() = (ushort)(timestamp >> (4 * 8));
        guid.TimeHi_Ver() = (ushort)(timestamp >> (6 * 8));
        guid.ClkSeqLow() = (byte)(clockSeq >> (0 * 8));
        guid.ClkSeqHi_Var() = (byte)(clockSeq >> (1 * 8));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void FillNodeIdField(ref Guid guid)
    {
        var nodeId = this.NodeIdBytes;
        var lastNodeId = this.LastNodeIdBytes;
        if (nodeId != lastNodeId)
        {
            this.UpdateLastNodeIdBytes();
        }
        guid.SetNodeId(nodeId);
    }

    private long GetTimestampAndClockSeq(out int clockSeq)
    {
        var tsShift = this.TimestampShift;
        lock (this.TimestampProvider)
        {
            var timestamp = this.CurrentTimestamp;
            var lastTs = this.LastTimestamp;
            if ((timestamp >> tsShift) <= (lastTs >> tsShift))
            {
                this.ClockSequence++;
            }
            this.LastTimestamp = timestamp;
            clockSeq = this.ClockSequence;
            return timestamp;
        }
    }

    private unsafe void UpdateLastNodeIdBytes()
    {
        lock (this.TimestampProvider)
        {
            var nodeId = this.NodeIdBytes;
            var lastId = this.LastNodeIdBytes;
            if ((nodeId != lastId) && (lastId is not null))
            {
                if ((nodeId[0] != lastId[0]) || (nodeId[1] != lastId[1]) ||
                    (nodeId[2] != lastId[2]) || (nodeId[3] != lastId[3]) ||
                    (nodeId[4] != lastId[4]) || (nodeId[5] != lastId[5]))
                {
                    this.ClockSequence = GlobalRandom.Next<int>();
                }
            }
            this.LastNodeIdBytes = nodeId;
        }
    }
}
