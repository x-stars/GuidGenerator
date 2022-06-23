using System;

namespace XNetEx.Guids;

/// <summary>
/// 表示 <see cref="Guid"/> 的变体。
/// </summary>
public enum GuidVariant
{
    /// <summary>
    /// 保留，用于向后兼容 NCS UUID。
    /// </summary>
    Ncs = 0,
    /// <summary>
    /// 表示 RFC 4122 定义的 UUID 变体。
    /// </summary>
    Rfc4122 = 1,
    /// <summary>
    /// 保留，用于向后兼容 Microsoft 早期 GUID。
    /// </summary>
    Microsoft = 2,
    /// <summary>
    /// 保留供将来使用。
    /// </summary>
    Reserved = 3,
}
