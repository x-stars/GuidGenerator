using System;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
#endif

namespace XNetEx.Guids;

static partial class GuidExtensions
{
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
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return GuidExtensions.ParseUrn((ReadOnlySpan<char>)input);
#else
        var guidUrnString = input.Trim();
        if (!guidUrnString.StartsWith("urn:uuid:",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new FormatException("Guid URN should start with \"urn:uuid:\".");
        }
        var guidString = guidUrnString[9..];
        return Guid.ParseExact(guidString, "D");
#endif
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
        var guidUrnString = input.Trim();
        if (!guidUrnString.StartsWith("urn:uuid:",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new FormatException("Guid URN should start with \"urn:uuid:\".");
        }
        var guidString = guidUrnString[9..];
        return Guid.ParseExact(guidString, "D");
    }
#endif

    /// <summary>
    /// Converts the string representation of a GUID to the equivalent
    /// <see cref="Guid"/> structure, provided that the string is in the URN format.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <param name="guid">When this method returns <see langword="true"/>,
    /// contains the parsed <see cref="Guid"/> value.</param>
    /// <returns><see langword="true"/> if the parse operation was successful;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryParseUrn(string input, out Guid guid)
    {
        if (input is null)
        {
            guid = default(Guid);
            return false;
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return GuidExtensions.TryParseUrn((ReadOnlySpan<char>)input, out guid);
#else
        var guidUrnString = input.Trim();
        if (!guidUrnString.StartsWith("urn:uuid:",
                StringComparison.OrdinalIgnoreCase))
        {
            guid = default(Guid);
            return false;
        }
        var guidString = guidUrnString[9..];
        return Guid.TryParseExact(guidString, "D", out guid);
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Converts the character span representation of a GUID to the equivalent
    /// <see cref="Guid"/> structure, provided that the string is in the URN format.
    /// </summary>
    /// <param name="input">A read-only span containing
    /// the characters representing the GUID to convert.</param>
    /// <param name="guid">When this method returns <see langword="true"/>,
    /// contains the parsed <see cref="Guid"/> value.</param>
    /// <returns><see langword="true"/> if the parse operation was successful;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryParseUrn(ReadOnlySpan<char> input, out Guid guid)
    {
        var guidUrnString = input.Trim();
        if (!guidUrnString.StartsWith("urn:uuid:",
                StringComparison.OrdinalIgnoreCase))
        {
            guid = default(Guid);
            return false;
        }
        var guidString = guidUrnString[9..];
        return Guid.TryParseExact(guidString, "D", out guid);
    }
#endif

    /// <summary>
    /// Returns a URN string representation
    /// of the value of this <see cref="Guid"/> instance.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <returns>The value of this <see cref="Guid"/>, represented as a series
    /// of lowercase hexadecimal digits in the URN format.</returns>
    public static string ToUrnString(this Guid guid)
    {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        const int urnFormatLength = 9 + 36;
        var guidUrnBuffer = (stackalloc char[urnFormatLength]);
        var result = guid.TryFormatUrn(guidUrnBuffer, out var charsWritten);
        Debug.Assert(result && (charsWritten == urnFormatLength));
        return new string(guidUrnBuffer);
#else
        return "urn:uuid:" + guid.ToString("D");
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the <see cref="Guid"/> instance
    /// into the provided character span in the URN format.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="destination">The span in which
    /// to write the <see cref="Guid"/> as a span of characters.</param>
    /// <param name="charsWritten">When this method returns <see langword="true"/>,
    /// contains the number of characters written into the span.</param>
    /// <returns><see langword="true"/> if the formatting operation was successful;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryFormatUrn(
        this Guid guid, Span<char> destination, out int charsWritten)
    {
        const int urnFormatLength = 9 + 36;
        if (destination.Length < urnFormatLength)
        {
            charsWritten = 0;
            return false;
        }
        ((ReadOnlySpan<char>)"urn:uuid:").CopyTo(destination);
        var guidBuffer = destination[9..urnFormatLength];
        var result = guid.TryFormat(guidBuffer, out int guidCharsWritten, "D");
        Debug.Assert(result && (guidCharsWritten == 36));
        charsWritten = urnFormatLength;
        return true;
    }
#endif
}
