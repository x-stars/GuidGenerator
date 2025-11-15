using System;
using System.Runtime.CompilerServices;
using System.Threading;
using XNetEx.Guids.Components;

namespace XNetEx.Guids.Generators;

internal partial class TimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
#if !UUIDREV_DISABLE
    , IBlockingGuidGenerator
#endif
{
    private protected readonly GuidComponents GuidComponents;

    private readonly TimestampProvider TimestampProvider;

    private readonly NodeIdProvider NodeIdProvider;

    private readonly GuidGeneratorState GeneratorState;

    protected TimeBasedGuidGenerator() : this(NodeIdSource.PhysicalAddress) { }

    protected TimeBasedGuidGenerator(NodeIdSource nodeIdSource)
    {
        this.GuidComponents = GuidComponents.OfVersion(this.Version);
        this.TimestampProvider = TimestampProvider.Instance;
        this.NodeIdProvider = NodeIdProvider.GetInstance(nodeIdSource);
        this.GeneratorState = GuidGeneratorState.GetInstance(nodeIdSource);
    }

    protected TimeBasedGuidGenerator(
        NodeIdSource nodeIdSource, short? initClockSeq = null,
        Func<DateTime>? timestampProvider = null, Func<byte[]>? nodeIdProvider = null)
        : this(NodeIdSource.VolatileRandom)
    {
        this.TimestampProvider = (timestampProvider is not null) ?
            TimestampProvider.CreateCustom(timestampProvider) :
            TimestampProvider.Instance;
        this.NodeIdProvider = (nodeIdProvider is not null) ?
            NodeIdProvider.CreateCustom(nodeIdProvider) :
            NodeIdProvider.GetInstance(nodeIdSource);
        if (initClockSeq is short initClockSeqValue)
        {
            this.GeneratorState.SetClockSequence(initClockSeqValue);
        }
    }

    internal static TimeBasedGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static TimeBasedGuidGenerator Initialize()
            {
                return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                    new TimeBasedGuidGenerator());
            }

            return Volatile.Read(ref field) ?? Initialize();
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
                return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                    new TimeBasedGuidGenerator(NodeIdSource.NonVolatileRandom));
            }

            return Volatile.Read(ref field) ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.Version1;

    private long CurrentTimestamp => this.TimestampProvider.CurrentTimestamp;

    private byte[] NodeIdBytes => this.NodeIdProvider.NodeIdBytes;

    internal static TimeBasedGuidGenerator CreateInstanceR()
    {
        return new TimeBasedGuidGenerator(NodeIdSource.VolatileRandom);
    }

    internal static TimeBasedGuidGenerator CreateCustomState(
        NodeIdSource nodeIdSource = NodeIdSource.PhysicalAddress, short? initClockSeq = null,
        Func<DateTime>? timestampProvider = null, Func<byte[]>? nodeIdProvider = null)
    {
        if (nodeIdSource is NodeIdSource.None)
        {
            throw new InvalidOperationException(
                "This GUID version does not support using NodeIdSource.None.");
        }

        return new TimeBasedGuidGenerator(
            nodeIdSource, initClockSeq, timestampProvider, nodeIdProvider);
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

    public virtual bool TryNewGuid(out Guid result)
    {
        result = default(Guid);
        if (this.TryFillTimeAndNodeFields(ref result))
        {
            this.FillVersionFieldUnchecked(ref result);
            this.FillVariantFieldUnchecked(ref result);
            return true;
        }
        return false;
    }

    private bool TryFillTimeAndNodeFields(ref Guid guid)
    {
        var state = this.GeneratorState;
        var timestamp = this.CurrentTimestamp;
        var nodeId = this.NodeIdBytes;
        var updated = state.Update(
            timestamp, nodeId, out var clockSeq);
        if (!updated) { return false; }
        this.FillTimeFieldsChecked(ref guid, timestamp);
        var components = this.GuidComponents;
        components.SetClockSequence(ref guid, clockSeq);
        components.SetNodeId(ref guid, nodeId);
        return true;
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
}
