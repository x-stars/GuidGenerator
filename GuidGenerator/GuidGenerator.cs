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
        /// 获取当前实例生成的 <see cref="Guid"/> 的版本。
        /// </summary>
        /// <returns>当前实例生成的 <see cref="Guid"/> 的版本。</returns>
        public abstract GuidVersion Version { get; }

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
    }
}