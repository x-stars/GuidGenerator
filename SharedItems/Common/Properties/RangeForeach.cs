// Copyright (c) 2022 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

// Provides the range-foreach syntax for C# 9.0 or higher.
// Requires: `System.Index` struct, `System.Range` struct.
// Reference this file to write foreach loops like this:
//   foreach (var index in 0..100) { }
// which is equivalent to the traditional for loop below:
//   for (int index = 0; index < 100; index++) { }
// Use the `Step` method to write foreach loops like this:
//   foreach (var index in (99..^1).Step(-2)) { }
// which is equivalent to the traditional for loop below:
//   for (int index = 99; index > -1; index -= 2) { }
// TIPS: `0` can be omitted in range expressions,
//       e.g. `..100` (equivalent to `0..100`).
// NOTE: Use `^` to represent negative numbers,
//       e.g. `^100..0` (instead of `-100..0`).

#pragma warning disable
#nullable enable

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[DebuggerNonUserCode, ExcludeFromCodeCoverage]
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("This type supports the range-foreach syntax " +
          "and should not be used directly in user code.")]
internal static class __RangeEnumerable
{
    public static Enumerator GetEnumerator(this Range range)
    {
        return new Enumerator(in range);
    }

    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    public struct Enumerator
    {
        private int CurrentIndex;

        private readonly int EndIndex;

        private readonly long Padding__;

#if NET45_OR_GREATER || NETCOREAPP || NETSTANDARD
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal Enumerator(in Range range)
        {
            this.CurrentIndex = range.Start.GetOffset(0) - 1;
            this.EndIndex = range.End.GetOffset(0);
            this.Padding__ = default(long);
        }

        public int Current => this.CurrentIndex;

#if NET45_OR_GREATER || NETCOREAPP || NETSTANDARD
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool MoveNext() => ++this.CurrentIndex < this.EndIndex;
    }

    public static Stepped Step(this Range range, int step)
    {
        return new Stepped(range, step);
    }

    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    public readonly struct Stepped
    {
        public readonly Range Range;

        public readonly int Step;

#if NET45_OR_GREATER || NETCOREAPP || NETSTANDARD
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public Stepped(Range range, int step)
        {
            if (step == 0) { Stepped.ThrowStepOutOfRange(); }
            this.Range = range; this.Step = step;
        }

        public Enumerator GetEnumerator() => new Enumerator(in this);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowStepOutOfRange() =>
            throw new ArgumentOutOfRangeException("step", "Non-zero number required.");

        [DebuggerNonUserCode, ExcludeFromCodeCoverage]
        public struct Enumerator
        {
            private int CurrentIndex;

            private readonly int EndIndex;

            private readonly int StepSign;

            private readonly int StepValue;

#if NET45_OR_GREATER || NETCOREAPP || NETSTANDARD
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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

#if NET45_OR_GREATER || NETCOREAPP || NETSTANDARD
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public bool MoveNext() =>
                (this.CurrentIndex += this.StepValue) < this.EndIndex;
        }
    }
}

#if !(EXCLUDE_FROM_CODE_COVERAGE_ATTRIBUTE || NETCOREAPP3_0_OR_GREATER)
#if !(NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    // Excludes the attributed code from code coverage information.
    internal sealed partial class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}
#endif
#endif
