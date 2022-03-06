// Copyright (c) 2022 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

// Provide the range-foreach syntax for C# 9.0 or higher.
// Requires: Framework >= 4.0 || Core || Standard.
// Reference this file to write foreach-loops like this:
//   foreach (var index in 0..100) { /* ... */ }
// which is equivalent to the legacy for-loop below:
//   for (int i = 0; i < 100; i++) { /* ... */ }
// NOTE: use '^' to represent negative numbers,
//       e.g. ^100..0 (instead of -100..0).

#nullable disable

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[CompilerGenerated, DebuggerNonUserCode]
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class RangeEnumerable
{
    public static RangeEnumerator GetEnumerator(this Range range)
    {
        return new RangeEnumerator(range);
    }
}

namespace System
{
    [CompilerGenerated, DebuggerNonUserCode]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal struct RangeEnumerator
    {
        private int CurrentIndex;

        private readonly int EndIndex;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RangeEnumerator(Range range)
        {
            var pair = new IndexPair() { Range = range };
            int start = pair.StartIndex, end = pair.EndIndex;
            this.CurrentIndex = start - (start >> 31) - 1;
            this.EndIndex = end - (end >> 31);
        }

        public int Current => this.CurrentIndex;

        public bool MoveNext() => ++this.CurrentIndex < this.EndIndex;

        [StructLayout(LayoutKind.Explicit)]
        [CompilerGenerated, DebuggerNonUserCode]
        private struct IndexPair
        {
            [FieldOffset(0)] public Range Range;
            [FieldOffset(0)] public int StartIndex;
            [FieldOffset(4)] public int EndIndex;
        }
    }
}

#if !(NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// NOTE: some APIs have been removed for code size and compatibility reasons.

namespace System
{
    [CompilerGenerated, DebuggerNonUserCode]
    [EditorBrowsable(EditorBrowsableState.Never)]
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
            (value is Index other) && (this._value == other._value);

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
    internal readonly struct Range : IEquatable<Range>
    {
        public Index Start { get; }

        public Index End { get; }

        public Range(Index start, Index end) { this.Start = start; this.End = end; }

        public override bool Equals(object value) =>
            (value is Range other) && this.Start.Equals(other.Start) && this.End.Equals(other.End);

        public bool Equals(Range other) => this.Start.Equals(other.Start) && this.End.Equals(other.End);

        public override int GetHashCode() => this.Start.GetHashCode() * 31 + this.End.GetHashCode();

        public override string ToString() => this.Start.ToString() + ".." + this.End.ToString();

        public static Range StartAt(Index start) => new Range(start, Index.End);

        public static Range EndAt(Index end) => new Range(Index.Start, end);

        public static Range All => new Range(Index.Start, Index.End);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public (int Offset, int Length) GetOffsetAndLength(int length) { ... }
    }
}
#endif
