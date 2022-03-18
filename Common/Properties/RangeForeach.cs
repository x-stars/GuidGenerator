// Copyright (c) 2022 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

// Provide the range-foreach syntax for C# 9.0 or higher.
// Requires: Framework >= 4.0 || Core || Standard.
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
[Obsolete(RangeEnumerable.NoDirectUsageMessage)]
internal static class RangeEnumerable
{
    internal const string NoDirectUsageMessage =
        "This type supports the range-foreach syntax " +
        "and should not be used directly in user code.";

    public static Enumerator GetEnumerator(this Range range)
    {
        return new Enumerator(range);
    }

#if STEPPED_RANGE
    public static Stepped Step(this Range range, int step)
    {
        return new Stepped(range, step);
    }
#endif

    [CompilerGenerated, DebuggerNonUserCode]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(RangeEnumerable.NoDirectUsageMessage)]
    public struct Enumerator
    {
        private int CurrentIndex;

        private readonly int EndIndex;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(Range range)
        {
            this.CurrentIndex = range.Start.GetOffset(0) - 1;
            this.EndIndex = range.End.GetOffset(0);
        }

        public int Current => this.CurrentIndex;

        public bool MoveNext() => ++this.CurrentIndex < this.EndIndex;
    }

#if STEPPED_RANGE
    [CompilerGenerated, DebuggerNonUserCode]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(RangeEnumerable.NoDirectUsageMessage)]
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

        public Enumerator GetEnumerator() => new Enumerator(this);

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
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(RangeEnumerable.NoDirectUsageMessage)]
        public struct Enumerator
        {
            private int CurrentIndex;

            private readonly int EndIndex;

            private readonly int StepSign;

            private readonly int StepValue;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(Stepped stepped)
            {
                var range = stepped.Range;
                int start = range.Start.GetOffset(0);
                int end = range.End.GetOffset(0);
                int step = stepped.Step;
                int sign = step >> 31;
                this.CurrentIndex = (start ^ sign) - 1;
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

#if !(INDEX_RANGE || NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// NOTE: Some APIs are disabled by default for compatibility reasons.

namespace System
{
    [CompilerGenerated, DebuggerNonUserCode]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(RangeEnumerable.NoDirectUsageMessage)]
    internal readonly struct Index : IEquatable<Index>
    {
        private readonly int _value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Index(int value, bool fromEnd = false)
        {
            if (value < 0) { throw Index.ValueOutOfRange(); }
            this._value = fromEnd ? ~value : value;
        }

        private Index(int value) { this._value = value; }

        public static Index Start => new Index(0);

        public static Index End => new Index(~0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromStart(int value) =>
            (value < 0) ? throw Index.ValueOutOfRange() : new Index(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromEnd(int value) =>
            (value < 0) ? throw Index.ValueOutOfRange() : new Index(~value);

        public int Value => (this._value < 0) ? ~this._value : this._value;

        public bool IsFromEnd => this._value < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOffset(int length)
        {
            int offset = this._value;
            if (this.IsFromEnd) { offset += length + 1; }
            return offset;
        }

        public override bool Equals(object value) =>
            (value is Index other) && this.Equals(other);

        public bool Equals(Index other) => this._value == other._value;

        public override int GetHashCode() => this._value;

        public static implicit operator Index(int value) => Index.FromStart(value);

        public override string ToString() =>
            this.IsFromEnd ? this.ToStringFromEnd() : ((uint)this.Value).ToString();

        private string ToStringFromEnd() => "^" + this.Value.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArgumentOutOfRangeException ValueOutOfRange() =>
            new ArgumentOutOfRangeException("value", "Non-negative number required.");
    }

    [CompilerGenerated, DebuggerNonUserCode]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(RangeEnumerable.NoDirectUsageMessage)]
    internal readonly struct Range : IEquatable<Range>
    {
        public Index Start { get; }

        public Index End { get; }

        public Range(Index start, Index end) { this.Start = start; this.End = end; }

        public override bool Equals(object value) =>
            (value is Range other) && this.Equals(other);

        public bool Equals(Range other) =>
            this.Start.Equals(other.Start) && this.End.Equals(other.End);

        public override int GetHashCode() =>
            this.Start.GetHashCode() * 31 + this.End.GetHashCode();

        public override string ToString() =>
            this.Start.ToString() + ".." + this.End.ToString();

        public static Range StartAt(Index start) => new Range(start, Index.End);

        public static Range EndAt(Index end) => new Range(Index.Start, end);

        public static Range All => new Range(Index.Start, Index.End);

#if VALUE_TUPLE || NET47_OR_GREATER || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (int Offset, int Length) GetOffsetAndLength(int length)
        {
            int start = this.Start.GetOffset(length);
            int end = this.End.GetOffset(length);
            if ((uint)end > (uint)length || (uint)start > (uint)end)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            return (start, end - start);
        }
#endif
    }
}
#endif
