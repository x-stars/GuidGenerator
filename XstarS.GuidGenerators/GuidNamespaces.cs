using System;

namespace XstarS.GuidGenerators
{
    /// <summary>
    /// 提供由 RFC 4122 定义的 <see cref="Guid"/> 命名空间。
    /// </summary>
    public static class GuidNamespaces
    {
        /// <summary>
        /// 表示完整域名时的命名空间。
        /// </summary>
        public static readonly Guid DNS = new Guid(
            // 6ba7b810-9dad-11d1-80b4-00c04fd430c8
            0x6ba7b810, 0x9dad, 0x11d1,
            0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

        /// <summary>
        /// 表示 URL 的命名空间。
        /// </summary>
        public static readonly Guid URL = new Guid(
            // 6ba7b811-9dad-11d1-80b4-00c04fd430c8
            0x6ba7b811, 0x9dad, 0x11d1,
            0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

        /// <summary>
        /// 表示 ISO OID 的命名空间。
        /// </summary>
        public static readonly Guid OID = new Guid(
            // 6ba7b812-9dad-11d1-80b4-00c04fd430c8
            0x6ba7b812, 0x9dad, 0x11d1,
            0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

        /// <summary>
        /// 表示 X.500 DN 的命名空间。
        /// </summary>
        public static readonly Guid X500 = new Guid(
            // 6ba7b814-9dad-11d1-80b4-00c04fd430c8
            0x6ba7b814, 0x9dad, 0x11d1,
            0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8);

        /// <summary>
        /// 尝试根据命名空间的名称查找对应的 <see cref="Guid"/> 命名空间。
        /// </summary>
        /// <param name="nsName">要查找的命名空间的名称。</param>
        /// <param name="ns">查找到的 <see cref="Guid"/> 命名空间。
        /// 若无法找到对应名称的命名空间，则为默认值。</param>
        /// <returns>若找到了对应名称的<see cref="Guid"/> 命名空间，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool TryFindByName(string nsName, out Guid ns)
        {
            ns = nsName switch
            {
                nameof(DNS) => GuidNamespaces.DNS,
                nameof(URL) => GuidNamespaces.URL,
                nameof(OID) => GuidNamespaces.OID,
                nameof(X500) => GuidNamespaces.X500,
                _ => Guid.Empty
            };
            return ns != Guid.Empty;
        }
    }
}
