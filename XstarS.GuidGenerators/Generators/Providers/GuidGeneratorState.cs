using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal sealed partial class GuidGeneratorState
{
    private static readonly GuidGeneratorState PhysicalNodeState =
        new(NodeIdSource.PhysicalAddress);

    private static readonly GuidGeneratorState RandomNodeState =
        new(NodeIdSource.NonVolatileRandom);

    private readonly NodeIdSource NodeIdSource;

    private long Volatile_LastTimestamp;

    private volatile int ClockSequence;

    private volatile byte[]? LastNodeIdBytes;

    private NodeIdData LastNodeIdData;

    private GuidGeneratorState(NodeIdSource nodeIdSource)
    {
        this.NodeIdSource = nodeIdSource;
        this.ResetUnlocked();
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
            NodeIdSource.None => throw new InvalidOperationException(
                "The GUID generator state does not support using NodeIdSource.None."),
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

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Reset()
    {
        this.ResetUnlocked();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    internal void SetClockSequence(short clockSeq)
    {
        this.ClockSequence = clockSeq;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool UpdateNodeId(byte[] nodeId)
    {
        ref var nodeIdData = ref NodeIdData.OfBytes(nodeId);
        var nodeIdChanged = false;
        if ((this.LastNodeIdBytes is not null) &&
            !nodeIdData.Equals(in this.LastNodeIdData))
        {
            this.ClockSequence = this.GetInitClockSequence();
            nodeIdChanged = true;
        }
        this.SetLastNodeId(nodeId);
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
    private void ResetUnlocked()
    {
        this.LastTimestamp = 0L;
        this.ClockSequence = this.GetInitClockSequence();
        this.LastNodeIdBytes = null;
        this.LastNodeIdData = default(NodeIdData);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetInitClockSequence()
    {
        var newGuid = Guid.NewGuid();
        return (int)newGuid.TimeLow();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetLastNodeId(byte[] nodeId)
    {
        this.LastNodeIdBytes = nodeId;
        this.LastNodeIdData = NodeIdData.OfBytes(nodeId);
    }

    [StructLayout(LayoutKind.Sequential, Size = 6)]
    private struct NodeIdData
    {
        public volatile uint Bytes0123;
        public volatile ushort Bytes45;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(in NodeIdData other)
        {
            return (this.Bytes0123 == other.Bytes0123) &&
                   (this.Bytes45 == other.Bytes45);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ref NodeIdData OfBytes(byte[] bytes)
        {
#if UNSAFE_HELPERS || NETCOREAPP3_0_OR_GREATER
            return ref Unsafe.As<byte, NodeIdData>(ref bytes[0]);
#else
            fixed (byte* pBytes = &bytes[0])
            {
                return ref *(NodeIdData*)pBytes;
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly unsafe byte[] ToBytes()
        {
            var bytes = new byte[6];
#if UNSAFE_HELPERS || NETCOREAPP3_0_OR_GREATER
            Unsafe.As<byte, NodeIdData>(ref bytes[0]) = this;
#else
            fixed (byte* pBytes = &bytes[0])
            {
                *(NodeIdData*)pBytes = this;
            }
#endif
            return bytes;
        }
    }
}
