﻿using System;
using System.Diagnostics.CodeAnalysis;
using XNetEx.Guids.Components;

namespace XNetEx.Guids;

/// <summary>
/// Provides extension methods for <see cref="Guid"/>.
/// </summary>
public static partial class GuidExtensions
{
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
        var components = GuidComponents.Common;
        return components.GetVersion(ref guid);
    }

    /// <summary>
    /// Gets the variant of the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <returns>The variant of the <see cref="Guid"/>.</returns>
    public static GuidVariant GetVariant(this Guid guid)
    {
        var components = GuidComponents.Common;
        return components.GetVariant(ref guid);
    }

    /// <summary>
    /// Tries to get the timestamp represented by the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="timestamp">When this method returns <see langword="true"/>,
    /// contains the timestamp represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// is time-based; otherwise, <see langword="false"/>.</returns>
    public static unsafe bool TryGetTimestamp(this Guid guid, out DateTime timestamp)
    {
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().IsTimeBased())
        {
            timestamp = default(DateTime);
            return false;
        }

        const ulong utcFlag = 1UL << 62;
        var version = guid.GetVersion();
        var components = GuidComponents.OfVersion(version);
        var tsTicks = components.GetTimestamp(ref guid);
        var tsData = (ulong)tsTicks | utcFlag;
        timestamp = *(DateTime*)&tsData;
        return true;
    }

    /// <summary>
    /// Tries to get the clock sequence represented by the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="clockSeq">When this method returns <see langword="true"/>,
    /// contains the clock sequence represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// contains a clock sequence; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetClockSequence(this Guid guid, out short clockSeq)
    {
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsClockSequence())
        {
            clockSeq = default(int);
            return false;
        }

        var version = guid.GetVersion();
        var components = GuidComponents.OfVersion(version);
        clockSeq = components.GetClockSequence(ref guid);
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
    /// is a DCE Security UUID; otherwise, <see langword="false"/>.</returns>
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

        var components = GuidComponents.FixedFormat;
        domain = components.GetDomain(ref guid);
        localId = components.GetLocalId(ref guid);
        return true;
    }

    /// <summary>
    /// Tries to get the node ID represented by the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="nodeId">When this method returns <see langword="true"/>,
    /// contains the node ID represented by the <see cref="Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// contains node ID data; otherwise, <see langword="false"/>.</returns>
    public static unsafe bool TryGetNodeId(
        this Guid guid, [NotNullWhen(true)] out byte[]? nodeId)
    {
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsNodeId())
        {
            nodeId = null;
            return false;
        }

        var components = GuidComponents.FixedFormat;
        nodeId = components.GetNodeId(ref guid);
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
    /// contains node ID data and the node ID is successfully written to the specified span;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteNodeId(this Guid guid, Span<byte> destination)
    {
        if (destination.Length < 6) { return false; }
        if ((guid.GetVariant() != GuidVariant.Rfc4122) ||
            !guid.GetVersion().ContainsNodeId())
        {
            return false;
        }

        var components = GuidComponents.FixedFormat;
        components.WriteNodeId(ref guid, destination);
        return true;
    }
#endif
}
