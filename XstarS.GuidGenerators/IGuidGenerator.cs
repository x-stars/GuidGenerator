﻿using System;

namespace XstarS.GuidGenerators
{
    /// <summary>
    /// 提供生成 <see cref="Guid"/> 所需要的信息。
    /// </summary>
    public interface IGuidGeneratorInfo
    {
        /// <summary>
        /// 获取当前实例生成的 <see cref="Guid"/> 的版本。
        /// </summary>
        /// <returns>当前实例生成的 <see cref="Guid"/> 的版本。</returns>
        GuidVersion Version { get; }

        /// <summary>
        /// 获取当前实例生成的 <see cref="Guid"/> 的变体。
        /// </summary>
        /// <returns>当前实例生成的 <see cref="Guid"/> 的变体。</returns>
        GuidVariant Variant { get; }
    }

    /// <summary>
    /// 提供生成 <see cref="Guid"/> 的方法。
    /// </summary>
    public interface IGuidGenerator : IGuidGeneratorInfo
    {
        /// <summary>
        /// 生成一个新的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <returns>一个新的 <see cref="Guid"/> 实例。</returns>
        Guid NewGuid();
    }

    /// <summary>
    /// 提供生成基于名称的 <see cref="Guid"/> 的方法。
    /// </summary>
    public interface INameBasedGuidGenerator : IGuidGeneratorInfo
    {
        /// <summary>
        /// 根据指定的命名空间和名称生成一个新的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="ns">生成 <see cref="Guid"/> 时使用的命名空间。</param>
        /// <param name="name">生成 <see cref="Guid"/> 时使用的名称。</param>
        /// <returns>根据 <paramref name="ns"/> 和
        /// <paramref name="name"/> 生成的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        Guid NewGuid(Guid ns, string name);
    }

    /// <summary>
    /// 提供生成 DCE Security 用途的 <see cref="Guid"/> 的方法。
    /// </summary>
    public interface IDceSecurityGuidGenerator : IGuidGeneratorInfo
    {
        /// <summary>
        /// 根据指定的 DCE Security 域和本地 ID 生成一个新的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="domain">生成 <see cref="Guid"/> 时使用的 DCE Security 域。</param>
        /// <param name="localID">生成 <see cref="Guid"/> 时使用的自定义本地 ID。</param>
        /// <returns>根据 <paramref name="domain"/> 和
        /// <paramref name="localID"/> 生成的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="domain"/> 不为有效的 <see cref="DceSecurityDomain"/> 枚举值。</exception>
        Guid NewGuid(DceSecurityDomain domain, int? localID = null);
    }
}