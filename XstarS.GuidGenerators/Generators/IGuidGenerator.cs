using System;

namespace XNetEx.Guids.Generators;

/// <summary>
/// Provides information of the generated <see cref="Guid"/>.
/// </summary>
public interface IGuidGeneratorInfo
{
    /// <summary>
    /// Gets the version of the <see cref="Guid"/> generated by this instance.
    /// </summary>
    /// <returns>The version of the <see cref="Guid"/> generated by this instance.</returns>
    GuidVersion Version { get; }

    /// <summary>
    /// Gets the variant of the <see cref="Guid"/> generated by this instance.
    /// </summary>
    /// <returns>The variant of the <see cref="Guid"/> generated by this instance.</returns>
    GuidVariant Variant { get; }
}

/// <summary>
/// Provides a method to generate a <see cref="Guid"/>.
/// </summary>
public interface IGuidGenerator : IGuidGeneratorInfo
{
    /// <summary>
    /// Generates a new <see cref="Guid"/> instance.
    /// </summary>
    /// <returns>A new <see cref="Guid"/> instance.</returns>
    Guid NewGuid();
}

/// <summary>
/// Provides a method to generate a name-based <see cref="Guid"/>.
/// </summary>
public interface INameBasedGuidGenerator : IGuidGeneratorInfo
{
    /// <summary>
    /// Generates a new <see cref="Guid"/> instance based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
    /// <param name="name">The name byte array used to generate the <see cref="Guid"/>.</param>
    /// <returns>A new <see cref="Guid"/> instance generated based on
    /// <paramref name="nsId"/> and <paramref name="name"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    Guid NewGuid(Guid nsId, byte[] name);
}

/// <summary>
/// Provides a method to generate a DCE Security <see cref="Guid"/>.
/// </summary>
public interface IDceSecurityGuidGenerator : IGuidGeneratorInfo
{
    /// <summary>
    /// Generates a new <see cref="Guid"/> instance based on the specified DCE Security domain and local ID.
    /// </summary>
    /// <param name="domain">The DCE Security domain used to generate the <see cref="Guid"/>.</param>
    /// <param name="localId">The site-defined local ID used to generate the <see cref="Guid"/>.</param>
    /// <returns>A new <see cref="Guid"/> instance generated based on
    /// <paramref name="domain"/> and <paramref name="localId"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="domain"/> is not a valid <see cref="DceSecurityDomain"/> value.</exception>
    Guid NewGuid(DceSecurityDomain domain, int? localId = null);
}
