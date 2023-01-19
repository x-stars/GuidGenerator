using System;
using System.Diagnostics.CodeAnalysis;

namespace XNetEx.Guids;

/// <summary>
/// Provides extension methods for <see cref="Guid"/>.
/// </summary>
public static partial class GuidExtensions
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
    public static void Deconstruct(this Guid guid,
        out int a, out short b, out short c, out byte d, out byte e,
        out byte f, out byte g, out byte h, out byte i, out byte j, out byte k)
    {
        a = (int)guid.TimeLow();
        b = (short)guid.TimeMid();
        c = (short)guid.TimeHi_Ver();
        d = guid.ClkSeqHi_Var(); e = guid.ClkSeqLow();
        f = guid.NodeId(0); g = guid.NodeId(1);
        h = guid.NodeId(2); i = guid.NodeId(3);
        j = guid.NodeId(4); k = guid.NodeId(5);
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
    /// <param name="timestamp">When this method returns <see langword="true"/>,
    /// contains the timestamp represented by the <see cref="Guid"/>.</param>
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
    /// <param name="clockSeq">When this method returns <see langword="true"/>,
    /// contains the clock sequence represented by the <see cref="Guid"/>.</param>
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
    /// <param name="domain">When this method returns <see langword="true"/>,
    /// contains the DCE Security domain represented by the <see cref="Guid"/>.</param>
    /// <param name="localId">When this method returns <see langword="true"/>,
    /// contains the local ID represented by the <see cref="Guid"/>.</param>
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
    /// <param name="nodeId">When this method returns <see langword="true"/>,
    /// contains the node ID represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is time-based; otherwise, <see langword="false"/>.</returns>
    public static unsafe bool TryGetNodeId(
        this Guid guid, [NotNullWhen(true)] out byte[]? nodeId)
    {
        const int size = 6;
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsNodeId())
        {
            nodeId = null;
            return false;
        }
        nodeId = new byte[size];
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        guid.NodeId().CopyTo((Span<byte>)nodeId);
#else
        fixed (byte* pGuidNodeId = &guid.NodeId(0), pNodeId = &nodeId[0])
        {
            Buffer.MemoryCopy(pGuidNodeId, pNodeId, size, size);
        }
#endif
        return true;
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the node ID represented by the <see cref="Guid"/> into a span of bytes.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="destination">When this method returns <see langword="true"/>,
    /// contains the node ID represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is time-based and the node ID is successfully written to the specified span;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteNodeId(this Guid guid, Span<byte> destination)
    {
        const int size = 6;
        if (destination.Length < size) { return false; }
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsNodeId())
        {
            return false;
        }
        guid.NodeId().CopyTo(destination);
        return true;
    }
#endif
}
