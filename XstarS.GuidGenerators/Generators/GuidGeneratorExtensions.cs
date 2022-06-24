using System;
using System.Text;

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
    /// This instance dose not support generating a name-based <see cref="Guid"/>.</exception>
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

        var nameBytes = encoding.GetBytes(name);
        return guidGen.NewGuid(nsId, nameBytes);
    }

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
    /// <exception cref="NotSupportedException">
    /// This instance dose not support generating a name-based <see cref="Guid"/>.</exception>
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

        var nameBytes = encoding.GetBytes(name);
        return guidGen.NewGuid(nsId, nameBytes);
    }
}
