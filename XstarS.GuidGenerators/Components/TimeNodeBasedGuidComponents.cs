﻿using System;

namespace XNetEx.Guids.Components;

internal abstract class TimeNodeBasedGuidComponents
    : TimeBasedGuidComponents, ITimeNodeBasedGuidComponents
{
    protected TimeNodeBasedGuidComponents()
        : base(TimestampEpochs.Gregorian)
    {
    }

    public override short GetClockSequence(ref Guid guid)
    {
        return (short)(
            ((uint)guid.ClkSeqLow() << (0 * 8)) |
            ((uint)(guid.ClkSeqHi_Var() & ~0xC0) << (1 * 8)));
    }

    public override void SetClockSequence(ref Guid guid, short clockSeq)
    {
        guid.ClkSeqLow() = (byte)(clockSeq >> (0 * 8));
        var clkSeqHi = (byte)((clockSeq >> (1 * 8)) & ~0xC0);
        ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
        clkSeqHi_Var = (byte)(clkSeqHi_Var & 0xC0 | clkSeqHi);
    }

    public sealed override unsafe byte[] GetNodeId(ref Guid guid)
    {
        const int nodeIdSize = 6;
        var nodeId = new byte[nodeIdSize];
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        guid.NodeId().CopyTo((Span<byte>)nodeId);
#else
        fixed (byte* pGuidNodeId = &guid.NodeId(0), pNodeId = &nodeId[0])
        {
            Buffer.MemoryCopy(pGuidNodeId, pNodeId, nodeIdSize, nodeIdSize);
        }
#endif
        return nodeId;
    }

    public sealed override void SetNodeId(ref Guid guid, byte[] nodeId)
    {
        guid.SetNodeId(nodeId);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public sealed override void WriteNodeId(ref Guid guid, Span<byte> destination)
    {
        guid.NodeId().CopyTo(destination);
    }

    public sealed override void SetNodeId(ref Guid guid, ReadOnlySpan<byte> nodeId)
    {
        guid.SetNodeId(nodeId);
    }
#endif

    internal sealed new class Version1 : TimeNodeBasedGuidComponents
    {
        internal static readonly TimeNodeBasedGuidComponents.Version1 Instance =
            new TimeNodeBasedGuidComponents.Version1();

        private Version1() { }

        protected override long GetTimestampCore(ref Guid guid)
        {
            return (long)(
                ((ulong)guid.TimeLow() << (0 * 8)) |
                ((ulong)guid.TimeMid() << (4 * 8)) |
                ((ulong)(guid.TimeHi_Ver() & ~0xF000) << (6 * 8)));
        }

        protected override void SetTimestampCore(ref Guid guid, long timestamp)
        {
            guid.TimeLow() = (uint)(timestamp >> (0 * 8));
            guid.TimeMid() = (ushort)(timestamp >> (4 * 8));
            var timeHi = (ushort)((timestamp >> (6 * 8)) & ~0xF000);
            ref var timeHi_Ver = ref guid.TimeHi_Ver();
            timeHi_Ver = (ushort)(timeHi_Ver & 0xF000 | timeHi);
        }
    }

    internal sealed new class Version6 : TimeNodeBasedGuidComponents
    {
        internal static readonly TimeNodeBasedGuidComponents.Version6 Instance =
            new TimeNodeBasedGuidComponents.Version6();

        private Version6() { }

        protected override long GetTimestampCore(ref Guid guid)
        {
            return (long)(
                ((ulong)guid.TimeLow() << (4 * 8 - 4)) |
                ((ulong)guid.TimeMid() << (2 * 8 - 4)) |
                ((ulong)(guid.TimeHi_Ver() & ~0xF000) << (0 * 8)));
        }

        protected override void SetTimestampCore(ref Guid guid, long timestamp)
        {
            guid.TimeLow() = (uint)(timestamp >> (4 * 8 - 4));
            guid.TimeMid() = (ushort)(timestamp >> (2 * 8 - 4));
            var timeHi = (ushort)((timestamp >> (0 * 8)) & ~0xF000);
            ref var timeHi_Ver = ref guid.TimeHi_Ver();
            timeHi_Ver = (ushort)(timeHi_Ver & 0xF000 | timeHi);
        }
    }
}
