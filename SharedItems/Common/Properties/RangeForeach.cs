// Copyright (c) 2022 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

// Provide the range-foreach syntax for C# 9.0 or higher.
// Requires: System.Index and System.Range types.
// Reference this file to write foreach-loops like this:
//   foreach (var index in 0..100) { /* ... */ }
// which is equivalent to the legacy for-loop below:
//   for (int i = 0; i < 100; i++) { /* ... */ }
// NOTE: Use '^' to represent negative numbers,
//       e.g. ^100..0 (instead of -100..0).
// If STEPPED_RANGE is defined, this can also be used:
//   foreach (var index in (99..^1).Step(-2)) { /* ... */ }
// which is equivalent to the legacy for-loop below:
//   for (int i = 100 - 1; i >= 0; i += -2) { /* ... */ }

#nullable disable
//#define STEPPED_RANGE

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[CompilerGenerated, DebuggerNonUserCode]
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("This type supports the range-foreach syntax " +
          "and should not be used directly in user code.")]
internal static class RangeEnumerable
{
    public static Enumerator GetEnumerator(this Range range)
    {
        return new Enumerator(in range);
    }

#if STEPPED_RANGE
    public static Stepped Step(this Range range, int step)
    {
        return new Stepped(range, step);
    }
#endif

    [CompilerGenerated, DebuggerNonUserCode]
    public struct Enumerator
    {
        private int CurrentIndex;

        private readonly int EndIndex;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(in Range range)
        {
            this.CurrentIndex = range.Start.GetOffset(0) - 1;
            this.EndIndex = range.End.GetOffset(0);
        }

        public int Current => this.CurrentIndex;

        public bool MoveNext() => ++this.CurrentIndex < this.EndIndex;
    }

#if STEPPED_RANGE
    [CompilerGenerated, DebuggerNonUserCode]
    public readonly struct Stepped : IEquatable<Stepped>
    {
        public Range Range { get; }

        public int Step { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Stepped(Range range, int step)
        {
            if (step == 0) { throw Stepped.StepOutOfRange(); }
            this.Range = range; this.Step = step;
        }

        public Enumerator GetEnumerator() => new Enumerator(in this);

        public bool Equals(Stepped other) =>
            this.Range.Equals(other.Range) && (this.Step == other.Step);

        public override bool Equals(object obj) =>
            (obj is Stepped other) && this.Equals(other);

        public override int GetHashCode() =>
            this.Range.GetHashCode() * 31 + this.Step;

        public override string ToString() =>
            this.Range.ToString() + ".%" + this.Step.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArgumentOutOfRangeException StepOutOfRange() =>
            new ArgumentOutOfRangeException("step", "Non-zero number required.");

        [CompilerGenerated, DebuggerNonUserCode]
        public struct Enumerator
        {
            private int CurrentIndex;

            private readonly int EndIndex;

            private readonly int StepSign;

            private readonly int StepValue;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(in Stepped stepped)
            {
                var range = stepped.Range;
                int start = range.Start.GetOffset(0);
                int end = range.End.GetOffset(0);
                int step = stepped.Step;
                int sign = step >> 31;
                this.CurrentIndex = (start - step) ^ sign;
                this.EndIndex = end ^ sign;
                this.StepSign = sign;
                this.StepValue = (step ^ sign) - sign;
            }

            public int Current => this.CurrentIndex ^ this.StepSign;

            public bool MoveNext() =>
                (this.CurrentIndex += this.StepValue) < this.EndIndex;
        }
    }
#endif
}
