using System;

namespace XNetEx.Guids;

/// <summary>
/// Provides RFC 4122 compliant constants and conversion methods for <see cref="Guid"/>.
/// </summary>
public static class Uuid
{
    /// <summary>
    /// A read-only instance of the <see cref="Guid"/> structure whose value is all ones.
    /// </summary>
#if !UUIDREV_DISABLE
    public static readonly Guid MaxValue = new Guid(
#else
    internal static readonly Guid MaxValue = new Guid(
#endif
        // ffffffff-ffff-ffff-ffff-ffffffffffff
        0xffffffff, 0xffff, 0xffff,
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff);

#if NET7_0_OR_GREATER
    /// <summary>
    /// Creates a new <see cref="Guid"/> instance
    /// by using the specified 128-bit unsigned integer.
    /// </summary>
    /// <param name="value">A 128-bit unsigned integer
    /// containing the value of the GUID.</param>
    /// <returns>A new <see cref="Guid"/> instance
    /// of the specified 128-bit unsigned integer.</returns>
    [CLSCompliant(false)]
    public static Guid FromUInt128(UInt128 value)
    {
        return GuidExtensions.FromUInt128(value);
    }
#endif

    /// <summary>
    /// Creates a new <see cref="Guid"/> instance
    /// by using the specified byte array in big-endian order.
    /// </summary>
    /// <param name="bytes">A 16-element byte array
    /// containing the fields of the GUID in big-endian order.</param>
    /// <returns>A new <see cref="Guid"/> instance of the specified byte array.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="bytes"/> is not 16 bytes long.</exception>
    public static Guid FromByteArray(byte[] bytes)
    {
        return GuidExtensions.FromUuidByteArray(bytes);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Creates a new <see cref="Guid"/> instance
    /// by using the specified read-only byte span in big-endian order.
    /// </summary>
    /// <param name="bytes">A 16-element read-only span
    /// containing the bytes of the fields of the GUID in big-endian order.</param>
    /// <returns>A new <see cref="Guid"/> instance of the specified byte span.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="bytes"/> is not 16 bytes long.</exception>
    public static Guid FromBytes(ReadOnlySpan<byte> bytes)
    {
        return GuidExtensions.FromUuidBytes(bytes);
    }
#endif

#if NET7_0_OR_GREATER
    /// <summary>
    /// Returns a 128-bit unsigned integer
    /// that contains the value of the <see cref="Guid"/>.
    /// </summary>
    /// <param name="value">The <see cref="Guid"/> to convert.</param>
    /// <returns>A 128-bit unsigned integer
    /// that contains the value of the <see cref="Guid"/>.</returns>
    [CLSCompliant(false)]
    public static UInt128 ToUInt128(Guid value)
    {
        return GuidExtensions.ToUInt128(value);
    }
#endif

    /// <summary>
    /// Returns a 16-element byte array that contains the fields
    /// of the <see cref="Guid"/> in big-endian order.
    /// </summary>
    /// <param name="value">The <see cref="Guid"/> to convert.</param>
    /// <returns>A 16-element byte array that contains the fields
    /// of the <see cref="Guid"/> in big-endian order.</returns>
    public static byte[] ToByteArray(Guid value)
    {
        return GuidExtensions.ToUuidByteArray(value);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the fields of the <see cref="Guid"/>
    /// into a span of bytes in big-endian order.
    /// </summary>
    /// <param name="value">The <see cref="Guid"/> to write.</param>
    /// <param name="destination">When this method returns <see langword="true"/>,
    /// contains the fields of the <see cref="Guid"/> in big-endian order.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/> is successfully
    /// written to the specified span; otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteBytes(Guid value, Span<byte> destination)
    {
        return GuidExtensions.TryWriteUuidBytes(value, destination);
    }
#endif

    /// <summary>
    /// Converts the string representation of a GUID to the equivalent
    /// <see cref="Guid"/> structure, provided that the string is in the URN format.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>A structure that contains the value that was parsed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="input"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException">
    /// <paramref name="input"/> is not in the URN format.</exception>
    public static Guid ParseUrn(string input)
    {
        return GuidExtensions.ParseUrn(input);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Converts the character span representation of a GUID to the equivalent
    /// <see cref="Guid"/> structure, provided that the string is in the URN format.
    /// </summary>
    /// <param name="input">A read-only span containing
    /// the characters representing the GUID to convert.</param>
    /// <returns>A structure that contains the value that was parsed.</returns>
    /// <exception cref="FormatException">
    /// <paramref name="input"/> is not in the URN format.</exception>
    public static Guid ParseUrn(ReadOnlySpan<char> input)
    {
        return GuidExtensions.ParseUrn(input);
    }
#endif

    /// <summary>
    /// Converts the string representation of a GUID to the equivalent
    /// <see cref="Guid"/> structure, provided that the string is in the URN format.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <param name="result">When this method returns <see langword="true"/>,
    /// contains the parsed <see cref="Guid"/> value.</param>
    /// <returns><see langword="true"/> if the parse operation was successful;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryParseUrn(string input, out Guid result)
    {
        return GuidExtensions.TryParseUrn(input, out result);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Converts the character span representation of a GUID to the equivalent
    /// <see cref="Guid"/> structure, provided that the string is in the URN format.
    /// </summary>
    /// <param name="input">A read-only span containing
    /// the characters representing the GUID to convert.</param>
    /// <param name="result">When this method returns <see langword="true"/>,
    /// contains the parsed <see cref="Guid"/> value.</param>
    /// <returns><see langword="true"/> if the parse operation was successful;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryParseUrn(ReadOnlySpan<char> input, out Guid result)
    {
        return GuidExtensions.TryParseUrn(input, out result);
    }
#endif

    /// <summary>
    /// Returns a URN string representation
    /// of the value of this <see cref="Guid"/> instance.
    /// </summary>
    /// <param name="value">The <see cref="Guid"/> to convert.</param>
    /// <returns>The value of this <see cref="Guid"/>, represented as a series
    /// of lowercase hexadecimal digits in the URN format.</returns>
    public static string ToUrnString(Guid value)
    {
        return GuidExtensions.ToUrnString(value);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the <see cref="Guid"/> instance
    /// into the provided character span in the URN format.
    /// </summary>
    /// <param name="value">The <see cref="Guid"/> to write.</param>
    /// <param name="destination">The span in which
    /// to write the <see cref="Guid"/> as a span of characters.</param>
    /// <param name="charsWritten">When this method returns <see langword="true"/>,
    /// contains the number of characters written into the span.</param>
    /// <returns><see langword="true"/> if the formatting operation was successful;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryFormatUrn(
        Guid value, Span<char> destination, out int charsWritten)
    {
        return GuidExtensions.TryFormatUrn(value, destination, out charsWritten);
    }
#endif
}
