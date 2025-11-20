using System;
using System.Text;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
#endif
#if !UUIDREV_DISABLE
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
#endif

namespace XNetEx.Guids.Generators;

/// <summary>
/// Provides extension methods for <see cref="GuidGenerator"/>.
/// </summary>
public static class GuidGeneratorExtensions
{
    /// <summary>
    /// Generates a new <see cref="Guid"/> instance based on the specified namespace ID and name.
    /// </summary>
    /// <param name="guidGen">The <see cref="GuidGenerator"/>.</param>
    /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
    /// <param name="name">The name string used to generate the <see cref="Guid"/>.</param>
    /// <param name="encoding">The <see cref="Encoding"/> used to encode the name string.</param>
    /// <returns>A new <see cref="Guid"/> instance generated based on
    /// <paramref name="nsId"/> and <paramref name="name"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="guidGen"/>
    /// or <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// The provided hash algorithm's hash size is less than 128 bits.</exception>
    /// <exception cref="NotSupportedException">
    /// This instance does not support generating a name-based <see cref="Guid"/>.</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="guidGen"/> has already been disposed.</exception>
    public static Guid NewGuid(this GuidGenerator guidGen,
        Guid nsId, string name, Encoding? encoding = null)
    {
        ArgumentNullException.ThrowIfNull(guidGen);
        ArgumentNullException.ThrowIfNull(name);
        encoding ??= Encoding.UTF8;

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return guidGen.NewGuid(nsId, (ReadOnlySpan<char>)name, encoding);
#else
        var nameBytes = encoding.GetBytes(name);
        return guidGen.NewGuid(nsId, nameBytes);
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Generates a new <see cref="Guid"/> instance based on the specified namespace ID and name.
    /// </summary>
    /// <param name="guidGen">The <see cref="GuidGenerator"/>.</param>
    /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
    /// <param name="name">The name character span used to generate the <see cref="Guid"/>.</param>
    /// <param name="encoding">The <see cref="Encoding"/> used to encode the name string.</param>
    /// <returns>A new <see cref="Guid"/> instance generated based on
    /// <paramref name="nsId"/> and <paramref name="name"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="guidGen"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// The provided hash algorithm's hash size is less than 128 bits.</exception>
    /// <exception cref="NotSupportedException">
    /// This instance does not support generating a name-based <see cref="Guid"/>.</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="guidGen"/> has already been disposed.</exception>
    public static Guid NewGuid(this GuidGenerator guidGen,
        Guid nsId, ReadOnlySpan<char> name, Encoding? encoding = null)
    {
        ArgumentNullException.ThrowIfNull(guidGen);
        encoding ??= Encoding.UTF8;

        var nameLength = encoding.GetByteCount(name);
        var nameBytes = ((uint)nameLength <= 1024) ?
            (stackalloc byte[nameLength]) : (new byte[nameLength]);
        var bytesWritten = encoding.GetBytes(name, nameBytes);
        Debug.Assert(bytesWritten == nameLength);
        return guidGen.NewGuid(nsId, nameBytes);
    }
#endif

    /// <summary>
    /// Generates a new <see cref="Guid"/> instance based on the specified namespace ID and name.
    /// </summary>
    /// <param name="guidGen">The <see cref="INameBasedGuidGenerator"/>.</param>
    /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
    /// <param name="name">The name string used to generate the <see cref="Guid"/>.</param>
    /// <param name="encoding">The <see cref="Encoding"/> used to encode the name string.</param>
    /// <returns>A new <see cref="Guid"/> instance generated based on
    /// <paramref name="nsId"/> and <paramref name="name"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="guidGen"/>
    /// or <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// The provided hash algorithm's hash size is less than 128 bits.</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="guidGen"/> has already been disposed.</exception>
    public static Guid NewGuid(this INameBasedGuidGenerator guidGen,
        Guid nsId, string name, Encoding? encoding = null)
    {
        ArgumentNullException.ThrowIfNull(guidGen);
        ArgumentNullException.ThrowIfNull(name);
        encoding ??= Encoding.UTF8;

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return guidGen.NewGuid(nsId, (ReadOnlySpan<char>)name, encoding);
#else
        var nameBytes = encoding.GetBytes(name);
        return guidGen.NewGuid(nsId, nameBytes);
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Generates a new <see cref="Guid"/> instance based on the specified namespace ID and name.
    /// </summary>
    /// <param name="guidGen">The <see cref="INameBasedGuidGenerator"/>.</param>
    /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
    /// <param name="name">The name character span used to generate the <see cref="Guid"/>.</param>
    /// <param name="encoding">The <see cref="Encoding"/> used to encode the name string.</param>
    /// <returns>A new <see cref="Guid"/> instance generated based on
    /// <paramref name="nsId"/> and <paramref name="name"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="guidGen"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// The provided hash algorithm's hash size is less than 128 bits.</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="guidGen"/> has already been disposed.</exception>
    public static Guid NewGuid(this INameBasedGuidGenerator guidGen,
        Guid nsId, ReadOnlySpan<char> name, Encoding? encoding = null)
    {
        ArgumentNullException.ThrowIfNull(guidGen);
        encoding ??= Encoding.UTF8;

        var nameLength = encoding.GetByteCount(name);
        var nameBytes = ((uint)nameLength <= 1024) ?
            (stackalloc byte[nameLength]) : (new byte[nameLength]);
        var bytesWritten = encoding.GetBytes(name, nameBytes);
        Debug.Assert(bytesWritten == nameLength);
        return guidGen.NewGuid(nsId, nameBytes);
    }
#endif

#if !UUIDREV_DISABLE
    private static readonly ConditionalWeakTable<HashAlgorithm, INameBasedGuidGenerator> Version8NOfHashing = new();

    extension(GuidGenerator)
    {
        /// <summary>
        /// Gets the related <see cref="INameBasedGuidGenerator"/> of the specified <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="hashing">The <see cref="HashAlgorithm"/>.</param>
        /// <returns>The related <see cref="INameBasedGuidGenerator"/> of <paramref name="hashing"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hashing"/> is null.</exception>
        internal static INameBasedGuidGenerator GetVersion8NOf(HashAlgorithm hashing)
        {
            ArgumentNullException.ThrowIfNull(hashing);
            return GuidGeneratorExtensions.Version8NOfHashing.GetValue(hashing, GuidGenerator.CreateVersion8N);
        }
    }
#endif
}
