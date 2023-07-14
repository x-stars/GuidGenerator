#if !FEATURE_DISABLE_UUIDREV
using System;
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
        ref Guid newGuid, short timestamp, out short sequence);

    private sealed class Local : ClockResetCounter
    {
        internal static readonly ClockResetCounter.Local Instance =
            new ClockResetCounter.Local();

        [ThreadStatic] private static short LastTimestamp;

        [ThreadStatic] private static short Sequence;

        private Local() { }

        public override bool TryGetSequence(
            ref Guid newGuid, short timestamp, out short sequence)
        {
            var lastTs = Local.LastTimestamp;
            if (timestamp == lastTs)
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
            Local.LastTimestamp = timestamp;
            return true;
        }
    }

    private sealed class Global : ClockResetCounter
    {
        internal static readonly ClockResetCounter.Global Instance =
            new ClockResetCounter.Global();

        // InitFlag: 31, LastTs: 30~16, Sequence: 14~0.
        private volatile int SequenceState;

        private Global()
        {
            this.SequenceState = -1;
        }

        public override bool TryGetSequence(
            ref Guid newGuid, short timestamp, out short sequence)
        {
            var initLastTs = -1;
            var spinner = new SpinWait();
            while (true)
            {
                var state = this.SequenceState;
                var lastTs = (short)(state >> 16);
                if (initLastTs == -1)
                {
                    initLastTs = lastTs;
                }
                if (lastTs != initLastTs)
                {
                    sequence = (short)0;
                    return false;
                }

                if (timestamp == lastTs)
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
                    var nextState = ((int)timestamp << 16) | newSeq;
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
    }
}
#endif
