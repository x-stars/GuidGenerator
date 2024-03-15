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
        long timestamp, short clock, short initSeq, out short sequence);

    private sealed class Local : ClockResetCounter
    {
        internal static readonly ClockResetCounter.Local Instance = new();

        [ThreadStatic] private static long LastTimestamp;

        [ThreadStatic] private static short LastClock;

        [ThreadStatic] private static short Sequence;

        private Local() { }

        public override bool TryGetSequence(
            long timestamp, short clock, short initSeq, out short sequence)
        {
            this.UpdateTimestamp(timestamp);
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
                var newSeq = initSeq & SequenceMask & ~CounterGuard;
                Local.Sequence = (short)newSeq;
                sequence = (short)newSeq;
            }
            Local.LastClock = clock;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateTimestamp(long timestamp)
        {
            if (timestamp != Local.LastTimestamp)
            {
                Local.LastTimestamp = timestamp;
                Local.LastClock = -1;
            }
        }
    }

    private sealed class Global : ClockResetCounter
    {
        internal static readonly ClockResetCounter.Global Instance = new();

        private static readonly int RollbackLimit = Environment.ProcessorCount;

        private long Volatile_LastTimestamp;

        // InitFlag: 31, LastClock: 27~16, Sequence: 14~0.
        private volatile int SequenceState;

        private volatile int RollbackCount;

        private Global()
        {
            this.LastTimestamp = 0;
            this.SequenceState = -1;
            this.RollbackCount = 0;
        }

        private long LastTimestamp
        {
            get => Volatile.Read(ref this.Volatile_LastTimestamp);
            set => Volatile.Write(ref this.Volatile_LastTimestamp, value);
        }

        public override bool TryGetSequence(
            long timestamp, short clock, short initSeq, out short sequence)
        {
            var initLastClock = -1;
            var spinner = new SpinWait();
            while (true)
            {
                if (!this.TryUpdateTimestamp(timestamp, out var state))
                {
                    sequence = (short)0;
                    return false;
                }
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
                    var newSeq = initSeq & SequenceMask & ~CounterGuard;
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
        private bool TryUpdateTimestamp(long timestamp, out int state)
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            bool TryUpdateCore(long timestamp, out int state)
            {
                if (timestamp == this.LastTimestamp)
                {
                    this.RollbackCount = 0;
                    return (state = this.SequenceState) >= 0;
                }
                if ((timestamp - this.LastTimestamp) is > -10 and < 0)
                {
                    return (state = -1) >= 0;
                }
                if ((timestamp < this.LastTimestamp) &&
                    (Interlocked.Increment(ref this.RollbackCount) < RollbackLimit))
                {
                    return (state = -1) >= 0;
                }
                this.LastTimestamp = timestamp;
                this.SequenceState = state = -1;
                this.RollbackCount = 0;
                return true;
            }

            if (timestamp == this.LastTimestamp)
            {
                this.RollbackCount = 0;
                return (state = this.SequenceState) >= 0;
            }
            return TryUpdateCore(timestamp, out state);
        }
    }
}
#endif
