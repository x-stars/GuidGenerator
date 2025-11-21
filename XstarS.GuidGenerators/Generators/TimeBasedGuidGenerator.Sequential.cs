#if !UUIDREV_DISABLE
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

partial class TimeBasedGuidGenerator
{
    internal class Sequential : TimeBasedGuidGenerator
    {
        private Sequential(NodeIdSource nodeIdSource) : base(nodeIdSource) { }

        private Sequential(
            NodeIdSource nodeIdSource, short? initClockSeq = null,
            Func<DateTime>? timestampProvider = null, Func<byte[]>? nodeIdProvider = null)
            : base(nodeIdSource, initClockSeq, timestampProvider, nodeIdProvider)
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
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new TimeBasedGuidGenerator.Sequential.Randomized());
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        internal static new TimeBasedGuidGenerator.Sequential InstanceR
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static TimeBasedGuidGenerator.Sequential Initialize()
                {
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new TimeBasedGuidGenerator.Sequential(NodeIdSource.NonVolatileRandom));
                }

                return Volatile.Read(ref field) ?? Initialize();
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
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new TimeBasedGuidGenerator.Sequential(NodeIdSource.PhysicalAddress));
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        public sealed override GuidVersion Version => GuidVersion.Version6;

        internal static new TimeBasedGuidGenerator.Sequential CreateInstanceR()
        {
            return new TimeBasedGuidGenerator.Sequential(NodeIdSource.VolatileRandom);
        }

        internal static new TimeBasedGuidGenerator.Sequential CreateCustomState(
            NodeIdSource nodeIdSource = NodeIdSource.None, short? initClockSeq = null,
            Func<DateTime>? timestampProvider = null, Func<byte[]>? nodeIdProvider = null)
        {
            if (nodeIdSource is NodeIdSource.None)
            {
                return new TimeBasedGuidGenerator.Sequential.Randomized(timestampProvider);
            }

            return new TimeBasedGuidGenerator.Sequential(
                nodeIdSource, initClockSeq, timestampProvider, nodeIdProvider);
        }

        private sealed class Randomized : TimeBasedGuidGenerator.Sequential
        {
            internal Randomized() : base(NodeIdSource.VolatileRandom) { }

            internal Randomized(Func<DateTime>? timestampProvider = null)
                : base(NodeIdSource.VolatileRandom, initClockSeq: null, timestampProvider)
            {
            }

            public override bool TryNewGuid(out Guid result)
            {
                result = default(Guid);
                if (this.TryFillTimeFields(ref result))
                {
                    var newGuid = Guid.NewGuid();
                    result.DataLow() = newGuid.DataLow();
                    result.NodeId(0) |= 0x01;
                    this.FillVersionFieldUnchecked(ref result);
                    Debug.Assert(result.GetVariant() == this.Variant);
                    return true;
                }
                return false;
            }

            private bool TryFillTimeFields(ref Guid guid)
            {
                var state = this.GeneratorState;
                var nodeId = this.NodeIdBytes;
                var timestamp = this.CurrentTimestamp;
                var updated = state.Update(
                    nodeId, timestamp, out var clockSeq);
                if (!updated) { return false; }
                this.FillTimeFieldsChecked(ref guid, timestamp);
                return true;
            }
        }
    }
}
#endif
