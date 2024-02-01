#if !UUIDREV_DISABLE
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal abstract class ClockResetCounter
{
    internal const int SequenceMask = ~(-1 << 14);

    internal const int CounterGuard = 1 << 10;

    private ClockResetCounter() { }

    internal static ClockResetCounter GetInstance(bool isGlobal)
    {
        return isGlobal switch
        {
            false => ClockResetCounter.Local.Instance,
            true => ClockResetCounter.Global.Instance,
        };
    }

    public abstract bool TryGetSequence(
        ref Guid newGuid, long timestamp, short clock, out short sequence);

    private sealed class Local : ClockResetCounter
    {
        internal static readonly ClockResetCounter.Local Instance =
            new ClockResetCounter.Local();

        [ThreadStatic] private static long LastTimestamp;

        [ThreadStatic] private static short LastClock;

        [ThreadStatic] private static short Sequence;

        private Local() { }

        public override bool TryGetSequence(
            ref Guid newGuid, long timestamp, short clock, out short sequence)
        {
            if (!this.TryUpdateTimestamp(timestamp))
            {
                sequence = (short)0;
                return false;
            }

            var lastClock = Local.LastClock;
            if (clock < lastClock)
            {
                sequence = (short)0;
                return false;
            }

            if (clock == lastClock)
            {
                var nextSeq = Local.Sequence + 1;
                if (nextSeq > SequenceMask)
                {
                    sequence = (short)0;
                    return false;
                }
                Local.Sequence = (short)nextSeq;
                sequence = (short)nextSeq;
            }
            else
            {
                var clkSeqHi = newGuid.ClkSeqHi_Var() << 8;
                var guidClkSeq = clkSeqHi | newGuid.ClkSeqLow();
                var newSeq = guidClkSeq & SequenceMask & ~CounterGuard;
                Local.Sequence = (short)newSeq;
                sequence = (short)newSeq;
            }
            Local.LastClock = clock;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryUpdateTimestamp(long timestamp)
        {
            if (timestamp != Local.LastTimestamp)
            {
                if (timestamp == Local.LastTimestamp - 1)
                {
                    return false;
                }
                Local.LastTimestamp = timestamp;
                Local.LastClock = -1;
            }
            return true;
        }
    }

    private sealed class Global : ClockResetCounter
    {
        internal static readonly ClockResetCounter.Global Instance =
            new ClockResetCounter.Global();

        private long Volatile_LastTimestamp;

        // InitFlag: 31, LastClock: 27~16, Sequence: 14~0.
        private volatile int SequenceState;

        private Global()
        {
            this.LastTimestamp = 0;
            this.SequenceState = -1;
        }

        private long LastTimestamp
        {
            get => Volatile.Read(ref this.Volatile_LastTimestamp);
            set => Volatile.Write(ref this.Volatile_LastTimestamp, value);
        }

        public override bool TryGetSequence(
            ref Guid newGuid, long timestamp, short clock, out short sequence)
        {
            if (!this.TryUpdateTimestamp(timestamp))
            {
                sequence = (short)0;
                return false;
            }

            var initLastClock = -1;
            var spinner = new SpinWait();
            while (true)
            {
                var state = this.SequenceState;
                var lastClock = (short)(state >> 16);
                if (initLastClock == -1)
                {
                    initLastClock = lastClock;
                }
                if (lastClock != initLastClock)
                {
                    sequence = (short)0;
                    return false;
                }
                if (clock < lastClock)
                {
                    sequence = (short)0;
                    return false;
                }

                if (clock == lastClock)
                {
                    var nextState = state + 1;
                    var nextSeq = (int)(short)nextState;
                    if (nextSeq > SequenceMask)
                    {
                        sequence = (short)0;
                        return false;
                    }
                    if (Interlocked.CompareExchange(
                        ref this.SequenceState, nextState, state) == state)
                    {
                        sequence = (short)nextSeq;
                        return true;
                    }
                }
                else
                {
                    var clkSeqHi = newGuid.ClkSeqHi_Var() << 8;
                    var guidClkSeq = clkSeqHi | newGuid.ClkSeqLow();
                    var newSeq = guidClkSeq & SequenceMask & ~CounterGuard;
                    var nextState = ((int)clock << 16) | newSeq;
                    if (Interlocked.CompareExchange(
                        ref this.SequenceState, nextState, state) == state)
                    {
                        sequence = (short)newSeq;
                        return true;
                    }
                }
                spinner.SpinOnce();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryUpdateTimestamp(long timestamp)
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            bool TryUpdateCore(long timestamp)
            {
                if (timestamp == this.LastTimestamp)
                {
                    return true;
                }
                if (timestamp == this.LastTimestamp - 1)
                {
                    return false;
                }
                this.LastTimestamp = timestamp;
                this.SequenceState = -1;
                return true;
            }

            return (timestamp == this.LastTimestamp) ||
                TryUpdateCore(timestamp);
        }
    }
}
#endif
