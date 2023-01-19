using System;
using System.Text;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
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
    /// <exception cref="NotSupportedException">
    /// This instance does not support generating a name-based <see cref="Guid"/>.</exception>
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
    /// <exception cref="NotSupportedException">
    /// This instance does not support generating a name-based <see cref="Guid"/>.</exception>
    public static Guid NewGuid(this GuidGenerator guidGen,
        Guid nsId, ReadOnlySpan<char> name, Encoding? encoding = null)
    {
        if (guidGen is null)
        {
            throw new ArgumentNullException(nameof(guidGen));
        }
        encoding ??= Encoding.UTF8;

        var nameLength = encoding.GetByteCount(name);
        var nameBytes = (nameLength <= 4096) ?
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
    public static Guid NewGuid(this INameBasedGuidGenerator guidGen,
        Guid nsId, ReadOnlySpan<char> name, Encoding? encoding = null)
    {
        if (guidGen is null)
        {
            throw new ArgumentNullException(nameof(guidGen));
        }
        encoding ??= Encoding.UTF8;

        var nameLength = encoding.GetByteCount(name);
        var nameBytes = (nameLength <= 4096) ?
            (stackalloc byte[nameLength]) : (new byte[nameLength]);
        var bytesWritten = encoding.GetBytes(name, nameBytes);
        Debug.Assert(bytesWritten == nameLength);
        return guidGen.NewGuid(nsId, nameBytes);
    }
#endif
}
