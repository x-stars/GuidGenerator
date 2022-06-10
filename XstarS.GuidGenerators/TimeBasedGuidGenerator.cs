﻿using System;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal class TimeBasedGuidGenerator : GuidGenerator, IGuidGenerator
    {
        private static class Singleton
        {
            internal static readonly TimeBasedGuidGenerator Value =
                new TimeBasedGuidGenerator();
        }

        private readonly TimestampProvider TimestampProvider;

        private readonly NodeIdProvider NodeIdProvider;

        private long LastTimestamp;

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
            this.ClockSequence = GlobalRandom.Next();
        }

        internal static TimeBasedGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => TimeBasedGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Version1;

        protected virtual int TimestampShift => 0;

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
            this.FillNodeIdField(ref guid);
            this.FillTimeRelatedFields(ref guid);
            this.FillVersionField(ref guid);
            this.FillVariantField(ref guid);
            return guid;
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

        private unsafe void UpdateLastNodeIdBytes()
        {
            lock (this.TimestampProvider)
            {
                var nodeId = this.NodeIdBytes;
                var lastNodeId = this.LastNodeIdBytes;
                if ((nodeId != lastNodeId) && (lastNodeId is not null))
                {
                    fixed (byte* pLast = &lastNodeId[0], pNode = &nodeId[0])
                    {
                        var equals = (*(int*)pNode == *(int*)pLast) &&
                            (*((short*)pLast + 2) == *((short*)pLast + 2));
                        if (!equals) { this.ClockSequence = GlobalRandom.Next(); }
                    }
                }
                this.LastNodeIdBytes = nodeId;
            }
        }
    }
}
