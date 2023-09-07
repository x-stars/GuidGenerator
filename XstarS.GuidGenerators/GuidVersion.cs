using System;

namespace XNetEx.Guids;

/// <summary>
/// Represents the version of a <see cref="Guid"/>.
/// </summary>
public enum GuidVersion : byte
{
    /// <summary>
    /// Represents the version of the nil UUID (<see cref="Guid.Empty"/>).
    /// </summary>
    Empty = 0,
    /// <summary>
    /// Represents RFC 4122 UUID version 1, the time-based version.
    /// </summary>
    Version1 = 1,
    /// <summary>
    /// Represents RFC 4122 UUID version 2, DCE Security version with embedded UIDs.
    /// </summary>
    Version2 = 2,
    /// <summary>
    /// Represents RFC 4122 UUID version 3, the name-based version using the MD5 hashing.
    /// </summary>
    Version3 = 3,
    /// <summary>
    /// Represents RFC 4122 UUID version 4, the randomly generated version.
    /// </summary>
    Version4 = 4,
    /// <summary>
    /// Represents RFC 4122 UUID version 5, the name-based version using the SHA-1 hashing.
    /// </summary>
    Version5 = 5,
#if !UUIDREV_DISABLE
    /// <summary>
    /// Represents RFC 4122 UUID revision version 6, the reordered time-based version.
    /// </summary>
    Version6 = 6,
    /// <summary>
    /// Represents RFC 4122 UUID revision version 7, the Unix Epoch time-based version.
    /// </summary>
    Version7 = 7,
    /// <summary>
    /// Represents RFC 4122 UUID revision version 8, reserved for custom UUID formats.
    /// </summary>
    Version8 = 8,
    /// <summary>
    /// Represents the largest possible value of <see cref="GuidVersion"/>.
    /// </summary>
    MaxValue = 15,
#endif
}

/// <summary>
/// Provides metadata for <see cref="GuidVersion"/>.
/// </summary>
public static class GuidVersionInfo
{
    /// <summary>
    /// Gets a value that indicates whether a <see cref="Guid"/> of
    /// the <see cref="GuidVersion"/> is generated based on the current time.
    /// </summary>
    /// <param name="version">The <see cref="GuidVersion"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="Guid"/> of
    /// <paramref name="version"/> is generated based on the current time;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool IsTimeBased(this GuidVersion version) =>
        version == GuidVersion.Version1 ||
        version == GuidVersion.Version2 ||
#if !UUIDREV_DISABLE
        version == GuidVersion.Version6 ||
        version == GuidVersion.Version7;
#else
        false;
#endif

    /// <summary>
    /// Gets a value that indicates whether a <see cref="Guid"/> of
    /// the <see cref="GuidVersion"/> is generated based on the input name.
    /// </summary>
    /// <param name="version">The <see cref="GuidVersion"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="Guid"/> of
    /// <paramref name="version"/> is generated based on the input name;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool IsNameBased(this GuidVersion version) =>
        version == GuidVersion.Version3 ||
        version == GuidVersion.Version5;

    /// <summary>
    /// Gets a value that indicates whether a <see cref="Guid"/> of
    /// the <see cref="GuidVersion"/> is generated randomly.
    /// </summary>
    /// <param name="version">The <see cref="GuidVersion"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="Guid"/> of
    /// <paramref name="version"/> is generated randomly;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool IsRandomized(this GuidVersion version) =>
        version == GuidVersion.Version4 ||
#if !UUIDREV_DISABLE
        version == GuidVersion.Version7;
#else
        false;
#endif

    /// <summary>
    /// Gets a value that indicates whether a <see cref="Guid"/> of
    /// the <see cref="GuidVersion"/> contains a clock sequence.
    /// </summary>
    /// <param name="version">The <see cref="GuidVersion"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="Guid"/> of
    /// <paramref name="version"/> contains a clock sequence;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool ContainsClockSequence(this GuidVersion version) =>
        version == GuidVersion.Version1 ||
        version == GuidVersion.Version2 ||
#if !UUIDREV_DISABLE
        version == GuidVersion.Version6;
#else
        false;
#endif

    /// <summary>
    /// Gets a value that indicates whether a <see cref="Guid"/> of
    /// the <see cref="GuidVersion"/> contains node ID data.
    /// </summary>
    /// <param name="version">The <see cref="GuidVersion"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="Guid"/> of
    /// <paramref name="version"/> contains node ID data;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool ContainsNodeId(this GuidVersion version) =>
        version == GuidVersion.Version1 ||
        version == GuidVersion.Version2 ||
#if !UUIDREV_DISABLE
        version == GuidVersion.Version6;
#else
        false;
#endif

    /// <summary>
    /// Gets a value that indicates whether a <see cref="Guid"/> of
    /// the <see cref="GuidVersion"/> contains local ID data.
    /// </summary>
    /// <param name="version">The <see cref="GuidVersion"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="Guid"/> of
    /// <paramref name="version"/> contains local ID data;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool ContainsLocalId(this GuidVersion version) =>
        version == GuidVersion.Version2;
}
