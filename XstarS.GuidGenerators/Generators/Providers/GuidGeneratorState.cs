using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal sealed partial class GuidGeneratorState
{
    private static readonly GuidGeneratorState PhysicalNodeState =
        new GuidGeneratorState(NodeIdSource.PhysicalAddress);

    private static readonly GuidGeneratorState RandomNodeState =
        new GuidGeneratorState(NodeIdSource.NonVolatileRandom);

    private readonly NodeIdSource NodeIdSource;

    private long Volatile_LastTimestamp;

    private volatile int ClockSequence;

    private volatile byte[]? LastNodeIdBytes;

    private GuidGeneratorState(NodeIdSource nodeIdSource)
    {
        this.NodeIdSource = nodeIdSource;
        this.Reinitialize();
    }

    private long LastTimestamp
    {
        get => Volatile.Read(ref this.Volatile_LastTimestamp);
        set => Volatile.Write(ref this.Volatile_LastTimestamp, value);
    }

    internal static GuidGeneratorState GetInstance(NodeIdSource nodeIdSource)
    {
        return nodeIdSource switch
        {
            NodeIdSource.PhysicalAddress => GuidGeneratorState.PhysicalNodeState,
            NodeIdSource.VolatileRandom => GuidGeneratorState.CreateVolatileState(),
            NodeIdSource.NonVolatileRandom => GuidGeneratorState.RandomNodeState,
            _ => throw new ArgumentOutOfRangeException(nameof(nodeIdSource)),
        };
    }

    private static GuidGeneratorState CreateVolatileState()
    {
        return new GuidGeneratorState(NodeIdSource.VolatileRandom);
    }

    public bool Refresh(long timestamp, byte[] nodeId, out short clockSeq)
    {
        var refreshed = false;
        lock (this)
        {
            _ = this.UpdateNodeId(nodeId);
            refreshed = this.UpdateTimestamp(timestamp);
            clockSeq = (short)this.ClockSequence;
        }
        _ = GuidGeneratorState.LastSavingAsyncResultCache.Value;
        return refreshed;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool UpdateNodeId(byte[] nodeId)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool NodeIdEquals(byte[] nodeId, byte[] lastNode)
        {
            return (nodeId == lastNode) ||
                NodeIdEqualityHelper.OfBytes(nodeId).Equals(
                    in NodeIdEqualityHelper.OfBytes(lastNode));
        }

        var nodeIdSource = this.NodeIdSource;
        if (!nodeIdSource.IsNonVolatile()) { return false; }
        var lastNode = this.LastNodeIdBytes;
        if (nodeId == lastNode) { return false; }

        var nodeIdChanged = false;
        if ((lastNode is not null) && !NodeIdEquals(nodeId, lastNode))
        {
            this.ClockSequence = this.GetInitClockSequence();
            nodeIdChanged = true;
        }
        this.LastNodeIdBytes = nodeId;
        return nodeIdChanged;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool UpdateTimestamp(long timestamp)
    {
        if (timestamp == this.LastTimestamp)
        {
            return false;
        }

        if (timestamp <= this.LastTimestamp)
        {
            this.ClockSequence++;
        }
        this.LastTimestamp = timestamp;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Reinitialize()
    {
        this.LastTimestamp = 0L;
        this.ClockSequence = this.GetInitClockSequence();
        this.LastNodeIdBytes = null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetInitClockSequence()
    {
        var newGuid = Guid.NewGuid();
        return (int)newGuid.TimeLow();
    }

    [StructLayout(LayoutKind.Sequential, Size = 6)]
    private struct NodeIdEqualityHelper
    {
        public uint Bytes0123;
        public ushort Bytes45;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(in NodeIdEqualityHelper other)
        {
            return (this.Bytes0123 == other.Bytes0123) &&
                   (this.Bytes45 == other.Bytes45);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ref NodeIdEqualityHelper OfBytes(byte[] bytes)
        {
#if UNSAFE_HELPERS || NETCOREAPP3_0_OR_GREATER
            return ref Unsafe.As<byte, NodeIdEqualityHelper>(ref bytes[0]);
#else
            fixed (byte* pBytes = &bytes[0])
            {
                return ref *(NodeIdEqualityHelper*)pBytes;
            }
#endif
        }
    }
}
