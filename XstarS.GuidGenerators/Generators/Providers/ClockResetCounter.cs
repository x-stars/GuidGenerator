#if !FEATURE_DISABLE_UUIDREV
using System;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal abstract class ClockResetCounter
{
    internal const int SequenceMask = ~(-1 << 14);

    internal const int CounterLimit = 1 << 12;

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
        ref Guid newGuid, int timestamp, out short sequence);

    private sealed class Local : ClockResetCounter
    {
        internal static readonly ClockResetCounter.Local Instance =
            new ClockResetCounter.Local();

        [ThreadStatic] private static int LastTimestamp;

        [ThreadStatic] private static int BaseSequence;

        [ThreadStatic] private static int Counter;

        private Local() { }

        public override bool TryGetSequence(
            ref Guid newGuid, int timestamp, out short sequence)
        {
            var lastTs = Local.LastTimestamp;
            if (timestamp == lastTs)
            {
                var counter = Local.Counter + 1;
                if (counter >= CounterLimit)
                {
                    sequence = (short)0;
                    return false;
                }
                Local.Counter = counter;
                var baseSeq = Local.BaseSequence;
                sequence = (short)(baseSeq + counter);
            }
            else
            {
                var clkSeqHi = newGuid.ClkSeqHi_Var() << 8;
                var guidClkSeq = clkSeqHi | newGuid.ClkSeqLow();
                var newBaseSeq = guidClkSeq & SequenceMask & ~CounterLimit;
                Local.BaseSequence = newBaseSeq;
                Local.Counter = 0;
                sequence = (short)newBaseSeq;
            }
            Local.LastTimestamp = timestamp;
            return true;
        }
    }

    private sealed class Global : ClockResetCounter
    {
        internal static readonly ClockResetCounter.Global Instance =
            new ClockResetCounter.Global();

        // InitFlag: 63, LastTs: 62~32, BaseSeq: 32~16, Counter: 15~0.
        private /*volatile*/ long SequenceState;

        private Global()
        {
            this.SequenceState = -1L;
        }

        public override bool TryGetSequence(
            ref Guid newGuid, int timestamp, out short sequence)
        {
            var initLastTs = -1;
            var spinner = new SpinWait();
            while (true)
            {
                var state = Volatile.Read(ref this.SequenceState);
                var lastTs = (int)(state >> 32);
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
                    var counter = (ushort)nextState;
                    if (counter >= CounterLimit)
                    {
                        sequence = (short)0;
                        return false;
                    }
                    if (Interlocked.CompareExchange(
                        ref this.SequenceState, nextState, state) == state)
                    {
                        var baseSeq = (ushort)(nextState >> 16);
                        sequence = (short)(baseSeq + counter);
                        return true;
                    }
                }
                else
                {
                    var clkSeqHi = newGuid.ClkSeqHi_Var() << 8;
                    var guidClkSeq = clkSeqHi | newGuid.ClkSeqLow();
                    var newBaseSeq = guidClkSeq & SequenceMask & ~CounterLimit;
                    var nextState = ((long)timestamp << 32) | ((long)newBaseSeq << 16);
                    if (Interlocked.CompareExchange(
                        ref this.SequenceState, nextState, state) == state)
                    {
                        sequence = (short)newBaseSeq;
                        return true;
                    }
                }

                spinner.SpinOnce();
            }
        }
    }
}
#endif
