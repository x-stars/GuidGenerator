using System;

namespace XNetEx.Guids.Generators;

partial class GuidGenerator
{
    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of <see cref="Guid.Empty"/>.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of <see cref="Guid.Empty"/>.</returns>
    public static IGuidGenerator Empty => EmptyGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 1.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 1.</returns>
    public static IGuidGenerator Version1 => TimeBasedGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="IDceSecurityGuidGenerator"/> instance of RFC 4122 UUID version 2.
    /// </summary>
    /// <returns>The <see cref="IDceSecurityGuidGenerator"/> instance of RFC 4122 UUID version 2.</returns>
    public static IDceSecurityGuidGenerator Version2 => DceSecurityGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID version 3.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID version 3.</returns>
    public static INameBasedGuidGenerator Version3 => NameBasedGuidGenerator.MD5Hashing.Instance;

    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 4.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 4.</returns>
    public static IGuidGenerator Version4 => RandomizedGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID version 5.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID version 5.</returns>
    public static INameBasedGuidGenerator Version5 => NameBasedGuidGenerator.SHA1Hashing.Instance;

    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of <see cref="Uuid.MaxValue"/>.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of <see cref="Uuid.MaxValue"/>.</returns>
    public static IGuidGenerator MaxValue => MaxValueGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 1
    /// using a non-volatile random node ID.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 1
    /// using a non-volatile random node ID.</returns>
    public static IGuidGenerator Version1R => TimeBasedGuidGenerator.InstanceR;

    /// <summary>
    /// Creates a <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 1
    /// using a volatile random node ID.
    /// </summary>
    /// <returns>A <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 1
    /// using a volatile random node ID.</returns>
    public static IGuidGenerator CreateVersion1R() => TimeBasedGuidGenerator.CreateInstanceR();

    /// <summary>
    /// Gets the <see cref="GuidGenerator"/> instance of the specified <see cref="GuidVersion"/>.
    /// </summary>
    /// <param name="version">The version of the <see cref="Guid"/> to generate.</param>
    /// <returns>The <see cref="GuidGenerator"/> instance of <paramref name="version"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="version"/> is not a valid <see cref="GuidVersion"/> value.</exception>
    public static GuidGenerator OfVersion(GuidVersion version) => version switch
    {
        GuidVersion.Empty => EmptyGuidGenerator.Instance,
        GuidVersion.Version1 => TimeBasedGuidGenerator.Instance,
        GuidVersion.Version2 => DceSecurityGuidGenerator.Instance,
        GuidVersion.Version3 => NameBasedGuidGenerator.MD5Hashing.Instance,
        GuidVersion.Version4 => RandomizedGuidGenerator.Instance,
        GuidVersion.Version5 => NameBasedGuidGenerator.SHA1Hashing.Instance,
        GuidVersion.MaxValue => MaxValueGuidGenerator.Instance,
        _ => throw new ArgumentOutOfRangeException(nameof(version)),
    };
}
