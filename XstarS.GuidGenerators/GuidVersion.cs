using System;

namespace XstarS.GuidGenerators;

/// <summary>
/// 表示 <see cref="Guid"/> 的版本。
/// </summary>
public enum GuidVersion
{
    /// <summary>
    /// 表示 Nil UUID (<see cref="Guid.Empty"/>) 的版本。
    /// </summary>
    Empty = 0,
    /// <summary>
    /// 表示 RFC 4122 UUID 版本 1，此版本的 <see cref="Guid"/> 基于当前时间生成。
    /// </summary>
    Version1 = 1,
    /// <summary>
    /// 表示 RFC 4122 UUID 版本 2，此版本的 <see cref="Guid"/> 用于 DCE Security 用途。
    /// </summary>
    Version2 = 2,
    /// <summary>
    /// 表示 RFC 4122 UUID 版本 3，此版本的 <see cref="Guid"/> 基于输入名称的 MD5 生成。
    /// </summary>
    Version3 = 3,
    /// <summary>
    /// 表示 RFC 4122 UUID 版本 4，此版本的 <see cref="Guid"/> 基于随机数生成。
    /// </summary>
    Version4 = 4,
    /// <summary>
    /// 表示 RFC 4122 UUID 版本 5，此版本的 <see cref="Guid"/> 基于输入名称的 SHA1 生成。
    /// </summary>
    Version5 = 5,
}
