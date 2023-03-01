using System;
using System.Security.Cryptography;

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
    /// Gets the <see cref="IGuidGenerator"/> instance of RFC 4122 UUID revision version 6.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of RFC 4122 UUID revision version 6.</returns>
    public static IGuidGenerator Version6 => TimeBasedGuidGenerator.Sequential.Instance;

    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of RFC 4122 UUID revision version 7.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of RFC 4122 UUID revision version 7.</returns>
    public static IGuidGenerator Version7 => UnixTimeBasedGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// example implementation (UUIDREV Appendix C.7).
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// example implementation (UUIDREV Appendix C.7).</returns>
    public static IGuidGenerator Version8 => CustomGuidGenerator.Example.Instance;

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
    /// Gets the <see cref="IGuidGenerator"/> instance of RFC 4122 UUID revision version 6
    /// using a physical (IEEE 802 MAC) address node ID.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of RFC 4122 UUID revision version 6
    /// using a physical (IEEE 802 MAC) address node ID.</returns>
    public static IGuidGenerator Version6P => TimeBasedGuidGenerator.Sequential.InstanceP;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-256 hashspace ID and hashing algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-256 hashspace ID and hashing algorithm.</returns>
    public static INameBasedGuidGenerator Version8NSha256 => NameBasedGuidGenerator.CustomHashing.InstanceNSha256;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-384 hashspace ID and hashing algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-384 hashspace ID and hashing algorithm.</returns>
    public static INameBasedGuidGenerator Version8NSha384 => NameBasedGuidGenerator.CustomHashing.InstanceNSha384;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-512 hashspace ID and hashing algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-512 hashspace ID and hashing algorithm.</returns>
    public static INameBasedGuidGenerator Version8NSha512 => NameBasedGuidGenerator.CustomHashing.InstanceNSha512;

    /// <summary>
    /// Creates a new <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 1
    /// using a volatile random node ID.
    /// </summary>
    /// <returns>A new <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 1
    /// using a volatile random node ID.</returns>
    public static IGuidGenerator CreateVersion1R() => TimeBasedGuidGenerator.CreateInstanceR();

    /// <summary>
    /// Creates a new <see cref="IGuidGenerator"/> instance of RFC 4122 UUID revision version 6
    /// using a volatile random node ID.
    /// </summary>
    /// <returns>A new <see cref="IGuidGenerator"/> instance of RFC 4122 UUID revision version 6
    /// using a volatile random node ID.</returns>
    public static IGuidGenerator CreateVersion6() => TimeBasedGuidGenerator.Sequential.CreateInstance();

    /// <summary>
    /// Creates a new <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the specified hashspace ID and hashing algorithm.
    /// </summary>
    /// <param name="hashspaceId">The hashspace ID used to identify the hashing algorithm.</param>
    /// <param name="hashing">The hashing algorithm used to transform input values.</param>
    /// <returns>A new <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using <paramref name="hashspaceId"/> and <paramref name="hashing"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashing"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <see cref="HashAlgorithm.HashSize"/> of <paramref name="hashing"/> is less than 128.</exception>
    public static INameBasedGuidGenerator CreateVersion8N(Guid hashspaceId, HashAlgorithm hashing)
    {
        return NameBasedGuidGenerator.CustomHashing.CreateInstance(hashspaceId, hashing);
    }

    /// <summary>
    /// Creates a new <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the specified hashspace ID and hashing algorithm creation delegate.
    /// </summary>
    /// <param name="hashspaceId">The hashspace ID used to identify the hashing algorithm.</param>
    /// <param name="hashingFactory">The delegate used to create the hashing algorithm.</param>
    /// <returns>A new <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using <paramref name="hashspaceId"/> and the hashing algorithm
    /// created by <paramref name="hashingFactory"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashingFactory"/> is <see langword="null"/>.</exception>
    public static INameBasedGuidGenerator CreateVersion8N(Guid hashspaceId, Func<HashAlgorithm> hashingFactory)
    {
        return NameBasedGuidGenerator.CustomHashing.CreateInstance(hashspaceId, hashingFactory);
    }

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
        GuidVersion.Version6 => TimeBasedGuidGenerator.Sequential.Instance,
        GuidVersion.Version7 => UnixTimeBasedGuidGenerator.Instance,
        GuidVersion.Version8 => CustomGuidGenerator.Example.Instance,
        GuidVersion.MaxValue => MaxValueGuidGenerator.Instance,
        _ => throw new ArgumentOutOfRangeException(nameof(version)),
    };
}
