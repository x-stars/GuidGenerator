using System;

namespace XstarS.GuidGenerators;

/// <summary>
/// 提供 <see cref="GuidVersion"/> 的相关信息。
/// </summary>
public static class GuidVersionInfo
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
    /// 获取当前 <see cref="GuidVersion"/> 生成的 <see cref="Guid"/> 是否包含节点 ID 信息。
    /// </summary>
    /// <param name="version">要获取信息的 <see cref="GuidVersion"/>。</param>
    /// <returns>若 <paramref name="version"/> 生成的 <see cref="Guid"/> 包含节点 ID 信息，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool ContainsNodeId(this GuidVersion version) =>
        version == GuidVersion.Version1 ||
        version == GuidVersion.Version2;

    /// <summary>
    /// 获取当前 <see cref="GuidVersion"/> 生成的 <see cref="Guid"/> 是否包含本地 ID 信息。
    /// </summary>
    /// <param name="version">要获取信息的 <see cref="GuidVersion"/>。</param>
    /// <returns>若 <paramref name="version"/> 生成的 <see cref="Guid"/> 包含本地 ID 信息，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool ContainsLocalId(this GuidVersion version) =>
        version == GuidVersion.Version2;
}
