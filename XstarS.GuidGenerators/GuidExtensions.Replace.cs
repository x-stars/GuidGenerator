using System;
using XNetEx.Guids.Components;

namespace XNetEx.Guids;

static partial class GuidExtensions
{
    /// <summary>
    /// Replaces the version of the current <see cref="Guid"/> with the specified GUID version number.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="version">The version to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the version replaced with <paramref name="version"/>.</returns>
    public static Guid ReplaceVersion(this Guid guid, byte version)
    {
        return guid.ReplaceVersion((GuidVersion)version);
    }

    /// <summary>
    /// Replaces the version of the current <see cref="Guid"/> with the specified <see cref="GuidVersion"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="version">The version to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the version replaced with <paramref name="version"/>.</returns>
    public static Guid ReplaceVersion(this Guid guid, GuidVersion version)
    {
        var result = guid;
        var components = GuidComponents.Common;
        components.SetVersion(ref result, version);
        return result;
    }

    /// <summary>
    /// Replaces the version of the current <see cref="Guid"/> with the specified <see cref="GuidVariant"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="variant">The variant to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the variant replaced with <paramref name="variant"/>.</returns>
    public static Guid ReplaceVariant(this Guid guid, GuidVariant variant)
    {
        var result = guid;
        var components = GuidComponents.Common;
        components.SetVariant(ref result, variant);
        return result;
    }

    /// <summary>
    /// Replaces the timestamp of the current <see cref="Guid"/>
    /// with the specified <see cref="DateTime"/> if the <see cref="Guid"/> is time-based.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="timestamp">The timestamp to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the timestamp replaced with <paramref name="timestamp"/>,
    /// or the original value if the <see cref="Guid"/> is not time-based.</returns>
    public static Guid ReplaceTimestamp(this Guid guid, DateTime timestamp)
    {
        if (!guid.GetRfc4122Version().IsTimeBased())
        {
            return guid;
        }

        var result = guid;
        var version = guid.GetVersion();
        var components = GuidComponents.OfVersion(version);
        var tsTicks = timestamp.ToUniversalTime().Ticks;
        components.SetTimestamp(ref result, tsTicks);
        return result;
    }

    /// <summary>
    /// Replaces the timestamp of the current <see cref="Guid"/>
    /// with the specified <see cref="DateTimeOffset"/> if the <see cref="Guid"/> is time-based.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="timestamp">The timestamp to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the timestamp replaced with <paramref name="timestamp"/>,
    /// or the original value if the <see cref="Guid"/> is not time-based.</returns>
    public static Guid ReplaceTimestamp(this Guid guid, DateTimeOffset timestamp)
    {
        return guid.ReplaceTimestamp(timestamp.UtcDateTime);
    }

    /// <summary>
    /// Replaces the clock sequence of the current <see cref="Guid"/>
    /// with the specified 16-bit signed integer if the <see cref="Guid"/> contains a clock sequence.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="clockSeq">The clock sequence to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the clock sequence replaced with <paramref name="clockSeq"/>,
    /// or the original value if the <see cref="Guid"/> does not contain a clock sequence.</returns>
    public static Guid ReplaceClockSequence(this Guid guid, short clockSeq)
    {
        if (!guid.GetRfc4122Version().ContainsClockSequence())
        {
            return guid;
        }

        var result = guid;
        var version = guid.GetVersion();
        var components = GuidComponents.OfVersion(version);
        components.SetClockSequence(ref result, clockSeq);
        return result;
    }

    /// <summary>
    /// Replaces the DCE Security domain and local ID of the current <see cref="Guid"/>
    /// with the specified <see cref="DceSecurityDomain"/> and 32-bit signed integer
    /// if the <see cref="Guid"/> is a DCE Security UUID.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="domain">The DCE Security domain to use as replacement.</param>
    /// <param name="localId">The local ID to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the DCE Security domain replaced with <paramref name="domain"/>
    /// and the local ID replaced with and <paramref name="localId"/>,
    /// or the original value if the <see cref="Guid"/> is not a DCE Security UUID.</returns>
    public static Guid ReplaceDomainAndLocalId(
        this Guid guid, DceSecurityDomain domain, int localId)
    {
        if (!guid.GetRfc4122Version().ContainsLocalId())
        {
            return guid;
        }

        var result = guid;
        var components = GuidComponents.FixedFormat;
        components.SetDomain(ref result, domain);
        components.SetLocalId(ref result, localId);
        return result;
    }

    /// <summary>
    /// Replaces the node ID of the current <see cref="Guid"/>
    /// with the specified byte array if the <see cref="Guid"/> contains node ID data.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="nodeId">The node ID to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the node ID replaced with <paramref name="nodeId"/>,
    /// or the original value if the <see cref="Guid"/> does not contain node ID data.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="nodeId"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="nodeId"/> is not 6 bytes long.</exception>
    public static Guid ReplaceNodeId(this Guid guid, byte[] nodeId)
    {
        if (nodeId is null)
        {
            throw new ArgumentNullException(nameof(nodeId));
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return guid.ReplaceNodeId((Span<byte>)nodeId);
#else
        if (nodeId.Length != 6)
        {
            throw new ArgumentException(
                "Node ID for Guid must be exactly 6 bytes long.",
                nameof(nodeId));
        }
        if (!guid.GetRfc4122Version().ContainsNodeId())
        {
            return guid;
        }

        var result = guid;
        var components = GuidComponents.FixedFormat;
        components.SetNodeId(ref result, nodeId);
        return result;
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Replaces the node ID of the current <see cref="Guid"/>
    /// with the specified byte span if the <see cref="Guid"/> contains node ID data.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="nodeId">The node ID to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the node ID replaced with <paramref name="nodeId"/>,
    /// or the original value if the <see cref="Guid"/> does not contain node ID data.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="nodeId"/> is not 6 bytes long.</exception>
    public static Guid ReplaceNodeId(this Guid guid, ReadOnlySpan<byte> nodeId)
    {
        if (nodeId.Length != 6)
        {
            throw new ArgumentException(
                "Node ID for Guid must be exactly 6 bytes long.",
                nameof(nodeId));
        }
        if (!guid.GetRfc4122Version().ContainsNodeId())
        {
            return guid;
        }

        var result = guid;
        var components = GuidComponents.FixedFormat;
        components.SetNodeId(ref result, nodeId);
        return result;
    }
#endif
}
