using System;

namespace XNetEx.Guids;

/// <summary>
/// Provides <see cref="Guid"/> namespace IDs specified in RFC 4122.
/// </summary>
public static class GuidNamespaces
{
    /// <summary>
    /// Represents the namespace ID for a fully-qualified domain name.
    /// </summary>
    public static readonly Guid Dns = new(
        // 6ba7b810-9dad-11d1-80b4-00c04fd430c8
        0x6ba7b810, 0x9dad, 0x11d1,
        0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

    /// <summary>
    /// Represents the namespace ID for a URL.
    /// </summary>
    public static readonly Guid Url = new(
        // 6ba7b811-9dad-11d1-80b4-00c04fd430c8
        0x6ba7b811, 0x9dad, 0x11d1,
        0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

    /// <summary>
    /// Represents the namespace ID for an ISO OID.
    /// </summary>
    public static readonly Guid Oid = new(
        // 6ba7b812-9dad-11d1-80b4-00c04fd430c8
        0x6ba7b812, 0x9dad, 0x11d1,
        0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

    /// <summary>
    /// Represents the namespace ID for an X.500 DN.
    /// </summary>
    public static readonly Guid X500 = new(
        // 6ba7b814-9dad-11d1-80b4-00c04fd430c8
        0x6ba7b814, 0x9dad, 0x11d1,
        0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);
}
