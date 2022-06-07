using System;

namespace XstarS.GuidGenerators
{
    partial class GuidGenerator
    {
        /// <summary>
        /// 获取生成 <see cref="Guid.Empty"/> 的 <see cref="IGuidGenerator"/> 对象。
        /// </summary>
        /// <returns>生成 <see cref="Guid.Empty"/> 的 <see cref="IGuidGenerator"/> 对象。</returns>
        public static IGuidGenerator Empty => EmptyGuidGenerator.Instance;

        /// <summary>
        /// 获取 RFC 4122 UUID 版本 1 的 <see cref="IGuidGenerator"/> 对象。
        /// </summary>
        /// <returns>RFC 4122 UUID 版本 1 的 <see cref="IGuidGenerator"/> 对象。</returns>
        public static IGuidGenerator Version1 => TimeBasedGuidGenerator.Instance;

        /// <summary>
        /// 获取 RFC 4122 UUID 版本 2 的 <see cref="IDceSecurityGuidGenerator"/> 对象。
        /// </summary>
        /// <returns>RFC 4122 UUID 版本 2 的 <see cref="IDceSecurityGuidGenerator"/> 对象。</returns>
        public static IDceSecurityGuidGenerator Version2 => DceSecurityGuidGenerator.Instance;

        /// <summary>
        /// 获取 RFC 4122 UUID 版本 3 的 <see cref="INameBasedGuidGenerator"/> 对象。
        /// </summary>
        /// <returns>RFC 4122 UUID 版本 3 的 <see cref="INameBasedGuidGenerator"/> 对象。</returns>
        public static INameBasedGuidGenerator Version3 => NameBasedGuidGenerator.MD5Hashing.Instance;

        /// <summary>
        /// 获取 RFC 4122 UUID 版本 4 的 <see cref="IGuidGenerator"/> 对象。
        /// </summary>
        /// <returns>RFC 4122 UUID 版本 4 的 <see cref="IGuidGenerator"/> 对象。</returns>
        public static IGuidGenerator Version4 => RandomizedGuidGenerator.Instance;

        /// <summary>
        /// 获取 RFC 4122 UUID 版本 5 的 <see cref="INameBasedGuidGenerator"/> 对象。
        /// </summary>
        /// <returns>RFC 4122 UUID 版本 5 的 <see cref="INameBasedGuidGenerator"/> 对象。</returns>
        public static INameBasedGuidGenerator Version5 => NameBasedGuidGenerator.SHA1Hashing.Instance;

        /// <summary>
        /// 生成一个指定版本的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="version">要生成的 <see cref="Guid"/> 的版本。</param>
        /// <returns>一个版本为 <paramref name="version"/> 的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="version"/> 不为有效的 <see cref="GuidVersion"/> 枚举值。</exception>
        public static Guid NewGuid(GuidVersion version)
        {
            return GuidGenerator.OfVersion(version).NewGuid();
        }

        /// <summary>
        /// 根据指定的命名空间 ID 和名称生成一个指定版本的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="version">要生成的 <see cref="Guid"/> 的版本。</param>
        /// <param name="nsId">生成 <see cref="Guid"/> 时使用的命名空间 ID。</param>
        /// <param name="name">生成 <see cref="Guid"/> 时使用的名称的字节数组。</param>
        /// <returns>根据 <paramref name="nsId"/> 和 <paramref name="name"/>
        /// 生成的版本为 <paramref name="version"/> 的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="version"/> 不为有效的 <see cref="GuidVersion"/> 枚举值。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="version"/> 版本不支持基于名称的 <see cref="Guid"/> 生成模式。</exception>
        public static Guid NewGuid(GuidVersion version, Guid nsId, byte[] name)
        {
            return GuidGenerator.OfVersion(version).NewGuid(nsId, name);
        }

        /// <summary>
        /// 根据指定的命名空间 ID 和名称生成一个指定版本的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="version">要生成的 <see cref="Guid"/> 的版本。</param>
        /// <param name="nsId">生成 <see cref="Guid"/> 时使用的命名空间 ID。</param>
        /// <param name="name">生成 <see cref="Guid"/> 时使用的名称的字符串。</param>
        /// <returns>根据 <paramref name="nsId"/> 和 <paramref name="name"/>
        /// 生成的版本为 <paramref name="version"/> 的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="version"/> 不为有效的 <see cref="GuidVersion"/> 枚举值。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="version"/> 版本不支持基于名称的 <see cref="Guid"/> 生成模式。</exception>
        public static Guid NewGuid(GuidVersion version, Guid nsId, string name)
        {
            return GuidGenerator.OfVersion(version).NewGuid(nsId, name);
        }

        /// <summary>
        /// 获取指定 <see cref="GuidVersion"/> 版本的 <see cref="GuidGenerator"/> 对象。
        /// </summary>
        /// <param name="version">要生成的 <see cref="Guid"/> 的版本。</param>
        /// <returns>版本为 <paramref name="version"/> 的 <see cref="GuidGenerator"/>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="version"/> 不为有效的 <see cref="GuidVersion"/> 枚举值。</exception>
        public static GuidGenerator OfVersion(GuidVersion version) => version switch
        {
            GuidVersion.Empty => EmptyGuidGenerator.Instance,
            GuidVersion.Version1 => TimeBasedGuidGenerator.Instance,
            GuidVersion.Version2 => DceSecurityGuidGenerator.Instance,
            GuidVersion.Version3 => NameBasedGuidGenerator.MD5Hashing.Instance,
            GuidVersion.Version4 => RandomizedGuidGenerator.Instance,
            GuidVersion.Version5 => NameBasedGuidGenerator.SHA1Hashing.Instance,
            _ => throw new ArgumentOutOfRangeException(nameof(version))
        };
    }
}
