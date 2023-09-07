#if !UUIDREV_DISABLE
using System;
using System.Security.Cryptography;

namespace XNetEx.Guids.Generators;

partial class GuidGenerator
{
    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-256 hashspace ID and hash algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-256 hashspace ID and hash algorithm.</returns>
    public static INameBasedGuidGenerator Version8NSha256 => NameBasedGuidGenerator.CustomHashing.InstanceSha256;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-384 hashspace ID and hash algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-384 hashspace ID and hash algorithm.</returns>
    public static INameBasedGuidGenerator Version8NSha384 => NameBasedGuidGenerator.CustomHashing.InstanceSha384;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-512 hashspace ID and hash algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA-512 hashspace ID and hash algorithm.</returns>
    public static INameBasedGuidGenerator Version8NSha512 => NameBasedGuidGenerator.CustomHashing.InstanceSha512;

#if NET8_0_OR_GREATER
    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA3-256 hashspace ID and hash algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA3-256 hashspace ID and hash algorithm.</returns>
    /// <exception cref="PlatformNotSupportedException">
    /// This platform does not support the SHA3-256 hash algorithm.</exception>
    public static INameBasedGuidGenerator Version8NSha3D256 => NameBasedGuidGenerator.CustomHashing.InstanceSha3D256;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA3-384 hashspace ID and hash algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA3-384 hashspace ID and hash algorithm.</returns>
    /// <exception cref="PlatformNotSupportedException">
    /// This platform does not support the SHA3-384 hash algorithm.</exception>
    public static INameBasedGuidGenerator Version8NSha3D384 => NameBasedGuidGenerator.CustomHashing.InstanceSha3D384;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA3-512 hashspace ID and hash algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHA3-512 hashspace ID and hash algorithm.</returns>
    /// <exception cref="PlatformNotSupportedException">
    /// This platform does not support the SHA3-512 hash algorithm.</exception>
    public static INameBasedGuidGenerator Version8NSha3D512 => NameBasedGuidGenerator.CustomHashing.InstanceSha3D512;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHAKE128 hashspace ID and hash algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHAKE128 hashspace ID and hash algorithm.</returns>
    /// <exception cref="PlatformNotSupportedException">
    /// This platform does not support the SHAKE128 hash algorithm.</exception>
    public static INameBasedGuidGenerator Version8NShake128 => NameBasedGuidGenerator.CustomHashing.InstanceShake128;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHAKE256 hashspace ID and hash algorithm.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the SHAKE256 hashspace ID and hash algorithm.</returns>
    /// <exception cref="PlatformNotSupportedException">
    /// This platform does not support the SHAKE256 hash algorithm.</exception>
    public static INameBasedGuidGenerator Version8NShake256 => NameBasedGuidGenerator.CustomHashing.InstanceShake256;
#endif

    /// <summary>
    /// Creates a new <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the specified hashspace ID and hash algorithm with a synchronization lock.
    /// </summary>
    /// <param name="hashspaceId">The hashspace ID used to identify the hash algorithm.</param>
    /// <param name="hashing">The hash algorithm used to transform input values.</param>
    /// <returns>A new <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using <paramref name="hashspaceId"/> and <paramref name="hashing"/>
    /// with a synchronization lock.</returns>
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
    /// using the specified hashspace ID and hash algorithm creation delegate.
    /// </summary>
    /// <param name="hashspaceId">The hashspace ID used to identify the hash algorithm.</param>
    /// <param name="hashingFactory">The delegate used to create the hash algorithm.</param>
    /// <returns>A new <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using <paramref name="hashspaceId"/> and the hash algorithm
    /// created by <paramref name="hashingFactory"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashingFactory"/> is <see langword="null"/>.</exception>
    public static INameBasedGuidGenerator CreateVersion8N(Guid hashspaceId, Func<HashAlgorithm> hashingFactory)
    {
        return NameBasedGuidGenerator.CustomHashing.CreateInstance(hashspaceId, hashingFactory);
    }

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of the specified hash algorithm.
    /// </summary>
    /// <param name="hashingName">The name of the hash algorithm used to transform input values.</param>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of <paramref name="hashingName"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashingName"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="hashingName"/> does not represent a supported hash algorithm.</exception>
    /// <exception cref="PlatformNotSupportedException">
    /// This platform does not support the specified hash algorithm.</exception>
    public static INameBasedGuidGenerator OfHashAlgorithm(string hashingName) => hashingName switch
    {
        HashAlgorithmNames.MD5 => NameBasedGuidGenerator.MD5Hashing.Instance,
        HashAlgorithmNames.SHA1 => NameBasedGuidGenerator.SHA1Hashing.Instance,
        HashAlgorithmNames.SHA256 => NameBasedGuidGenerator.CustomHashing.InstanceSha256,
        HashAlgorithmNames.SHA384 => NameBasedGuidGenerator.CustomHashing.InstanceSha384,
        HashAlgorithmNames.SHA512 => NameBasedGuidGenerator.CustomHashing.InstanceSha512,
#if NET8_0_OR_GREATER
        HashAlgorithmNames.SHA3_256 => NameBasedGuidGenerator.CustomHashing.InstanceSha3D256,
        HashAlgorithmNames.SHA3_384 => NameBasedGuidGenerator.CustomHashing.InstanceSha3D384,
        HashAlgorithmNames.SHA3_512 => NameBasedGuidGenerator.CustomHashing.InstanceSha3D512,
        HashAlgorithmNames.SHAKE128 => NameBasedGuidGenerator.CustomHashing.InstanceShake128,
        HashAlgorithmNames.SHAKE256 => NameBasedGuidGenerator.CustomHashing.InstanceShake256,
#endif
        null => throw new ArgumentNullException(nameof(hashingName)),
        _ => throw new ArgumentOutOfRangeException(nameof(hashingName)),
    };

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of the specified <see cref="HashAlgorithmName"/>.
    /// </summary>
    /// <param name="hashingName">The name of the hash algorithm used to transform input values.</param>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of <paramref name="hashingName"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="hashingName"/> does not represent a supported hash algorithm.</exception>
    /// <exception cref="PlatformNotSupportedException">
    /// This platform does not support the specified hash algorithm.</exception>
    public static INameBasedGuidGenerator OfHashAlgorithm(HashAlgorithmName hashingName)
    {
        return GuidGenerator.OfHashAlgorithm(hashingName.Name ?? string.Empty);
    }
}
#endif
