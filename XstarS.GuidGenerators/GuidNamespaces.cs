using System;

namespace XNetEx.Guids;

/// <summary>
/// 提供由 RFC 4122 定义的 <see cref="Guid"/> 命名空间 ID。
/// </summary>
public static class GuidNamespaces
{
    /// <summary>
    /// 表示完整域名的命名空间 ID。
    /// </summary>
    public static readonly Guid Dns = new Guid(
        // 6ba7b810-9dad-11d1-80b4-00c04fd430c8
        0x6ba7b810, 0x9dad, 0x11d1,
        0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

    /// <summary>
    /// 表示 URL 的命名空间 ID。
    /// </summary>
    public static readonly Guid Url = new Guid(
        // 6ba7b811-9dad-11d1-80b4-00c04fd430c8
        0x6ba7b811, 0x9dad, 0x11d1,
        0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

    /// <summary>
    /// 表示 ISO OID 的命名空间 ID。
    /// </summary>
    public static readonly Guid Oid = new Guid(
        // 6ba7b812-9dad-11d1-80b4-00c04fd430c8
        0x6ba7b812, 0x9dad, 0x11d1,
        0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

    /// <summary>
    /// 表示 X.500 DN 的命名空间 ID。
    /// </summary>
    public static readonly Guid X500 = new Guid(
        // 6ba7b814-9dad-11d1-80b4-00c04fd430c8
        0x6ba7b814, 0x9dad, 0x11d1,
        0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);
}
