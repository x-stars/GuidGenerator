// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !(NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
namespace System
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents a type that can be used to index a collection either from the start or the end.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    internal readonly struct Index : IEquatable<Index>
    {
        private readonly int _value;

        /// <summary>
        /// Initializes a new <see cref="Index"/> with a specified index position
        /// and a value that indicates if the index is from the start or the end of a collection.
        /// </summary>
        /// <param name="value">The index value. It has to be greater or equal than zero.</param>
        /// <param name="fromEnd">A boolean indicating if the index is from the start
        /// (<see langword="false"/>) or from the end (<see langword="true"/>) of a collection.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Index(int value, bool fromEnd = false)
        {
            if (value < 0) { throw Index.ValueOutOfRange(); }
            this._value = fromEnd ? ~value : value;
        }

        private Index(int value) { this._value = value; }

        /// <summary>
        /// Gets an <see cref="Index"/> that points to the first element of a collection.
        /// </summary>
        /// <returns>An instance that points to the first element of a collection.</returns>
        public static Index Start => new Index(0);

        /// <summary>
        /// Gets an <see cref="Index"/> that points beyond the last element.
        /// </summary>
        /// <returns>an <see cref="Index"/> that points beyond the last element.</returns>
        public static Index End => new Index(~0);

        /// <summary>
        /// Create an <see cref="Index"/> from the specified index at the start of a collection.
        /// </summary>
        /// <param name="value">The index position from the start of a collection.</param>
        /// <returns>The Index value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromStart(int value) =>
            (value < 0) ? throw Index.ValueOutOfRange() : new Index(value);

        /// <summary>
        /// Creates an <see cref="Index"/> from the end of a collection at a specified index position.
        /// </summary>
        /// <param name="value">The index value from the end of a collection.</param>
        /// <returns>The Index value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromEnd(int value) =>
            (value < 0) ? throw Index.ValueOutOfRange() : new Index(~value);

        /// <summary>
        /// Gets the index value.
        /// </summary>
        /// <returns>The index value.</returns>
        public int Value => (this._value < 0) ? ~this._value : this._value;

        /// <summary>
        /// Gets a value that indicates whether the index is from the start or the end.
        /// </summary>
        /// <returns><see langword="true"/> if the Index is from
        /// the end; otherwise, <see langword="false"/>.</returns>
        public bool IsFromEnd => this._value < 0;

        /// <summary>
        /// Calculates the offset from the start of the collection using the given collection length.
        /// </summary>
        /// <param name="length">The length of the collection that
        /// the Index will be used with. Must be a positive value.</param>
        /// <returns>The offset.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOffset(int length)
        {
            int offset = this._value;
            if (this.IsFromEnd) { offset += length + 1; }
            return offset;
        }

        /// <summary>
        /// Indicates whether the current Index object is equal to a specified object.
        /// </summary>
        /// <param name="value">An object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is of type <see cref="Index"/>
        /// and is equal to the current instance; <see langword="false"/> otherwise.</returns>
        public override bool Equals(object? value) => (value is Index other) && this.Equals(other);

        /// <summary>
        /// Returns a value that indicates whether
        /// the current object is equal to another <see cref="Index"/> object.
        /// </summary>
        /// <param name="other">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if the current Index object is equal to
        /// <paramref name="other"/>; <see langword="false"/> otherwise.</returns>
        public bool Equals(Index other) => this._value == other._value;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() => this._value;

        /// <summary>
        /// Converts integer number to an Index.
        /// </summary>
        /// <param name="value">The integer to convert.</param>
        /// <returns>An Index representing the integer.</returns>
        public static implicit operator Index(int value) => Index.FromStart(value);

        /// <summary>
        /// Returns the string representation of the current <see cref="Index"/> instance.
        /// </summary>
        /// <returns>The string representation of the <see cref="Index"/>.</returns>
        public override string ToString() =>
            this.IsFromEnd ? this.ToStringFromEnd() : ((uint)this.Value).ToString();

        private string ToStringFromEnd() => "^" + this.Value.ToString();

        private static ArgumentOutOfRangeException ValueOutOfRange() =>
            new ArgumentOutOfRangeException("value", "Non-negative number required.");
    }

    /// <summary>
    /// Represents a range that has start and end indexes.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    internal readonly struct Range : IEquatable<Range>
    {
        /// <summary>
        /// Gets the inclusive start index of the <see cref="Range"/>.
        /// </summary>
        /// <returns>The inclusive start index of the range.</returns>
        public Index Start { get; }

        /// <summary>
        /// Gets an <see cref="Index"/> that represents the exclusive end index of the range.
        /// </summary>
        /// <returns>The end index of the range.</returns>
        public Index End { get; }

        /// <summary>
        /// Instantiates a new <see cref="Range"/> instance with the specified starting and ending indexes.
        /// </summary>
        /// <param name="start">The inclusive start index of the range.</param>
        /// <param name="end">The exclusive end index of the range.</param>
        public Range(Index start, Index end) { this.Start = start; this.End = end; }

        /// <summary>
        /// Returns a value that indicates whether
        /// the current instance is equal to a specified object.
        /// </summary>
        /// <param name="value">An object to compare with this Range object.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is of type <see cref="Range"/>
        /// and is equal to the current instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? value) => (value is Range other) && this.Equals(other);

        /// <summary>
        /// Returns a value that indicates whether
        /// the current instance is equal to another <see cref="Range"/> object.
        /// </summary>
        /// <param name="other">A Range object to compare with this Range object.</param>
        /// <returns><see langword="true"/> if the current instance is equal to
        /// <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Range other) => this.Start.Equals(other.Start) && this.End.Equals(other.End);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() => this.Start.GetHashCode() * 31 + this.End.GetHashCode();

        /// <summary>
        /// Returns the string representation of the current <see cref="Range"/> object.
        /// </summary>
        /// <returns>The string representation of the range.</returns>
        public override string ToString() => this.Start.ToString() + ".." + this.End.ToString();

        /// <summary>
        /// Returns a new <see cref="Range"/> instance starting from
        /// a specified start index to the end of the collection.
        /// </summary>
        /// <param name="start">The position of the first element from
        /// which the Range will be created.</param>
        /// <returns>A range from <paramref name="start"/> to the end of the collection.</returns>
        public static Range StartAt(Index start) => new Range(start, Index.End);

        /// <summary>
        /// Creates a <see cref="Range"/> object starting from
        /// the first element in the collection to a specified end index.
        /// </summary>
        /// <param name="end">The position of the last element up to
        /// which the <see cref="Range"/> object will be created.</param>
        /// <returns>A range that starts from the first element to <paramref name="end"/>.</returns>
        public static Range EndAt(Index end) => new Range(Index.Start, end);

        /// <summary>
        /// Gets a <see cref="Range"/> object that starts from the first element to the end.
        /// </summary>
        /// <returns>A range from the start to the end.</returns>
        public static Range All => new Range(Index.Start, Index.End);

#if VALUE_TUPLE || NET47_OR_GREATER || NETCOREAPP || NETSTANDARD2_0_OR_GREATER
        /// <summary>
        /// Calculates the start offset and length of the range object using a collection length.
        /// </summary>
        /// <param name="length">A positive integer that represents
        /// the length of the collection that the range will be used with.</param>
        /// <returns>The start offset and length of the range.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> is outside the bounds of the current range.</exception>
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

#if !(NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER)
#if !(EXCLUDE_FROM_CODE_COVERAGE || NETCOREAPP3_0_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    [SuppressMessage("Microsoft.Design",
                     "CA1018:MarkAttributesWithAttributeUsageAttribute")]
    internal sealed partial class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}
#endif
#endif
