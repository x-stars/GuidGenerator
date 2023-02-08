using System;

namespace XNetEx.Guids;

static partial class GuidExtensions
{
    /// <summary>
    /// Replaces the version of the current <see cref="Guid"/>
    /// with the specified <see cref="GuidVersion"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="version">The version to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the version replaced with <paramref name="version"/>.</returns>
    public static Guid ReplaceVersion(this Guid guid, GuidVersion version)
    {
        var result = guid;
        var shiftVer = (int)version << (3 * 4);
        ref var timeHi_Ver = ref result.TimeHi_Ver();
        timeHi_Ver = (ushort)(timeHi_Ver & ~0xF000 | shiftVer);
        return result;
    }

    /// <summary>
    /// Replaces the version of the current <see cref="Guid"/>
    /// with the specified <see cref="GuidVariant"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="variant">The variant to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the variant replaced with <paramref name="variant"/>.</returns>
    public static Guid ReplaceVariant(this Guid guid, GuidVariant variant)
    {
        var result = guid;
        var shiftVar = -1 << (8 - (int)variant);
        var varMask = (shiftVar >> 1) & 0xE0;
        ref var clkSeqHi_Var = ref result.ClkSeqHi_Var();
        clkSeqHi_Var = (byte)(clkSeqHi_Var & ~varMask | shiftVar);
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
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().IsTimeBased())
        {
            return guid;
        }

        var result = guid;
        var version = guid.GetVersion();
        var utcTimestamp = timestamp.ToUniversalTime();
        if (version != GuidVersion.Version7)
        {
            var ecpochTicks = TimestampEpochs.Gregorian.Ticks;
            var tsField = utcTimestamp.Ticks - ecpochTicks;
            if (version != GuidVersion.Version6)
            {
                result.TimeLow() = (uint)(tsField >> (0 * 8));
                result.TimeMid() = (ushort)(tsField >> (4 * 8));
                result.TimeHi_Ver() = (ushort)(tsField >> (6 * 8));
            }
            else
            {
                result.TimeLow() = (uint)(tsField >> (4 * 8 - 4));
                result.TimeMid() = (ushort)(tsField >> (2 * 8 - 4));
                result.TimeHi_Ver() = (ushort)(tsField >> (0 * 8));
            }
            if (version.ContainsLocalId())
            {
                result.TimeLow() = guid.TimeLow();
            }
        }
        else
        {
            const long ticksPerMs = 1_000_000 / 100;
            var ecpochTicks = TimestampEpochs.UnixTime.Ticks;
            var tsField = (utcTimestamp.Ticks - ecpochTicks) / ticksPerMs;
            result.TimeLow() = (uint)(tsField >> (2 * 8));
            result.TimeMid() = (ushort)(tsField >> (0 * 8));
        }
        var shiftVer = (int)version << (3 * 4);
        ref var timeHi_Ver = ref result.TimeHi_Ver();
        timeHi_Ver = (ushort)(timeHi_Ver & ~0xF000 | shiftVer);
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
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsClockSequence())
        {
            return guid;
        }

        var result = guid;
        var version = guid.GetVersion();
        if (version.ContainsLocalId())
        {
            clockSeq <<= (1 * 8);
        }
        result.ClkSeqLow() = (byte)(clockSeq >> (0 * 8));
        result.ClkSeqHi_Var() = (byte)(clockSeq >> (1 * 8));
        if (version.ContainsLocalId())
        {
            result.ClkSeqLow() = guid.ClkSeqLow();
        }
        ref var clkSeqHi_Var = ref result.ClkSeqHi_Var();
        clkSeqHi_Var = (byte)(clkSeqHi_Var & ~0xC0 | 0x80);
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
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsLocalId())
        {
            return guid;
        }

        var result = guid;
        result.ClkSeqLow() = (byte)domain;
        result.TimeLow() = (uint)localId;
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

        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsNodeId())
        {
            return guid;
        }

        var result = guid;
        result.SetNodeId(nodeId);
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

        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsNodeId())
        {
            return guid;
        }

        var result = guid;
        result.SetNodeId(nodeId);
        return result;
    }
#endif
}
