using System;
using System.ComponentModel;

namespace XstarS.GuidGenerators
{
    using static GuidVersion;

    partial class GuidGenerator
    {
        /// <summary>
        /// 获取指定 GUID 版本的 <see cref="GuidGenerator"/> 对象。
        /// </summary>
        /// <param name="version">要生成的 <see cref="Guid"/> 的版本。</param>
        /// <returns>版本为 <paramref name="version"/> 的 <see cref="GuidGenerator"/>。</returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="version"/> 为 <see cref="GuidVersion.Unknown"/>。</exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="version"/> 不为有效的 <see cref="GuidVersion"/> 枚举值。</exception>
        public static GuidGenerator OfVersion(GuidVersion version)
        {
            return version switch
            {
                Unknown => throw new InvalidOperationException(),
                Version1 => TimeBasedGuidGenerator.Instance,
                Version2 => DceSecurityGuidGenerator.Instance,
                Version3 => NameBasedGuidGenerator.MD5Hashing.Instance,
                Version4 => RandomizedGuidGenerator.Instance,
                Version5 => NameBasedGuidGenerator.SHA1Hashing.Instance,
                _ => throw new InvalidEnumArgumentException()
            };
        }

        /// <summary>
        /// 生成一个指定版本的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="version">要生成的 <see cref="Guid"/> 的版本。</param>
        /// <returns>一个版本为 <paramref name="version"/> 的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="version"/> 为 <see cref="GuidVersion.Unknown"/>。</exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="version"/> 不为有效的 <see cref="GuidVersion"/> 枚举值。</exception>
        public static Guid NewGuid(GuidVersion version)
        {
            return GuidGenerator.OfVersion(version).NewGuid();
        }

        /// <summary>
        /// 根据命名空间和名称生成一个指定版本的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="version">要生成的 <see cref="Guid"/> 的版本。</param>
        /// <param name="ns">生成 <see cref="Guid"/> 时使用的命名空间。</param>
        /// <param name="name">生成 <see cref="Guid"/> 时使用的名称。</param>
        /// <returns>根据 <paramref name="ns"/> 和 <paramref name="name"/>
        /// 生成的版本为 <paramref name="version"/> 的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="version"/> 为 <see cref="GuidVersion.Unknown"/>。</exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="version"/> 不为有效的 <see cref="GuidVersion"/> 枚举值。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        public static Guid NewGuid(GuidVersion version, Guid ns, string name)
        {
            return GuidGenerator.OfVersion(version).NewGuid(ns, name);
        }
    }
}
