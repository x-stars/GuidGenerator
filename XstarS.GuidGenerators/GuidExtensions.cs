using System;

namespace XNetEx.Guids;

/// <summary>
/// Provides extension methods for <see cref="Guid"/>.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// Represents the epoch timestamp of a time-based <see cref="Guid"/>.
    /// </summary>
    internal static readonly DateTime BaseTimestamp =
        new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Gets the version of the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <returns>The version of the <see cref="Guid"/>.</returns>
    public static GuidVersion GetVersion(this Guid guid)
    {
        var shiftVer = (int)guid.TimeHi_Ver() & 0xF000;
        var version = shiftVer >> (3 * 4);
        return (GuidVersion)version;
    }

    /// <summary>
    /// Gets the variant of the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <returns>The variant of the <see cref="Guid"/>.</returns>
    public static GuidVariant GetVariant(this Guid guid)
    {
        var variant = -1;
        var shiftVar = (int)guid.ClkSeqHi_Var() & 0xE0;
        while ((sbyte)(shiftVar << ++variant) < 0) { }
        return (GuidVariant)variant;
    }

    /// <summary>
    /// Tries to get the timestamp represented by the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="timestamp">When this method returns, contains the timestamp
    /// represented by the <see cref="Guid"/> if the <see cref="Guid"/> is time-based;
    /// otherwise, the default value of <see cref="DateTime"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is time-based; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetTimestamp(this Guid guid, out DateTime timestamp)
    {
        var version = guid.GetVersion();
        if (!version.IsTimeBased())
        {
            timestamp = default(DateTime);
            return false;
        }
        var tsField = ((long)guid.TimeLow()) |
            ((long)guid.TimeMid() << (4 * 8)) |
            ((long)guid.TimeHi_Ver() << (6 * 8));
        tsField &= ~(0xF0L << (7 * 8));
        if (version.ContainsLocalId())
        {
            tsField &= ~0xFFFFFFFFL;
        }
        var tsTicks = GuidExtensions.BaseTimestamp.Ticks + tsField;
        timestamp = new DateTime(tsTicks, DateTimeKind.Utc);
        return true;
    }

    /// <summary>
    /// Tries to get the clock sequence represented by the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="clockSeq">When this method returns, contains the clock sequence
    /// represented by the <see cref="Guid"/> if the <see cref="Guid"/> is time-based;
    /// otherwise, the default value of <see cref="short"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is time-based; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetClockSequence(this Guid guid, out short clockSeq)
    {
        var version = guid.GetVersion();
        if (!version.IsTimeBased())
        {
            clockSeq = default(int);
            return false;
        }
        var csField = ((int)guid.ClkSeqLow()) |
            ((int)guid.ClkSeqHi_Var() << (1 * 8));
        csField &= ~(0xC0 << (1 * 8));
        if (version.ContainsLocalId())
        {
            csField >>= (1 * 8);
        }
        clockSeq = (short)csField;
        return true;
    }

    /// <summary>
    /// Tries to get the DCE Security domain and local ID represented by the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="domain">When this method returns, contains the DCE Security domain
    /// represented by the <see cref="Guid"/> if the <see cref="Guid"/> is a DCE security UUID;
    /// otherwise, the default value of <see cref="DceSecurityDomain"/>.</param>
    /// <param name="localId">When this method returns, contains the local ID
    /// represented by the <see cref="Guid"/> if the <see cref="Guid"/> is a DCE security UUID;
    /// otherwise, the default value of <see cref="int"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is a DCE security UUID; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetDomainAndLocalId(
        this Guid guid, out DceSecurityDomain domain, out int localId)
    {
        var hasLocalId = guid.GetVersion().ContainsLocalId();
        domain = hasLocalId ? (DceSecurityDomain)guid.ClkSeqLow() : 0;
        localId = hasLocalId ? (int)guid.TimeLow() : 0;
        return hasLocalId;
    }

    /// <summary>
    /// Tries to get the node ID represented by the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="nodeId">When this method returns, contains the node ID
    /// represented by the <see cref="Guid"/> if the <see cref="Guid"/> is time-based;
    /// otherwise, a 6-element byte array filled with zero values.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is time-based; otherwise, <see langword="false"/>.</returns>
    public static unsafe bool TryGetNodeId(this Guid guid, out byte[] nodeId)
    {
        const int size = 6;
        nodeId = new byte[size];
        if (!guid.GetVersion().ContainsNodeId())
        {
            return false;
        }
        fixed (byte* pNodeId = &nodeId[0])
        {
            Buffer.MemoryCopy(guid.NodeId(), pNodeId, size, size);
        }
        return true;
    }

    /// <summary>
    /// Returns a 16-element byte array that contains fields of
    /// the <see cref="Guid"/> in big-endian order (RFC 4122 compliant).
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <returns>A 16-element byte array that contains fields of
    /// the <see cref="Guid"/> in big-endian order (RFC 4122 compliant).</returns>
    public static unsafe byte[] ToUuidByteArray(this Guid guid)
    {
        var uuidBytes = new byte[16];
        fixed (byte* pUuidBytes = &uuidBytes[0])
        {
            guid.WriteUuidBytes(pUuidBytes);
        }
        return uuidBytes;
    }

    /// <summary>
    /// Tries to write fields of the <see cref="Guid"/>
    /// in big-endian order (RFC 4122 compliant) into the specified address.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="destination">The destination address to write data into.</param>
    internal static unsafe void WriteUuidBytes(this Guid guid, byte* destination)
    {
        *(Guid*)destination = guid;
        if (BitConverter.IsLittleEndian)
        {
            var pGuidBytes = (byte*)&guid;
            destination[0] = pGuidBytes[3];
            destination[1] = pGuidBytes[2];
            destination[2] = pGuidBytes[1];
            destination[3] = pGuidBytes[0];
            destination[4] = pGuidBytes[5];
            destination[5] = pGuidBytes[4];
            destination[6] = pGuidBytes[7];
            destination[7] = pGuidBytes[6];
        }
    }
}
