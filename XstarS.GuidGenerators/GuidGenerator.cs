using System;

namespace XstarS.GuidGenerators
{
    /// <summary>
    /// 提供生成 <see cref="Guid"/> 的方法。
    /// </summary>
    public abstract class GuidGenerator
    {
        /// <summary>
        /// 初始化 <see cref="GuidGenerator"/> 类的新实例。
        /// </summary>
        protected GuidGenerator() { }

        /// <summary>
        /// 获取指定 GUID 版本的 <see cref="GuidGenerator"/> 对象。
        /// </summary>
        /// <param name="version">要生成的 <see cref="Guid"/> 的版本。</param>
        /// <returns>版本为 <paramref name="version"/> 的 <see cref="GuidGenerator"/>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="version"/> 不为有效的 <see cref="GuidVersion"/> 枚举值。</exception>
        public static GuidGenerator OfVersion(GuidVersion version)
        {
            return version switch
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
        /// 根据命名空间和名称生成一个指定版本的 <see cref="Guid"/> 实例。
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
        /// 获取当前实例生成的 <see cref="Guid"/> 的版本。
        /// </summary>
        /// <returns>当前实例生成的 <see cref="Guid"/> 的版本。</returns>
        public abstract GuidVersion Version { get; }

        /// <summary>
        /// 获取当前实例生成的 <see cref="Guid"/> 是否依赖于输入参数。
        /// </summary>
        /// <returns>若当前实例生成的 <see cref="Guid"/> 依赖于输入参数，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public virtual bool RequiresInput => this.Version.IsNameBased();

        /// <summary>
        /// 生成一个新的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <returns>一个新的 <see cref="Guid"/> 实例。</returns>
        public abstract Guid NewGuid();

        /// <summary>
        /// 根据命名空间和名称生成一个新的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="ns">生成 <see cref="Guid"/> 时使用的命名空间。</param>
        /// <param name="name">生成 <see cref="Guid"/> 时使用的名称。</param>
        /// <returns>根据 <paramref name="ns"/> 和
        /// <paramref name="name"/> 生成的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        public virtual Guid NewGuid(Guid ns, string name) => this.NewGuid();

        /// <summary>
        /// 填充 <see cref="Guid"/> 中表示 GUID 版本的字段。
        /// </summary>
        /// <param name="guid">要填充字段的 <see cref="Guid"/>。</param>
        protected void FillVersionField(ref Guid guid)
        {
            var shiftVer = (int)this.Version << (3 * 4);
            ref var timeHi_Ver = ref guid.TimeHi_Ver();
            timeHi_Ver = (ushort)(timeHi_Ver & ~0xF000 | shiftVer);
        }

        /// <summary>
        /// 填充 <see cref="Guid"/> 中表示 GUID 变体的字段。
        /// </summary>
        /// <param name="guid">要填充字段的 <see cref="Guid"/>。</param>
        protected virtual void FillVariantField(ref Guid guid)
        {
            const int shiftVar = 0x80;
            ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
            clkSeqHi_Var = (byte)(clkSeqHi_Var & ~0xC0 | shiftVar);
        }
    }
}
