﻿using System;

namespace XstarS.GuidGenerators
{
    /// <summary>
    /// 表示 <see cref="Guid"/> 的版本。
    /// </summary>
    public enum GuidVersion
    {
        /// <summary>
        /// 表示未知 GUID 版本。
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 表示 RFC 4122 UUID 版本 1。
        /// </summary>
        Version1 = 1,
        /// <summary>
        /// 表示 RFC 4122 UUID 版本 2。
        /// </summary>
        Version2 = 2,
        /// <summary>
        /// 表示 RFC 4122 UUID 版本 3。
        /// </summary>
        Version3 = 3,
        /// <summary>
        /// 表示 RFC 4122 UUID 版本 4。
        /// </summary>
        Version4 = 4,
        /// <summary>
        /// 表示 RFC 4122 UUID 版本 5。
        /// </summary>
        Version5 = 5,
    }

    /// <summary>
    /// 提供 <see cref="GuidVersion"/> 相关的扩展方法。
    /// </summary>
    public static class GuidVersionExtensions
    {
        /// <summary>
        /// 获取当前 <see cref="GuidVersion"/> 是否基于时间生成 <see cref="Guid"/>。
        /// </summary>
        /// <param name="version">要获取信息的 <see cref="GuidVersion"/>。</param>
        /// <returns>若 <paramref name="version"/> 基于时间生成 <see cref="Guid"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool IsTimeBased(this GuidVersion version) =>
            version == GuidVersion.Version1 ||
            version == GuidVersion.Version2;

        /// <summary>
        /// 获取当前 <see cref="GuidVersion"/> 是否基于名称生成 <see cref="Guid"/>。
        /// </summary>
        /// <param name="version">要获取信息的 <see cref="GuidVersion"/>。</param>
        /// <returns>若 <paramref name="version"/> 基于名称生成 <see cref="Guid"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool IsNameBased(this GuidVersion version) =>
            version == GuidVersion.Version3 ||
            version == GuidVersion.Version5;

        /// <summary>
        /// 获取当前 <see cref="GuidVersion"/> 是否基于随机数生成 <see cref="Guid"/>。
        /// </summary>
        /// <param name="version">要获取信息的 <see cref="GuidVersion"/>。</param>
        /// <returns>若 <paramref name="version"/> 基于随机数生成 <see cref="Guid"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool IsRandomized(this GuidVersion version) =>
            version == GuidVersion.Version4;

        /// <summary>
        /// 获取当前 <see cref="GuidVersion"/> 生成的 <see cref="Guid"/> 是否包含私有 ID 信息。
        /// </summary>
        /// <param name="version">要获取信息的 <see cref="GuidVersion"/>。</param>
        /// <returns>若 <paramref name="version"/> 生成的 <see cref="Guid"/> 包含私有 ID 信息，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool ContainsPrivateID(this GuidVersion version) =>
            version == GuidVersion.Version1 ||
            version == GuidVersion.Version2;

        /// <summary>
        /// 获取当前 <see cref="Guid"/> 的版本。
        /// </summary>
        /// <param name="guid">要获取版本的 <see cref="Guid"/>。</param>
        /// <returns>当前 <see cref="Guid"/> 的版本。</returns>
        public static GuidVersion GetVersion(this Guid guid) =>
            (GuidVersion)((guid.ToByteArray()[7] & 0xF0) >> 4);
    }
}
