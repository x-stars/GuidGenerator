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

    /// <summary>
    /// Creates a new <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID revision version 8
    /// using the specified hashspace ID and hash algorithm.
    /// </summary>
    /// <param name="hashspaceId">The hashspace ID used to identify the hash algorithm.</param>
    /// <param name="hashing">The hash algorithm used to transform input values.</param>
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
    public static INameBasedGuidGenerator OfHashAlgorithm(string hashingName) => hashingName switch
    {
        nameof(HashAlgorithmName.MD5) => GuidGenerator.Version3,
        nameof(HashAlgorithmName.SHA1) => GuidGenerator.Version5,
        nameof(HashAlgorithmName.SHA256) => GuidGenerator.Version8NSha256,
        nameof(HashAlgorithmName.SHA384) => GuidGenerator.Version8NSha384,
        nameof(HashAlgorithmName.SHA512) => GuidGenerator.Version8NSha512,
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
    public static INameBasedGuidGenerator OfHashAlgorithm(HashAlgorithmName hashingName)
    {
        return GuidGenerator.OfHashAlgorithm(hashingName.Name ?? string.Empty);
    }
}
