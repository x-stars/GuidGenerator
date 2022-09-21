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
    internal static readonly DateTime TimeBasedEpoch =
        new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Deconstructs the <see cref="Guid"/> into fields of integers and bytes.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="a">The first 4 bytes of the GUID.</param>
    /// <param name="b">The next 2 bytes of the GUID.</param>
    /// <param name="c">The next 2 bytes of the GUID.</param>
    /// <param name="d">The next byte of the GUID.</param>
    /// <param name="e">The next byte of the GUID.</param>
    /// <param name="f">The next byte of the GUID.</param>
    /// <param name="g">The next byte of the GUID.</param>
    /// <param name="h">The next byte of the GUID.</param>
    /// <param name="i">The next byte of the GUID.</param>
    /// <param name="j">The next byte of the GUID.</param>
    /// <param name="k">The next byte of the GUID.</param>
    public static unsafe void Deconstruct(this Guid guid,
        out int a, out short b, out short c, out byte d, out byte e,
        out byte f, out byte g, out byte h, out byte i, out byte j, out byte k)
    {
        a = (int)guid.TimeLow();
        b = (short)guid.TimeMid();
        c = (short)guid.TimeHi_Ver();
        d = guid.ClkSeqHi_Var(); e = guid.ClkSeqLow();
        var nodeId = guid.NodeId();
        f = nodeId[0]; g = nodeId[1]; h = nodeId[2];
        i = nodeId[3]; j = nodeId[4]; k = nodeId[5];
    }

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
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().IsTimeBased())
        {
            timestamp = default(DateTime);
            return false;
        }
        var tsField = ((long)guid.TimeLow()) |
            ((long)guid.TimeMid() << (4 * 8)) |
            ((long)guid.TimeHi_Ver() << (6 * 8));
        tsField &= ~(0xF0L << (7 * 8));
        if (guid.GetVersion().ContainsLocalId())
        {
            tsField &= ~0xFFFFFFFFL;
        }
        var tsTicks = GuidExtensions.TimeBasedEpoch.Ticks + tsField;
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
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().IsTimeBased())
        {
            clockSeq = default(int);
            return false;
        }
        var csField = ((int)guid.ClkSeqLow()) |
            ((int)guid.ClkSeqHi_Var() << (1 * 8));
        csField &= ~(0xC0 << (1 * 8));
        if (guid.GetVersion().ContainsLocalId())
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
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsLocalId())
        {
            domain = default(DceSecurityDomain);
            localId = default(int);
            return false;
        }
        domain = (DceSecurityDomain)guid.ClkSeqLow();
        localId = (int)guid.TimeLow();
        return true;
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
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsNodeId())
        {
            return false;
        }
        fixed (byte* pNodeId = &nodeId[0])
        {
            Buffer.MemoryCopy(guid.NodeId(), pNodeId, size, size);
        }
        return true;
    }

#if MEMORY_SPAN || NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the node ID represented by the <see cref="Guid"/> into a span of bytes.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="destination">When this method returns, contains the node ID
    /// represented by the <see cref="Guid"/> if the <see cref="Guid"/> is time-based.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is time-based and the node ID is successfully written to the specified span;
    /// otherwise, <see langword="false"/>.</returns>
    public static unsafe bool TryWriteNodeId(this Guid guid, Span<byte> destination)
    {
        const int size = 6;
        if (destination.Length < size) { return false; }
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsNodeId())
        {
            return false;
        }
        fixed (byte* pNodeId = &destination[0])
        {
            Buffer.MemoryCopy(guid.NodeId(), pNodeId, size, size);
        }
        return true;
    }
#endif

    /// <summary>
    /// Creates a new <see cref="Guid"/> instance by using
    /// the specified byte array in big-endian order (RFC 4122 compliant).
    /// </summary>
    /// <param name="bytes">A 16-element byte array containing
    /// the fields of the GUID in big-endian order (RFC 4122 compliant).</param>
    /// <returns>A new <see cref="Guid"/> instance of the specified byte array.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="bytes"/> is not 16 bytes long.</exception>
    public static unsafe Guid FromUuidByteArray(byte[] bytes)
    {
        var guid = new Guid(bytes);
        fixed (byte* pBytes = &bytes[0])
        {
            var uuid = *(Guid*)pBytes;
            uuid.WriteUuidBytes((byte*)&guid);
        }
        return guid;
    }

#if MEMORY_SPAN || NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Creates a new <see cref="Guid"/> instance by using the specified
    /// read-only byte span in big-endian order (RFC 4122 compliant).
    /// </summary>
    /// <param name="bytes">A 16-element read-only span containing the bytes of
    /// the fields of the GUID in big-endian order (RFC 4122 compliant).</param>
    /// <returns>A new <see cref="Guid"/> instance of the specified byte span.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="bytes"/> is not 16 bytes long.</exception>
    public static unsafe Guid FromUuidBytes(ReadOnlySpan<byte> bytes)
    {
        var guid = new Guid(bytes);
        fixed (byte* pBytes = &bytes[0])
        {
            var uuid = *(Guid*)pBytes;
            uuid.WriteUuidBytes((byte*)&guid);
        }
        return guid;
    }
#endif

    /// <summary>
    /// Returns a 16-element byte array that contains the fields of
    /// the <see cref="Guid"/> in big-endian order (RFC 4122 compliant).
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <returns>A 16-element byte array that contains the fields of
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

#if MEMORY_SPAN || NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the fields of the <see cref="Guid"/>
    /// into a span of bytes in big-endian order (RFC 4122 compliant).
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="destination">When this method returns, contains the fields of
    /// the <see cref="Guid"/> in big-endian order (RFC 4122 compliant).</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/> is successfully
    /// written to the specified span; otherwise, <see langword="false"/>.</returns>
    public static unsafe bool TryWriteUuidBytes(this Guid guid, Span<byte> destination)
    {
        if (destination.Length < 16) { return false; }
        fixed (byte* pUuidBytes = &destination[0])
        {
            guid.WriteUuidBytes(pUuidBytes);
        }
        return true;
    }
#endif

    /// <summary>
    /// Writes the fields of the <see cref="Guid"/>
    /// into the specified address in big-endian order (RFC 4122 compliant).
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
