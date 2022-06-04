using System;

namespace XstarS.GuidGenerators
{
    /// <summary>
    /// 提供生成 <see cref="Guid"/> 的方法。
    /// </summary>
    public abstract partial class GuidGenerator : IGuidGeneratorInfo, IGuidGenerator
    {
        /// <summary>
        /// 初始化 <see cref="GuidGenerator"/> 类的新实例。
        /// </summary>
        protected GuidGenerator() { }

        /// <summary>
        /// 获取当前实例生成的 <see cref="Guid"/> 的版本。
        /// </summary>
        /// <returns>当前实例生成的 <see cref="Guid"/> 的版本。</returns>
        public abstract GuidVersion Version { get; }

        /// <summary>
        /// 获取当前实例生成的 <see cref="Guid"/> 的变体。
        /// </summary>
        /// <returns>当前实例生成的 <see cref="Guid"/> 的变体。</returns>
        public virtual GuidVariant Variant => GuidVariant.RFC4122;

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
        /// 根据指定的命名空间和名称生成一个新的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="ns">生成 <see cref="Guid"/> 时使用的命名空间。</param>
        /// <param name="name">生成 <see cref="Guid"/> 时使用的名称。</param>
        /// <returns>根据 <paramref name="ns"/> 和
        /// <paramref name="name"/> 生成的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        public virtual Guid NewGuid(Guid ns, string name) => this.NewGuid();

        /// <summary>
        /// 根据指定的 DCE Security 域和本地 ID 生成一个新的 <see cref="Guid"/> 实例。
        /// </summary>
        /// <param name="domain">生成 <see cref="Guid"/> 时使用的 DCE Security 域。</param>
        /// <param name="localID">生成 <see cref="Guid"/> 时使用的自定义本地 ID。</param>
        /// <returns>根据 <paramref name="domain"/> 和
        /// <paramref name="localID"/> 生成的 <see cref="Guid"/> 实例。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="domain"/> 不为有效的 <see cref="DceSecurityDomain"/> 枚举值。</exception>
        public virtual Guid NewGuid(DceSecurityDomain domain, int? localID = null) => this.NewGuid();

        /// <summary>
        /// 填充指定的 <see cref="Guid"/> 中表示 GUID 版本的字段。
        /// </summary>
        /// <param name="guid">要填充字段的 <see cref="Guid"/>。</param>
        protected void FillVersionField(ref Guid guid)
        {
            var shiftVer = (int)this.Version << (3 * 4);
            ref var timeHi_Ver = ref guid.TimeHi_Ver();
            timeHi_Ver = (ushort)(timeHi_Ver & ~0xF000 | shiftVer);
        }

        /// <summary>
        /// 填充指定的 <see cref="Guid"/> 中表示 GUID 变体的字段。
        /// </summary>
        /// <param name="guid">要填充字段的 <see cref="Guid"/>。</param>
        protected void FillVariantField(ref Guid guid)
        {
            var shiftVar = -1 << (8 - (int)this.Variant);
            var varMask = 0xE0 & (shiftVar >> 1);
            ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
            clkSeqHi_Var = (byte)(clkSeqHi_Var & ~varMask | shiftVar);
        }
    }
}
