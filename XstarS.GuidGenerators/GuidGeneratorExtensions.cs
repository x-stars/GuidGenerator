using System;
using System.Text;

namespace XstarS.GuidGenerators;

/// <summary>
/// 提供 <see cref="GuidGenerator"/> 的扩展方法。
/// </summary>
public static class GuidGeneratorExtensions
{
    /// <summary>
    /// 根据指定的命名空间 ID 和名称生成一个新的 <see cref="Guid"/> 实例。
    /// </summary>
    /// <param name="guidGen">当前 <see cref="GuidGenerator"/> 实例。</param>
    /// <param name="nsId">生成 <see cref="Guid"/> 时使用的命名空间 ID。</param>
    /// <param name="name">生成 <see cref="Guid"/> 时使用的名称的字符串。</param>
    /// <param name="encoding">指定用于编码名称的 <see cref="Encoding"/>。</param>
    /// <returns>根据 <paramref name="nsId"/> 和以 <paramref name="encoding"/>
    /// 编码的 <paramref name="name"/> 生成的 <see cref="Guid"/> 实例。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="guidGen"/>
    /// 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="NotSupportedException"><paramref name="guidGen"/>
    /// 不支持基于名称的 <see cref="Guid"/> 生成模式。</exception>
    public static Guid NewGuid(this GuidGenerator guidGen,
        Guid nsId, string name, Encoding? encoding = null)
    {
        if (guidGen is null)
        {
            throw new ArgumentNullException(nameof(guidGen));
        }
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        encoding ??= Encoding.UTF8;

        var nameBytes = encoding.GetBytes(name);
        return guidGen.NewGuid(nsId, nameBytes);
    }

    /// <summary>
    /// 根据指定的命名空间 ID 和名称生成一个新的 <see cref="Guid"/> 实例。
    /// </summary>
    /// <param name="guidGen">当前 <see cref="INameBasedGuidGenerator"/> 实例。</param>
    /// <param name="nsId">生成 <see cref="Guid"/> 时使用的命名空间 ID。</param>
    /// <param name="name">生成 <see cref="Guid"/> 时使用的名称的字符串。</param>
    /// <param name="encoding">指定用于编码名称的 <see cref="Encoding"/>。</param>
    /// <returns>根据 <paramref name="nsId"/> 和以 <paramref name="encoding"/>
    /// 编码的 <paramref name="name"/> 生成的 <see cref="Guid"/> 实例。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="guidGen"/>
    /// 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
    public static Guid NewGuid(this INameBasedGuidGenerator guidGen,
        Guid nsId, string name, Encoding? encoding = null)
    {
        if (guidGen is null)
        {
            throw new ArgumentNullException(nameof(guidGen));
        }
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        encoding ??= Encoding.UTF8;

        var nameBytes = encoding.GetBytes(name);
        return guidGen.NewGuid(nsId, nameBytes);
    }
}
