using System;

namespace XstarS.GuidGenerators
{
    partial class GuidGenerator
    {
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
        /// 根据指定的命名空间和名称生成一个指定版本的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="version">要生成的 <see cref="Guid"/> 的版本。</param>
        /// <param name="ns">生成 <see cref="Guid"/> 时使用的命名空间。</param>
        /// <param name="name">生成 <see cref="Guid"/> 时使用的名称。</param>
        /// <returns>根据 <paramref name="ns"/> 和 <paramref name="name"/>
        /// 生成的版本为 <paramref name="version"/> 的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="version"/> 不为有效的 <see cref="GuidVersion"/> 枚举值。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        public static Guid NewGuid(GuidVersion version, Guid ns, string name)
        {
            return GuidGenerator.OfVersion(version).NewGuid(ns, name);
        }

        /// <summary>
        /// 生成一个 RFC 4122 UUID 版本 1 的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <returns>一个 RFC 4122 UUID 版本 1 的 <see cref="Guid"/> 实例。</returns>
        public static Guid NewGuidV1()
        {
            return GuidGenerator.NewGuid(GuidVersion.Version1);
        }

        /// <summary>
        /// 根据指定的 DCE Security 域以及本地 ID 生成一个 RFC 4122 UUID 版本 2 的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="domain">生成 <see cref="Guid"/> 使用的 DCE Security 域。</param>
        /// <param name="localID">自定义本地 ID 的 32 位整数。</param>
        /// <returns>一个 RFC 4122 UUID 版本 2 的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="domain"/> 不为有效的 <see cref="DceSecurityDomain"/> 枚举值。</exception>
        public static Guid NewGuidV2(DceSecurityDomain domain, int? localID = null)
        {
            var guidGen = GuidGenerator.OfVersion(GuidVersion.Version2);
            return guidGen.NewGuid(domain, localID);
        }

        /// <summary>
        /// 根据指定的命名空间和名称生成一个 RFC 4122 UUID 版本 3 的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="ns">生成 <see cref="Guid"/> 时使用的命名空间。</param>
        /// <param name="name">生成 <see cref="Guid"/> 时使用的名称。</param>
        /// <returns>根据 <paramref name="ns"/> 和 <paramref name="name"/>
        /// 生成的 RFC 4122 UUID 版本 3 的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        public static Guid NewGuidV3(Guid ns, string name)
        {
            return GuidGenerator.NewGuid(GuidVersion.Version3, ns, name);
        }

        /// <summary>
        /// 生成一个 RFC 4122 UUID 版本 4 的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <returns>一个 RFC 4122 UUID 版本 4 的 <see cref="Guid"/> 实例。</returns>
        public static Guid NewGuidV4()
        {
            return GuidGenerator.NewGuid(GuidVersion.Version4);
        }

        /// <summary>
        /// 根据指定的命名空间和名称生成一个 RFC 4122 UUID 版本 5 的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="ns">生成 <see cref="Guid"/> 时使用的命名空间。</param>
        /// <param name="name">生成 <see cref="Guid"/> 时使用的名称。</param>
        /// <returns>根据 <paramref name="ns"/> 和 <paramref name="name"/>
        /// 生成的 RFC 4122 UUID 版本 5 的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        public static Guid NewGuidV5(Guid ns, string name)
        {
            return GuidGenerator.NewGuid(GuidVersion.Version5, ns, name);
        }
    }
}
