using System;
using System.Diagnostics.CodeAnalysis;
using XNetEx.Guids.Components;

namespace XNetEx.Guids;

static partial class GuidExtensions
{
    /// <summary>
    /// Tries to get the hash data represented by the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="hashData">When this method returns <see langword="true"/>,
    /// contains the hash data represented by the <see cref="Guid"/>.</param>
    /// <param name="bitmask">When this method returns <see langword="true"/>,
    /// contains the bitmask of the hash data represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is name-based; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetHashData(this Guid guid,
        [NotNullWhen(true)] out byte[]? hashData, [NotNullWhen(true)] out byte[]? bitmask)
    {
        if (!guid.GetRfc4122Version().IsNameBased())
        {
            hashData = null;
            bitmask = null;
            return false;
        }

        var components = GuidComponents.FixedFormat;
        hashData = components.GetHashData(ref guid, out bitmask);
        return true;
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the hash data represented by the <see cref="Guid"/> into a span of bytes.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="destination">When this method returns <see langword="true"/>,
    /// contains the hash data represented by the <see cref="Guid"/>.</param>
    /// <param name="bitmask">When this method returns <see langword="true"/>,
    /// contains the bitmask of the hash data represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is name-based and the hash data is successfully written to the specified span;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteHashData(
        this Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        if (destination.Length < 16) { return false; }
        if (!guid.GetRfc4122Version().IsNameBased())
        {
            return false;
        }

        var components = GuidComponents.FixedFormat;
        components.WriteHashData(ref guid, destination, bitmask);
        return true;
    }
#endif

    /// <summary>
    /// Tries to get the random data represented by the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="randomData">When this method returns <see langword="true"/>,
    /// contains the random data represented by the <see cref="Guid"/>.</param>
    /// <param name="bitmask">When this method returns <see langword="true"/>,
    /// contains the bitmask of the random data represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is generated randomly; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetRandomData(this Guid guid,
        [NotNullWhen(true)] out byte[]? randomData, [NotNullWhen(true)] out byte[]? bitmask)
    {
        if (!guid.GetRfc4122Version().IsRandomized())
        {
            randomData = null;
            bitmask = null;
            return false;
        }

        var version = guid.GetVersion();
        var components = GuidComponents.OfVersion(version);
        randomData = components.GetRandomData(ref guid, out bitmask);
        return true;
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the random data represented by the <see cref="Guid"/> into a span of bytes.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="destination">When this method returns <see langword="true"/>,
    /// contains the random data represented by the <see cref="Guid"/>.</param>
    /// <param name="bitmask">When this method returns <see langword="true"/>,
    /// contains the bitmask of the random data represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is generated randomly and the random data is successfully written to the specified span;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteRandomData(
        this Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        if (destination.Length < 16) { return false; }
        if (!guid.GetRfc4122Version().IsRandomized())
        {
            return false;
        }

        var version = guid.GetVersion();
        var components = GuidComponents.OfVersion(version);
        components.WriteRandomData(ref guid, destination, bitmask);
        return true;
    }
#endif

#if !UUIDREV_DISABLE
    /// <summary>
    /// Tries to get the custom data represented by the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="customData">When this method returns <see langword="true"/>,
    /// contains the custom data represented by the <see cref="Guid"/>.</param>
    /// <param name="bitmask">When this method returns <see langword="true"/>,
    /// contains the bitmask of the custom data represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// contains custom data; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetCustomData(this Guid guid,
        [NotNullWhen(true)] out byte[]? customData, [NotNullWhen(true)] out byte[]? bitmask)
    {
        if (!guid.GetRfc4122Version().IsCustomized())
        {
            customData = null;
            bitmask = null;
            return false;
        }

        var components = GuidComponents.FixedFormat;
        customData = components.GetCustomData(ref guid, out bitmask);
        return true;
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the custom data represented by the <see cref="Guid"/> into a span of bytes.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="destination">When this method returns <see langword="true"/>,
    /// contains the custom data represented by the <see cref="Guid"/>.</param>
    /// <param name="bitmask">When this method returns <see langword="true"/>,
    /// contains the bitmask of the custom data represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// contains custom data and the custom data is successfully written to the specified span;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteCustomData(
        this Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        if (destination.Length < 16) { return false; }
        if (!guid.GetRfc4122Version().IsCustomized())
        {
            return false;
        }

        var components = GuidComponents.FixedFormat;
        components.WriteCustomData(ref guid, destination, bitmask);
        return true;
    }
#endif
#endif
}
