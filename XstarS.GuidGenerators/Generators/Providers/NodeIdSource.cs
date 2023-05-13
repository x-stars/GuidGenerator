namespace XNetEx.Guids.Generators;

/// <summary>
/// Represents the source type of a GUID node ID.
/// </summary>
#if !FEATURE_DISABLE_UUIDREV
public enum NodeIdSource
#else
internal enum NodeIdSource
#endif
{
    /// <summary>
    /// Represents that the node ID is not present.
    /// </summary>
    None = 0,
    /// <summary>
    /// Represents that the node ID is from a physical (IEEE 802 MAC) address of the host.
    /// </summary>
    PhysicalAddress = 1,
    /// <summary>
    /// Represents that the node ID is from a volatile random number.
    /// </summary>
    VolatileRandom = 2,
    /// <summary>
    /// Represents that the node ID is from a non-volatile random number.
    /// </summary>
    NonVolatileRandom = 3,
}

/// <summary>
/// Provides metadata for <see cref="NodeIdSource"/>.
/// </summary>
#if !FEATURE_DISABLE_UUIDREV
public static class NodeIdSourceInfo
#else
internal static class NodeIdSourceInfo
#endif
{
    /// <summary>
    /// Gets a value that indicates whether a node ID from
    /// the <see cref="NodeIdSource"/> should be stored in a non-volatile storage.
    /// </summary>
    /// <param name="source">The <see cref="NodeIdSource"/>.</param>
    /// <returns><see langword="true"/> if a node ID from
    /// the <see cref="NodeIdSource"/> should be stored in a non-volatile storage;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool IsNonVolatile(this NodeIdSource source) =>
        source == NodeIdSource.PhysicalAddress ||
        source == NodeIdSource.NonVolatileRandom;

    /// <summary>
    /// Gets a value that indicates whether a node ID from
    /// the <see cref="NodeIdSource"/> is generated from a random number.
    /// </summary>
    /// <param name="source">The <see cref="NodeIdSource"/>.</param>
    /// <returns><see langword="true"/> if a node ID from
    /// the <see cref="NodeIdSource"/> is generated from a random number;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool IsRandomValue(this NodeIdSource source) =>
        source == NodeIdSource.VolatileRandom ||
        source == NodeIdSource.NonVolatileRandom;
}
